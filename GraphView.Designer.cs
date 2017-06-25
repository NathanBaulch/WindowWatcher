namespace WindowWatcher
{
    partial class GraphView
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Panel pnlLeft;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.SplitContainer splitContainer1;
            System.Windows.Forms.SplitContainer splitContainer2;
            System.Windows.Forms.Label label5;
            this._fromTime = new System.Windows.Forms.DateTimePicker();
            this._toTime = new System.Windows.Forms.DateTimePicker();
            this._fromDate = new System.Windows.Forms.DateTimePicker();
            this._toDate = new System.Windows.Forms.DateTimePicker();
            this._processList = new System.Windows.Forms.CheckedListBox();
            this._captionList = new System.Windows.Forms.CheckedListBox();
            this._graph = new ZedGraph.ZedGraphControl();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this._categoryList = new System.Windows.Forms.CheckedListBox();
            label1 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            pnlLeft = new System.Windows.Forms.Panel();
            label2 = new System.Windows.Forms.Label();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            label5 = new System.Windows.Forms.Label();
            pnlLeft.SuspendLayout();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
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
            // label4
            // 
            label4.Dock = System.Windows.Forms.DockStyle.Top;
            label4.Location = new System.Drawing.Point(0, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(303, 16);
            label4.TabIndex = 0;
            label4.Text = "Caption:";
            // 
            // label3
            // 
            label3.Dock = System.Windows.Forms.DockStyle.Top;
            label3.Location = new System.Drawing.Point(0, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(303, 16);
            label3.TabIndex = 0;
            label3.Text = "Process:";
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
            pnlLeft.Size = new System.Drawing.Size(303, 60);
            pnlLeft.TabIndex = 0;
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
            this._toTime.Location = new System.Drawing.Point(132, 32);
            this._toTime.Name = "_toTime";
            this._toTime.Size = new System.Drawing.Size(88, 20);
            this._toTime.TabIndex = 5;
            this._toTime.ValueChanged += new System.EventHandler(this.CommonDateValueChanged);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(16, 36);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(23, 13);
            label2.TabIndex = 3;
            label2.Text = "To:";
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
            this._toDate.Location = new System.Drawing.Point(44, 32);
            this._toDate.Name = "_toDate";
            this._toDate.Size = new System.Drawing.Size(80, 20);
            this._toDate.TabIndex = 4;
            this._toDate.ValueChanged += new System.EventHandler(this.CommonDateValueChanged);
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            splitContainer1.Panel1.Controls.Add(pnlLeft);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(this._graph);
            splitContainer1.Size = new System.Drawing.Size(800, 600);
            splitContainer1.SplitterDistance = 303;
            splitContainer1.TabIndex = 8;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 60);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(this._processList);
            splitContainer2.Panel1.Controls.Add(label3);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            splitContainer2.Size = new System.Drawing.Size(303, 540);
            splitContainer2.SplitterDistance = 177;
            splitContainer2.TabIndex = 0;
            // 
            // _processList
            // 
            this._processList.CheckOnClick = true;
            this._processList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._processList.FormattingEnabled = true;
            this._processList.IntegralHeight = false;
            this._processList.Location = new System.Drawing.Point(0, 16);
            this._processList.Name = "_processList";
            this._processList.Size = new System.Drawing.Size(303, 161);
            this._processList.TabIndex = 1;
            this._processList.SelectedIndexChanged += new System.EventHandler(this.CommonListSelectedIndexChanged);
            this._processList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CommonListItemCheck);
            // 
            // _captionList
            // 
            this._captionList.CheckOnClick = true;
            this._captionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._captionList.FormattingEnabled = true;
            this._captionList.IntegralHeight = false;
            this._captionList.Location = new System.Drawing.Point(0, 16);
            this._captionList.Name = "_captionList";
            this._captionList.Size = new System.Drawing.Size(303, 162);
            this._captionList.TabIndex = 1;
            this._captionList.SelectedIndexChanged += new System.EventHandler(this.CommonListSelectedIndexChanged);
            this._captionList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CommonListItemCheck);
            // 
            // _graph
            // 
            this._graph.Dock = System.Windows.Forms.DockStyle.Fill;
            this._graph.IsEnableHPan = false;
            this._graph.IsEnableHZoom = false;
            this._graph.IsEnableVPan = false;
            this._graph.IsEnableVZoom = false;
            this._graph.IsShowContextMenu = false;
            this._graph.IsShowCopyMessage = false;
            this._graph.Location = new System.Drawing.Point(0, 0);
            this._graph.Name = "_graph";
            this._graph.ScrollGrace = 0;
            this._graph.ScrollMaxX = 0;
            this._graph.ScrollMaxY = 0;
            this._graph.ScrollMaxY2 = 0;
            this._graph.ScrollMinX = 0;
            this._graph.ScrollMinY = 0;
            this._graph.ScrollMinY2 = 0;
            this._graph.Size = new System.Drawing.Size(493, 600);
            this._graph.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this._captionList);
            this.splitContainer3.Panel1.Controls.Add(label4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this._categoryList);
            this.splitContainer3.Panel2.Controls.Add(label5);
            this.splitContainer3.Size = new System.Drawing.Size(303, 359);
            this.splitContainer3.SplitterDistance = 178;
            this.splitContainer3.TabIndex = 2;
            // 
            // _categoryList
            // 
            this._categoryList.CheckOnClick = true;
            this._categoryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._categoryList.FormattingEnabled = true;
            this._categoryList.IntegralHeight = false;
            this._categoryList.Location = new System.Drawing.Point(0, 16);
            this._categoryList.Name = "_categoryList";
            this._categoryList.Size = new System.Drawing.Size(303, 161);
            this._categoryList.TabIndex = 1;
            this._categoryList.SelectedIndexChanged += new System.EventHandler(this.CommonListSelectedIndexChanged);
            this._categoryList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CommonListItemCheck);
            // 
            // label5
            // 
            label5.Dock = System.Windows.Forms.DockStyle.Top;
            label5.Location = new System.Drawing.Point(0, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(303, 16);
            label5.TabIndex = 0;
            label5.Text = "Category:";
            // 
            // GraphView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer1);
            this.Name = "GraphView";
            this.Size = new System.Drawing.Size(800, 600);
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker _fromDate;
        private System.Windows.Forms.DateTimePicker _toDate;
        private ZedGraph.ZedGraphControl _graph;
        private System.Windows.Forms.DateTimePicker _fromTime;
        private System.Windows.Forms.DateTimePicker _toTime;
        private System.Windows.Forms.CheckedListBox _captionList;
        private System.Windows.Forms.CheckedListBox _processList;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.CheckedListBox _categoryList;

    }
}
