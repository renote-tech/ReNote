namespace Server.Database.Windows
{
    partial class EditItemDialog
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
            this.itemNameLabel = new System.Windows.Forms.Label();
            this.itemNameTB = new System.Windows.Forms.TextBox();
            this.editButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.dbPathLabel = new System.Windows.Forms.Label();
            this.itemValueTB = new System.Windows.Forms.TextBox();
            this.itemValueLabel = new System.Windows.Forms.Label();
            this.openEditorButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // itemNameLabel
            // 
            this.itemNameLabel.AutoSize = true;
            this.itemNameLabel.Location = new System.Drawing.Point(12, 9);
            this.itemNameLabel.Name = "itemNameLabel";
            this.itemNameLabel.Size = new System.Drawing.Size(42, 15);
            this.itemNameLabel.TabIndex = 0;
            this.itemNameLabel.Text = "Name:";
            // 
            // itemNameTB
            // 
            this.itemNameTB.Location = new System.Drawing.Point(12, 27);
            this.itemNameTB.Name = "itemNameTB";
            this.itemNameTB.Size = new System.Drawing.Size(380, 23);
            this.itemNameTB.TabIndex = 1;
            this.itemNameTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnTextBoxNameKeyPressed);
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(302, 136);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(90, 23);
            this.editButton.TabIndex = 5;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.OnCreateButtonClicked);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(12, 136);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.OnCancelButtonClicked);
            // 
            // dbPathLabel
            // 
            this.dbPathLabel.AutoSize = true;
            this.dbPathLabel.Location = new System.Drawing.Point(12, 112);
            this.dbPathLabel.Name = "dbPathLabel";
            this.dbPathLabel.Size = new System.Drawing.Size(46, 15);
            this.dbPathLabel.TabIndex = 6;
            this.dbPathLabel.Text = "Edit in: ";
            // 
            // itemValueTB
            // 
            this.itemValueTB.Location = new System.Drawing.Point(12, 78);
            this.itemValueTB.Name = "itemValueTB";
            this.itemValueTB.Size = new System.Drawing.Size(336, 23);
            this.itemValueTB.TabIndex = 4;
            this.itemValueTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTextBoxKeyDown);
            // 
            // itemValueLabel
            // 
            this.itemValueLabel.AutoSize = true;
            this.itemValueLabel.Location = new System.Drawing.Point(12, 60);
            this.itemValueLabel.Name = "itemValueLabel";
            this.itemValueLabel.Size = new System.Drawing.Size(38, 15);
            this.itemValueLabel.TabIndex = 3;
            this.itemValueLabel.Text = "Value:";
            // 
            // openEditorButton
            // 
            this.openEditorButton.Location = new System.Drawing.Point(354, 78);
            this.openEditorButton.Name = "openEditorButton";
            this.openEditorButton.Size = new System.Drawing.Size(38, 23);
            this.openEditorButton.TabIndex = 10;
            this.openEditorButton.Text = "...";
            this.openEditorButton.UseVisualStyleBackColor = true;
            this.openEditorButton.Click += new System.EventHandler(this.OnOpenEditorClicked);
            // 
            // EditItemDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(404, 171);
            this.Controls.Add(this.openEditorButton);
            this.Controls.Add(this.itemValueTB);
            this.Controls.Add(this.itemValueLabel);
            this.Controls.Add(this.dbPathLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.itemNameTB);
            this.Controls.Add(this.itemNameLabel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(420, 210);
            this.MinimumSize = new System.Drawing.Size(420, 210);
            this.Name = "EditItemDialog";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit  an existing Item...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label itemNameLabel;
        private System.Windows.Forms.TextBox itemNameTB;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label dbPathLabel;
        private System.Windows.Forms.TextBox itemValueTB;
        private System.Windows.Forms.Label itemValueLabel;
        private System.Windows.Forms.Button openEditorButton;
    }
}