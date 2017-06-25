using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace WindowWatcher
{
    public partial class CategoryView : BaseView
    {
        private BayesianClassifier _classifier;
        private Dictionary<string, Color> _colorMap;
        private IList<Log> _explicitLogs;
        private IList<Log> _guessedLogs;
        private bool _ignoreDateValueChanged;
        private LogRepository _repository;

        public CategoryView()
        {
            InitializeComponent();

            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                _grid,
                new object[] {true});
        }

        public override void Initialize(LogRepository repository)
        {
            _repository = repository;
            Text = "Category";
            var from = DateTime.Now.AddHours(-6).Date.AddHours(6);
            var to = from.AddDays(1);
            _ignoreDateValueChanged = true;
            _fromDate.Value = from.Date;
            _fromTime.Value = _fromTime.MinDate.Add(from.TimeOfDay);
            _toDate.Value = to.Date;
            _toTime.Value = _toTime.MinDate.Add(to.TimeOfDay);
            _ignoreDateValueChanged = false;

            _colorMap = new Dictionary<string, Color>();
            _classifier = new BayesianClassifier();
            var logs = _repository.GetClassified().ToList();

            foreach (var log in logs)
            {
                Teach(log);
            }

            var categoryMenuItems = logs.Select(log => log.Category)
                .Distinct()
                .OrderBy(category => category)
                .Reverse()
                .Select(category =>
                {
                    var color = GenerateColor(_colorMap.Count);
                    _colorMap.Add(category, color);
                    var menuItem = new ToolStripMenuItem
                    {
                        Text = category,
                        BackColor = color
                    };
                    menuItem.Click += CommonCategoryMenuItemClick;
                    return menuItem;
                });
            foreach (var menuItem in categoryMenuItems)
            {
                _contextMenuStrip.Items.Insert(0, menuItem);
            }

            _grid.AutoGenerateColumns = false;
            RefreshGrid();
        }

        private void CommonCategoryMenuItemClick(object sender, EventArgs e)
        {
            ApplyCategory(((ToolStripItem) sender).Text);
        }

        private void ApplyCategory(string category)
        {
            using (_repository.OpenConnection(true))
            {
                foreach (var log in _grid.SelectedRows.Cast<DataGridViewRow>().Select(row => (Log) row.DataBoundItem))
                {
                    if (!_explicitLogs.Contains(log))
                    {
                        _explicitLogs.Add(log);
                        _guessedLogs.Remove(log);
                    }
                    else if (log.Category != category)
                    {
                        _classifier.Unteach($"{log.Process} {log.Caption}", log.Category);
                    }
                    else
                    {
                        return;
                    }

                    log.Category = category;
                    log.Certainty = 1;
                    Teach(log);
                    _repository.UpdateCategory(log);
                }
            }

            foreach (var log in _guessedLogs)
            {
                ClassifyLog(log);
            }

            _grid.Refresh();
        }

        private void Teach(Log log)
        {
            _classifier.Teach($"{log.Process} {log.Caption}", log.Category);
        }

        private void _newCategoryButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new NewCategoryDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var category = dialog.CategoryName;
                    int i;

                    for (i = 0; i < _contextMenuStrip.Items.Count - 1; i++)
                    {
                        var item = _contextMenuStrip.Items[i];

                        if (string.Compare(category, item.Text) < 0)
                        {
                            break;
                        }
                    }

                    var color = GenerateColor(_colorMap.Count);
                    _colorMap.Add(category, color);
                    var menuItem = new ToolStripMenuItem
                    {
                        Text = category,
                        BackColor = color
                    };
                    menuItem.Click += CommonCategoryMenuItemClick;
                    _contextMenuStrip.Items.Insert(i, menuItem);

                    ApplyCategory(category);
                }
            }
        }

        private void _deleteButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet implemented", "Window Watcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void CommonDateValueChanged(object sender, EventArgs e)
        {
            if (!_ignoreDateValueChanged)
            {
                RefreshGrid();
            }
        }

        private void RefreshGrid()
        {
            var from = _fromDate.Value.Date.Add(_fromTime.Value.TimeOfDay);
            var to = _toDate.Value.Date.Add(_toTime.Value.TimeOfDay);
            var logs = _repository.GetSummary(from, to).ToList();
            _explicitLogs = logs.Where(log => !string.IsNullOrEmpty(log.Category)).ToList();

            foreach (var log in _explicitLogs)
            {
                log.Certainty = 1;
            }

            _guessedLogs = logs.Where(log => string.IsNullOrEmpty(log.Category)).ToList();

            foreach (var log in _guessedLogs)
            {
                ClassifyLog(log);
            }

            _grid.DataSource = new SortableBindingList<Log>(logs);
            _grid.Refresh();
        }

        private void ClassifyLog(Log log)
        {
            var classification = _classifier.Classify($"{log.Process} {log.Caption}");
            log.Category = classification.Category;
            log.Certainty = classification.Certainty;
        }

        private void _contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var enabled = _grid.SelectedRows.Count > 0;

            foreach (var item in _contextMenuStrip.Items.Cast<ToolStripItem>())
            {
                item.Enabled = enabled;
            }
        }

        private void _grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.Button == MouseButtons.Right)
            {
                if (!_grid.Rows[e.RowIndex].Selected)
                {
                    if ((ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        _grid.Rows[e.RowIndex].Selected = true;
                    }
                    else
                    {
                        foreach (var row in _grid.Rows.Cast<DataGridViewRow>())
                        {
                            row.Selected = row.Index == e.RowIndex;
                        }
                    }
                }

                var r = _grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                _contextMenuStrip.Show(_grid, r.Left + e.X, r.Top + e.Y);
            }
        }

        private static Color GenerateColor(int index)
        {
            index += 2;
            var r = Math.Max(Math.Min((int) Math.Round(200 + 100 * Math.Sin(index + 0 * Math.PI / 3)), 250), 150);
            var g = Math.Max(Math.Min((int) Math.Round(200 + 100 * Math.Sin(index + 1 * Math.PI / 3)), 250), 150);
            var b = Math.Max(Math.Min((int) Math.Round(200 + 100 * Math.Sin(index + 2 * Math.PI / 3)), 250), 150);
            return Color.FromArgb(0xFF, r, g, b);
        }

        private void _grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var columnName = _grid.Columns[e.ColumnIndex].Name;

            switch (columnName)
            {
                case "Category":
                    var row = _grid.Rows[e.RowIndex];
                    var log = (Log) row.DataBoundItem;
                    e.CellStyle.Font = new Font(DefaultFont, _explicitLogs.Contains(log) ? FontStyle.Bold : FontStyle.Regular);
                    row.DefaultCellStyle.BackColor = !string.IsNullOrEmpty(log.Category) ? _colorMap[log.Category] : SystemColors.Window;
                    break;
                case "Certainty":
                    e.Value = $"{e.Value:0.000}";
                    e.FormattingApplied = true;
                    break;
            }
        }

        #region Nested type: SortableBindingList

        private class SortableBindingList<T> : BindingList<T>
        {
            private readonly List<Sort> _sorts = new List<Sort>();

            public SortableBindingList(IList<T> list)
                : base(list)
            {
            }

            protected override bool SupportsSortingCore => true;

            protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
            {
                _sorts.RemoveAll(sort => sort.Prop == prop);
                _sorts.Insert(0, new Sort(prop, direction));
                ((List<T>) Items).Sort(SortItems);
            }

            private int SortItems(T left, T right)
            {
                return _sorts
                    .Select(sort => CompareItems(sort.Prop, sort.Direction == ListSortDirection.Ascending, left, right))
                    .FirstOrDefault(comparison => comparison != 0);
            }

            private static int CompareItems(PropertyDescriptor prop, bool isAscending, T left, T right)
            {
                var leftValue = prop.GetValue(left);
                var rightValue = prop.GetValue(right);
                var leftComparable = leftValue as IComparable;

                if (leftComparable != null)
                {
                    return leftComparable.CompareTo(rightValue) * (isAscending ? 1 : -1);
                }

                var rightComparable = rightValue as IComparable;

                if (rightComparable != null)
                {
                    return rightComparable.CompareTo(leftValue) * (isAscending ? 1 : -1);
                }

                return 0;
            }

            #region Nested type: Sort

            private class Sort
            {
                public Sort(PropertyDescriptor prop, ListSortDirection direction)
                {
                    Prop = prop;
                    Direction = direction;
                }

                public PropertyDescriptor Prop { get; }
                public ListSortDirection Direction { get; }
            }

            #endregion
        }

        #endregion
    }
}