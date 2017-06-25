namespace WindowWatcher
{
    partial class NewCategoryDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Button _okButton;
            System.Windows.Forms.Button _cancelButton;
            this._nameBox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            _okButton = new System.Windows.Forms.Button();
            _cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(32, 24);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(38, 13);
            label1.TabIndex = 0;
            label1.Text = "Name:";
            // 
            // _nameBox
            // 
            this._nameBox.Location = new System.Drawing.Point(76, 20);
            this._nameBox.Name = "_nameBox";
            this._nameBox.Size = new System.Drawing.Size(192, 20);
            this._nameBox.TabIndex = 1;
            // 
            // _okButton
            // 
            _okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            _okButton.Location = new System.Drawing.Point(68, 60);
            _okButton.Name = "_okButton";
            _okButton.Size = new System.Drawing.Size(75, 23);
            _okButton.TabIndex = 2;
            _okButton.Text = "OK";
            _okButton.UseVisualStyleBackColor = true;
            // 
            // _cancelButton
            // 
            _cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _cancelButton.Location = new System.Drawing.Point(148, 60);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Size = new System.Drawing.Size(75, 23);
            _cancelButton.TabIndex = 3;
            _cancelButton.Text = "Cancel";
            _cancelButton.UseVisualStyleBackColor = true;
            // 
            // NewCategoryDialog
            // 
            this.AcceptButton = _okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = _cancelButton;
            this.ClientSize = new System.Drawing.Size(292, 104);
            this.ControlBox = false;
            this.Controls.Add(_cancelButton);
            this.Controls.Add(_okButton);
            this.Controls.Add(this._nameBox);
            this.Controls.Add(label1);
            this.Name = "NewCategoryDialog";
            this.ShowInTaskbar = false;
            this.Text = "New Category";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _nameBox;
    }
}