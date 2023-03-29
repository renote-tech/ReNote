namespace Server.Database.Windows
{
    partial class NewContainerDialog
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
            this.containerNameLabel = new System.Windows.Forms.Label();
            this.containerNameTB = new System.Windows.Forms.TextBox();
            this.createButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.dbPathLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // containerNameLabel
            // 
            this.containerNameLabel.AutoSize = true;
            this.containerNameLabel.Location = new System.Drawing.Point(12, 9);
            this.containerNameLabel.Name = "containerNameLabel";
            this.containerNameLabel.Size = new System.Drawing.Size(42, 15);
            this.containerNameLabel.TabIndex = 0;
            this.containerNameLabel.Text = "Name:";
            // 
            // containerNameTB
            // 
            this.containerNameTB.Location = new System.Drawing.Point(12, 27);
            this.containerNameTB.Name = "containerNameTB";
            this.containerNameTB.Size = new System.Drawing.Size(380, 23);
            this.containerNameTB.TabIndex = 1;
            this.containerNameTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTextBoxKeyDown);
            this.containerNameTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnTextBoxKeyPressed);
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
            this.dbPathLabel.Size = new System.Drawing.Size(57, 15);
            this.dbPathLabel.TabIndex = 4;
            this.dbPathLabel.Text = "Create in:";
            // 
            // NewContainerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(404, 121);
            this.Controls.Add(this.dbPathLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.containerNameTB);
            this.Controls.Add(this.containerNameLabel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(420, 160);
            this.MinimumSize = new System.Drawing.Size(420, 160);
            this.Name = "NewContainerDialog";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create a new Container...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label containerNameLabel;
        private System.Windows.Forms.TextBox containerNameTB;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label dbPathLabel;
    }
}