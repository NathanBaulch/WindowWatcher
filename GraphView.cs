using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace WindowWatcher
{
    public partial class GraphView : BaseView
    {
        private readonly IList<string> _checkedCaptions = new List<string>();
        private readonly IList<string> _checkedCategories = new List<string>();
        private readonly IList<string> _checkedProcesses = new List<string>();
        private BayesianClassifier _classifier;
        private bool _ignoreDateValueChanged;
        private bool _itemChecked;
        private LogRepository _repository;

        public GraphView()
        {
            Symbol.Default.IsVisible = false;

            using (var g = CreateGraphics())
            {
                if (Math.Abs(g.DpiX - 96) > 0.1)
                {
                    var scale = g.DpiX / 96;
                    Axis.Default.TitleFontSize = scale * Axis.Default.TitleFontSize;
                    TextObj.Default.FontSize = scale * TextObj.Default.FontSize;
                    ZedGraph.Scale.Default.FontSize = scale * ZedGraph.Scale.Default.FontSize;
                }
            }

            InitializeComponent();
        }

        public override void Initialize(LogRepository repository)
        {
            _repository = repository;
            Text = "Graph";

            var pane = _graph.GraphPane;
            pane.Title.Text = null;
            pane.XAxis.Title.Text = "Time";
            pane.XAxis.Type = AxisType.Date;
            pane.YAxis.Title.Text = "Percent";
            pane.YAxis.Scale.Min = 0;
            pane.YAxis.Scale.Max = 100;
            pane.IsFontsScaled = false;

            var date = DateTime.Now.AddHours(-6).Date;
            var time = _fromTime.MinDate.AddHours(6);
            _ignoreDateValueChanged = true;
            _fromDate.Value = date;
            _fromTime.Value = time;
            _toDate.Value = date.AddDays(1);
            _toTime.Value = time;
            _ignoreDateValueChanged = false;
        }

        public override void Selected()
        {
            foreach (string item in _processList.Items)
            {
                if (_processList.CheckedItems.Contains(item))
                {
                    if (!_checkedProcesses.Contains(item))
                    {
                        _checkedProcesses.Add(item);
                    }
                }
                else
                {
                    if (_checkedProcesses.Contains(item))
                    {
                        _checkedProcesses.Remove(item);
                    }
                }
            }

            foreach (string item in _captionList.Items)
            {
                if (_captionList.CheckedItems.Contains(item))
                {
                    if (!_checkedCaptions.Contains(item))
                    {
                        _checkedCaptions.Add(item);
                    }
                }
                else
                {
                    if (_checkedCaptions.Contains(item))
                    {
                        _checkedCaptions.Remove(item);
                    }
                }
            }

            foreach (string item in _categoryList.Items)
            {
                if (_categoryList.CheckedItems.Contains(item))
                {
                    if (!_checkedCategories.Contains(item))
                    {
                        _checkedCategories.Add(item);
                    }
                }
                else
                {
                    if (_checkedCategories.Contains(item))
                    {
                        _checkedCategories.Remove(item);
                    }
                }
            }

            _processList.Items.Clear();
            _captionList.Items.Clear();
            _categoryList.Items.Clear();

            var from = _fromDate.Value.Date.Add(_fromTime.Value.TimeOfDay);
            var to = _toDate.Value.Date.Add(_toTime.Value.TimeOfDay);

            using (_repository.OpenConnection())
            {
                foreach (var item in _repository.GetDistinctProcesses(from, to))
                {
                    _processList.Items.Add(item, _checkedProcesses.Contains(item));
                }

                foreach (var item in _repository.GetDistinctCaptions(from, to))
                {
                    _captionList.Items.Add(item, _checkedCaptions.Contains(item));
                }

                _classifier = new BayesianClassifier();
                var logs = _repository.GetClassified().ToList();

                foreach (var log in logs)
                {
                    _classifier.Teach($"{log.Process} {log.Caption}", log.Category);
                }

                foreach (var item in logs.Select(log => log.Category).Distinct().OrderBy(category => category))
                {
                    _categoryList.Items.Add(item, _checkedCaptions.Contains(item));
                }
            }

            RefreshGraph();
        }

        private void CommonDateValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreDateValueChanged)
            {
                Selected();
            }
        }

        private void CommonListItemCheck(object sender, ItemCheckEventArgs e)
        {
            _itemChecked = true;
        }

        private void CommonListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_itemChecked)
            {
                RefreshGraph();
                _itemChecked = false;
            }
        }

        private void RefreshGraph()
        {
            using (new CursorScope(Cursors.WaitCursor))
            {
                var pane = _graph.GraphPane;
                pane.CurveList.Clear();

                var filter = new StringBuilder();
                var parameters = new Dictionary<string, object>();
                filter.Append(
                    "START >= ? " +
                    "and END < ? ");
                parameters.Add("START", _fromDate.Value.Date.Add(_fromTime.Value.TimeOfDay));
                parameters.Add("END", _toDate.Value.Date.Add(_toTime.Value.TimeOfDay));

                var processCount = _processList.CheckedItems.Count;
                var captionCount = _captionList.CheckedItems.Count;
                var categoryCount = _categoryList.CheckedItems.Count;

                if (processCount + captionCount + categoryCount > 0)
                {
                    filter.Append("and (");

                    for (var i = 0; i < processCount; i++)
                    {
                        filter.Append((i == 0 ? "PROCESS in (" : ", ") + "?");
                        parameters.Add("PROCESS" + i, _processList.CheckedItems[i]);

                        if (i == processCount - 1)
                        {
                            filter.Append(") ");
                        }
                    }

                    for (var i = 0; i < captionCount; i++)
                    {
                        if (i == 0)
                        {
                            if (processCount > 0)
                            {
                                filter.Append("or ");
                            }

                            filter.Append("CAPTION in (?");
                        }
                        else
                        {
                            filter.Append(", ?");
                        }

                        parameters.Add("CAPTION" + i, _captionList.CheckedItems[i]);

                        if (i == captionCount - 1)
                        {
                            filter.Append(") ");
                        }
                    }

                    if (categoryCount > 0)
                    {
                        if (processCount + captionCount > 0)
                        {
                            filter.Append("or ");
                        }

                        filter.Append("CATEGORY is null or ");

                        for (var i = 0; i < categoryCount; i++)
                        {
                            filter.Append((i == 0 ? "CATEGORY in (" : ", ") + "?");
                            parameters.Add("CATEGORY" + i, _categoryList.CheckedItems[i]);

                            if (i == categoryCount - 1)
                            {
                                filter.Append(") ");
                            }
                        }
                    }

                    filter.Append(") ");
                }

                var idleList = new PointPairList();
                var activeList = new PointPairList();
                var minDate = DateTime.MaxValue;
                var maxDate = DateTime.MinValue;
                var lastEnd = DateTime.MinValue;
                var timeEstimate = TimeSpan.Zero;

                var logs = _repository.GetByFilter(filter.ToString(), parameters)
                    .GroupBy(log => new Log {Start = log.Start, End = log.End})
                    .Select(CalculateGroupAggregates);

                foreach (var log in logs)
                {
                    var start = log.Start.Value;
                    var end = log.End.Value;
                    var idle = log.Idle.Value;
                    var active = log.Active.Value;

                    var scale = 100.0 / (int) (end - start).TotalSeconds;
                    var scaledIdle = idle * scale;
                    var scaledActive = active * scale;

                    if (lastEnd != DateTime.MinValue && lastEnd != start)
                    {
                        idleList.Add(new XDate(lastEnd), 0);
                        idleList.Add(new XDate(start), 0);
                        activeList.Add(new XDate(lastEnd), 0);
                        activeList.Add(new XDate(start), 0);
                    }

                    idleList.Add(new XDate(start), scaledActive + scaledIdle);
                    idleList.Add(new XDate(end), scaledActive + scaledIdle);
                    activeList.Add(new XDate(start), scaledActive);
                    activeList.Add(new XDate(end), scaledActive);

                    lastEnd = end;

                    if (start < minDate)
                    {
                        minDate = start;
                    }

                    if (end > maxDate)
                    {
                        maxDate = end;
                    }

                    if (scaledActive > 10)
                    {
                        timeEstimate += (end - start);
                    }
                }

                const long ticksPerFifteenMinutes = (15 * TimeSpan.TicksPerMinute);
                pane.XAxis.Scale.Min = new XDate(new DateTime(minDate.Ticks - (minDate.Ticks % ticksPerFifteenMinutes)));
                pane.XAxis.Scale.Max = new XDate(new DateTime(maxDate.Ticks - (maxDate.Ticks % ticksPerFifteenMinutes)).AddMinutes(15));

                var estimateList = new PointPairList
                {
                    {new XDate(minDate), 10},
                    {new XDate(maxDate), 10}
                };
                var estimateLine = pane.AddCurve(null, estimateList, Color.Red);
                estimateLine.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;

                var activeLine = pane.AddCurve(null, activeList, Color.LightGreen);
                activeLine.Line.Fill = new Fill(Color.LightGreen);
                activeLine.Symbol.IsVisible = false;

                var idleLine = pane.AddCurve(null, idleList, Color.Pink);
                idleLine.Line.Fill = new Fill(Color.Pink);
                idleLine.Symbol.IsVisible = false;

                timeEstimate = TimeSpan.FromMinutes(Math.Round(timeEstimate.TotalMinutes / 15.0) * 15);

                pane.GraphObjList.Clear();
                pane.GraphObjList.Add(
                    new TextObj(timeEstimate.ToString(), 0.18, 0.40)
                    {
                        Location =
                        {
                            CoordinateFrame = CoordType.PaneFraction,
                            AlignH = AlignH.Center,
                            AlignV = AlignV.Bottom
                        }
                    });
                _graph.AxisChange();
                _graph.Refresh();
            }
        }

        private Log CalculateGroupAggregates(IGrouping<Log, Log> group)
        {
            var filteredGroups = (_categoryList.CheckedItems.Count > 0 ? group.Where(FilterLogCategory) : group).ToList();
            group.Key.Idle = filteredGroups.Sum(item => item.Idle);
            group.Key.Active = filteredGroups.Sum(item => item.Active);
            return group.Key;
        }

        private bool FilterLogCategory(Log item)
        {
            var classification = _classifier.Classify($"{item.Process} {item.Caption}");
            var category = classification.Category;
            return !string.IsNullOrEmpty(category) && _categoryList.CheckedItems.Contains(category);
        }
    }
}