namespace WindowWatcher
{
    partial class SettingsView
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
            this._loadCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // _loadCheckBox
            // 
            this._loadCheckBox.AutoSize = true;
            this._loadCheckBox.Location = new System.Drawing.Point(20, 20);
            this._loadCheckBox.Name = "_loadCheckBox";
            this._loadCheckBox.Size = new System.Drawing.Size(100, 17);
            this._loadCheckBox.TabIndex = 1;
            this._loadCheckBox.Text = "Load at start-up";
            this._loadCheckBox.UseVisualStyleBackColor = true;
            this._loadCheckBox.CheckedChanged += new System.EventHandler(this.chkLoadStartUp_CheckedChanged);
            // 
            // SettingsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._loadCheckBox);
            this.Name = "SettingsView";
            this.Size = new System.Drawing.Size(800, 600);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _loadCheckBox;
    }
}
