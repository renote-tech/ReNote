namespace Server.Database.Windows
{
    partial class RenameContainerDialog
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
            this.newContainerNameLabel = new System.Windows.Forms.Label();
            this.newContainerNameTB = new System.Windows.Forms.TextBox();
            this.createButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.dbPathLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // newContainerNameLabel
            // 
            this.newContainerNameLabel.AutoSize = true;
            this.newContainerNameLabel.Location = new System.Drawing.Point(12, 9);
            this.newContainerNameLabel.Name = "newContainerNameLabel";
            this.newContainerNameLabel.Size = new System.Drawing.Size(67, 15);
            this.newContainerNameLabel.TabIndex = 0;
            this.newContainerNameLabel.Text = "New name:";
            // 
            // newContainerNameTB
            // 
            this.newContainerNameTB.Location = new System.Drawing.Point(12, 27);
            this.newContainerNameTB.Name = "newContainerNameTB";
            this.newContainerNameTB.Size = new System.Drawing.Size(380, 23);
            this.newContainerNameTB.TabIndex = 1;
            this.newContainerNameTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTextBoxKeyDown);
            this.newContainerNameTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnTextBoxKeyPressed);
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(302, 86);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(90, 23);
            this.createButton.TabIndex = 2;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.OnCreateButtonClicked);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(12, 86);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.OnCancelButtonClicked);
            // 
            // dbPathLabel
            // 
            this.dbPathLabel.AutoSize = true;
            this.dbPathLabel.Location = new System.Drawing.Point(12, 60);
            this.dbPathLabel.Name = "dbPathLabel";
            this.dbPathLabel.Size = new System.Drawing.Size(43, 15);
            this.dbPathLabel.TabIndex = 4;
            this.dbPathLabel.Text = "Edit in:";
            // 
            // RenameContainerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(404, 121);
            this.Controls.Add(this.dbPathLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.newContainerNameTB);
            this.Controls.Add(this.newContainerNameLabel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(420, 160);
            this.MinimumSize = new System.Drawing.Size(420, 160);
            this.Name = "RenameContainerDialog";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rename an existing Container...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label newContainerNameLabel;
        private System.Windows.Forms.TextBox newContainerNameTB;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label dbPathLabel;
    }
}