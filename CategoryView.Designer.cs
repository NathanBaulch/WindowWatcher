using System.Windows.Forms;

namespace WindowWatcher
{
    partial class CategoryView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Panel pnlLeft;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.ToolStripMenuItem _newCategoryButton;
            this._fromTime = new System.Windows.Forms.DateTimePicker();
            this._toTime = new System.Windows.Forms.DateTimePicker();
            this._fromDate = new System.Windows.Forms.DateTimePicker();
            this._toDate = new System.Windows.Forms.DateTimePicker();
            this._grid = new System.Windows.Forms.DataGridView();
            this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._deleteButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Process = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Caption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Idle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Active = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Certainty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            pnlLeft = new System.Windows.Forms.Panel();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            _newCategoryButton = new System.Windows.Forms.ToolStripMenuItem();
            pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this._contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(this._fromTime);
            pnlLeft.Controls.Add(this._toTime);
            pnlLeft.Controls.Add(label2);
            pnlLeft.Controls.Add(label1);
            pnlLeft.Controls.Add(this._fromDate);
            pnlLeft.Controls.Add(this._toDate);
            pnlLeft.Dock = System.Windows.Forms.DockStyle.Top;
            pnlLeft.Location = new System.Drawing.Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new System.Drawing.Size(800, 36);
            pnlLeft.TabIndex = 2;
            // 
            // _fromTime
            // 
            this._fromTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this._fromTime.Location = new System.Drawing.Point(132, 8);
            this._fromTime.Name = "_fromTime";
            this._fromTime.Size = new System.Drawing.Size(88, 20);
            this._fromTime.TabIndex = 2;
            this._fromTime.ValueChanged += new System.EventHandler(this.CommonDateValueChanged);
            // 
            // _toTime
            // 
            this._toTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this._toTime.Location = new System.Drawing.Point(352, 8);
            this._toTime.Name = "_toTime";
            this._toTime.Size = new System.Drawing.Size(88, 20);
            this._toTime.TabIndex = 5;
            this._toTime.ValueChanged += new System.EventHandler(this.CommonDateValueChanged);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(236, 12);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(23, 13);
            label2.TabIndex = 3;
            label2.Text = "To:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(8, 12);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(33, 13);
            label1.TabIndex = 0;
            label1.Text = "From:";
            // 
            // _fromDate
            // 
            this._fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._fromDate.Location = new System.Drawing.Point(44, 8);
            this._fromDate.Name = "_fromDate";
            this._fromDate.Size = new System.Drawing.Size(80, 20);
            this._fromDate.TabIndex = 1;
            this._fromDate.ValueChanged += new System.EventHandler(this.CommonDateValueChanged);
            // 
            // _toDate
            // 
            this._toDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._toDate.Location = new System.Drawing.Point(264, 8);
            this._toDate.Name = "_toDate";
            this._toDate.Size = new System.Drawing.Size(80, 20);
            this._toDate.TabIndex = 4;
            this._toDate.ValueChanged += new System.EventHandler(this.CommonDateValueChanged);
            // 
            // _newCategoryButton
            // 
            _newCategoryButton.Name = "_newCategoryButton";
            _newCategoryButton.Size = new System.Drawing.Size(166, 22);
            _newCategoryButton.Text = "New Category...";
            _newCategoryButton.Click += new System.EventHandler(this._newCategoryButton_Click);
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            this._grid.BackgroundColor = System.Drawing.SystemColors.Window;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Process,
            this.Caption,
            this.Idle,
            this.Active,
            this.Total,
            this.Category,
            this.Certainty});
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 36);
            this._grid.Name = "_grid";
            this._grid.RowHeadersVisible = false;
            this._grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(800, 564);
            this._grid.TabIndex = 0;
            this._grid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this._grid_CellMouseClick);
            this._grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this._grid_CellFormatting);
            // 
            // _contextMenuStrip
            // 
            this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            _newCategoryButton,
            this._deleteButton});
            this._contextMenuStrip.Name = "contextMenuStrip1";
            this._contextMenuStrip.Size = new System.Drawing.Size(167, 48);
            this._contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this._contextMenuStrip_Opening);
            // 
            // _deleteButton
            // 
            this._deleteButton.Name = "_deleteButton";
            this._deleteButton.Size = new System.Drawing.Size(166, 22);
            this._deleteButton.Text = "Delete";
            this._deleteButton.Click += new System.EventHandler(this._deleteButton_Click);
            // 
            // Process
            // 
            this.Process.DataPropertyName = "Process";
            this.Process.HeaderText = "Process";
            this.Process.Name = "Process";
            this.Process.ReadOnly = true;
            this.Process.Width = 300;
            // 
            // Caption
            // 
            this.Caption.DataPropertyName = "Caption";
            this.Caption.HeaderText = "Caption";
            this.Caption.Name = "Caption";
            this.Caption.ReadOnly = true;
            this.Caption.Width = 500;
            // 
            // Idle
            // 
            this.Idle.DataPropertyName = "Idle";
            this.Idle.HeaderText = "Idle";
            this.Idle.Name = "Idle";
            this.Idle.ReadOnly = true;
            this.Idle.Width = 60;
            // 
            // Active
            // 
            this.Active.DataPropertyName = "Active";
            this.Active.HeaderText = "Active";
            this.Active.Name = "Active";
            this.Active.ReadOnly = true;
            this.Active.Width = 80;
            // 
            // Total
            // 
            this.Total.DataPropertyName = "Total";
            this.Total.HeaderText = "Total";
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            this.Total.Width = 80;
            // 
            // Category
            // 
            this.Category.DataPropertyName = "Category";
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            this.Category.Width = 100;
            // 
            // Certainty
            // 
            this.Certainty.DataPropertyName = "Certainty";
            this.Certainty.HeaderText = "Certainty";
            this.Certainty.Name = "Certainty";
            this.Certainty.Width = 100;
            // 
            // CategoryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grid);
            this.Controls.Add(pnlLeft);
            this.Name = "CategoryView";
            this.Size = new System.Drawing.Size(800, 600);
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this._contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DateTimePicker _fromTime;
        private System.Windows.Forms.DateTimePicker _toTime;
        private System.Windows.Forms.DateTimePicker _fromDate;
        private System.Windows.Forms.DateTimePicker _toDate;
        private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem _deleteButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Process;
        private System.Windows.Forms.DataGridViewTextBoxColumn Caption;
        private System.Windows.Forms.DataGridViewTextBoxColumn Idle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Active;
        private System.Windows.Forms.DataGridViewTextBoxColumn Total;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Certainty;

    }
}
