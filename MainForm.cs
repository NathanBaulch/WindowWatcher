using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowWatcher.Properties;

namespace WindowWatcher
{
    public partial class MainForm : Form
    {
        private readonly IDictionary<string, IDictionary<string, Log>> _logs;
        private readonly Icon _normalIcon;
        private readonly LogRepository _repository;
        private readonly BaseView[] _views;
        private bool _closing;
        private DateTime _lastTick;
        private DateTime _start;

        public MainForm()
        {
            InitializeComponent();

            Shell32.ExtractIconEx(Application.ExecutablePath, 0, out var _, out var small, 1);
            _normalIcon = Icon.FromHandle(small);
            Icon = _normalIcon;
            _notifyIcon.Icon = _normalIcon;

            var dataFileName = Path.Combine(Application.StartupPath, "logs.db");
            _repository = new LogRepository("Data Source=" + dataFileName);
            var isNew = !File.Exists(dataFileName);

            using (_repository.OpenConnection())
            {
                if (isNew)
                {
                    _repository.CreateTable();
                }
            }

            _logs = new Dictionary<string, IDictionary<string, Log>>();
            _start = DateTime.Now;
            _lastTick = DateTime.Now;
            _closing = false;
            Visible = false;
            _timer.Start();

            _views = new BaseView[]
            {
                new SettingsView(),
                new CategoryView(),
                new GraphView()
            };
            foreach (var view in _views)
            {
                ConfigureView(view);
            }
        }

        private void ConfigureView(BaseView view)
        {
            view.Initialize(_repository);
            var page = new TabPage(view.Text);
            view.Dock = DockStyle.Fill;
            page.Controls.Add(view);
            _tabControl.TabPages.Add(page);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var process = GetActiveProcess();
            string processName;
            string windowCaption;

            if (process == null)
            {
                processName = string.Empty;
                windowCaption = string.Empty;
            }
            else
            {
                processName = process.ProcessName;
                windowCaption = process.MainWindowTitle;
            }

            var now = DateTime.Now;
            var reset = ((now - _lastTick).TotalSeconds > 10);

            if (reset)
            {
                Commit(_lastTick.AddSeconds(1));
                _logs.Clear();
                _start = now.AddSeconds(-1);
            }

            if (!_logs.TryGetValue(processName, out var processLogs))
            {
                processLogs = new Dictionary<string, Log>();
                _logs.Add(processName, processLogs);
            }

            if (!processLogs.TryGetValue(windowCaption, out var log))
            {
                log = new Log
                {
                    Process = processName,
                    Caption = windowCaption
                };
                processLogs.Add(windowCaption, log);
            }

            if (GetLastInputTime() > 1000)
            {
                log.Idle = (log.Idle ?? 0) + 1;
            }
            else
            {
                log.Active = (log.Active ?? 0) + 1;
            }

            if (!reset && (_lastTick.Minute % 15) == 14 && (now.Minute % 15) == 0)
            {
                Commit(now);
                _logs.Clear();
                _start = now;
            }

            _lastTick = now;
        }

        private void Commit(DateTime end)
        {
            using (_repository.OpenConnection(true))
            {
                var miscProcess = new Log
                {
                    Start = _start,
                    End = end,
                    Process = string.Empty,
                    Caption = string.Empty,
                    Idle = ((int) (end - _start).TotalSeconds),
                    Active = 0
                };

                foreach (var processItem in _logs)
                {
                    var miscCaption = new Log
                    {
                        Start = _start,
                        End = end,
                        Process = processItem.Key,
                        Caption = string.Empty,
                        Idle = 0,
                        Active = 0
                    };

                    foreach (var captionItem in processItem.Value)
                    {
                        var log = captionItem.Value;
                        var active = log.Active ?? 0;
                        var idle = log.Idle ?? 0;
                        log.Active = active;
                        log.Idle = idle;
                        var total = active + idle;
                        miscProcess.Idle -= total;

                        if (!string.IsNullOrEmpty(log.Process))
                        {
                            if (total > 5 && !string.IsNullOrEmpty(log.Caption))
                            {
                                log.Start = _start;
                                log.End = end;
                                _repository.Insert(log);
                            }
                            else
                            {
                                miscCaption.Idle += idle;
                                miscCaption.Active += active;
                            }
                        }
                        else
                        {
                            miscProcess.Idle += miscCaption.Idle;
                            miscProcess.Active += miscCaption.Active;
                        }
                    }

                    if (miscCaption.Total >= 10)
                    {
                        _repository.Insert(miscCaption);
                    }
                    else
                    {
                        miscProcess.Idle += miscCaption.Idle;
                        miscProcess.Active += miscCaption.Active;
                    }
                }

                if (miscProcess.Total > 0)
                {
                    _repository.Insert(miscProcess);
                }
            }
        }

        private static int GetLastInputTime()
        {
            var inputInfo = new User32.LASTINPUTINFO();
            inputInfo.cbSize = Marshal.SizeOf(inputInfo);
            inputInfo.dwTime = 0;
            return User32.GetLastInputInfo(ref inputInfo) ? Environment.TickCount - inputInfo.dwTime : 0;
        }

        private static Process GetActiveProcess()
        {
            Process process = null;
            var window = User32.GetForegroundWindow();

            if (window.ToInt32() != 0)
            {
                var parent = User32.GetParent(window);

                if (parent.ToInt32() != 0)
                {
                    window = parent;
                }

                if (User32.IsWindow(window))
                {
                    User32.GetWindowThreadProcessId(window, out var processId);
                    try
                    {
                        process = Process.GetProcessById(processId.ToInt32());
                    }
                    catch (ArgumentException)
                    {
                        //the process has since been closed
                    }
                }
            }

            return process;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (Visible)
            {
                BringToFront();
            }
            else
            {
                Visible = true;
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            var paused = !_pauseButton.Checked;
            _timer.Enabled = !paused;
            _pauseButton.Checked = paused;

            if (paused)
            {
                Icon = Resources.Paused;
                _notifyIcon.Icon = Resources.Paused;
            }
            else
            {
                Icon = _normalIcon;
                _notifyIcon.Icon = _normalIcon;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            _timer.Stop();
            Commit(DateTime.Now);

            foreach (var view in _views)
            {
                view.Destroy();
            }

            _closing = true;
            Application.Exit();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            _views[_tabControl.SelectedIndex].Selected();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_closing)
            {
                e.Cancel = true;
                Visible = false;
            }
        }
    }
}