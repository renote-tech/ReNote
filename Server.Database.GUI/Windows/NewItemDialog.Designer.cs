namespace Server.Database.Windows
{
    partial class NewItemDialog
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
            this.docNameLabel = new System.Windows.Forms.Label();
            this.docNameTextBox = new System.Windows.Forms.TextBox();
            this.createButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.dbCreatedLabel = new System.Windows.Forms.Label();
            this.pathLabel = new System.Windows.Forms.Label();
            this.docValueTextBox = new System.Windows.Forms.TextBox();
            this.docValueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // docNameLabel
            // 
            this.docNameLabel.AutoSize = true;
            this.docNameLabel.Location = new System.Drawing.Point(12, 9);
            this.docNameLabel.Name = "docNameLabel";
            this.docNameLabel.Size = new System.Drawing.Size(42, 15);
            this.docNameLabel.TabIndex = 0;
            this.docNameLabel.Text = "Name:";
            // 
            // docNameTextBox
            // 
            this.docNameTextBox.Location = new System.Drawing.Point(12, 27);
            this.docNameTextBox.Name = "docNameTextBox";
            this.docNameTextBox.Size = new System.Drawing.Size(380, 23);
            this.docNameTextBox.TabIndex = 1;
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(302, 136);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(90, 23);
            this.createButton.TabIndex = 5;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.OnCreateButtonClicked);
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
            // dbCreatedLabel
            // 
            this.dbCreatedLabel.AutoSize = true;
            this.dbCreatedLabel.Location = new System.Drawing.Point(12, 112);
            this.dbCreatedLabel.Name = "dbCreatedLabel";
            this.dbCreatedLabel.Size = new System.Drawing.Size(57, 15);
            this.dbCreatedLabel.TabIndex = 6;
            this.dbCreatedLabel.Text = "Create in:";
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.pathLabel.Location = new System.Drawing.Point(69, 112);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(0, 15);
            this.pathLabel.TabIndex = 7;
            // 
            // docValueTextBox
            // 
            this.docValueTextBox.Location = new System.Drawing.Point(12, 78);
            this.docValueTextBox.Name = "docValueTextBox";
            this.docValueTextBox.Size = new System.Drawing.Size(380, 23);
            this.docValueTextBox.TabIndex = 4;
            // 
            // docValueLabel
            // 
            this.docValueLabel.AutoSize = true;
            this.docValueLabel.Location = new System.Drawing.Point(12, 60);
            this.docValueLabel.Name = "docValueLabel";
            this.docValueLabel.Size = new System.Drawing.Size(38, 15);
            this.docValueLabel.TabIndex = 3;
            this.docValueLabel.Text = "Value:";
            // 
            // NewItemDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(404, 171);
            this.Controls.Add(this.docValueTextBox);
            this.Controls.Add(this.docValueLabel);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.dbCreatedLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.docNameTextBox);
            this.Controls.Add(this.docNameLabel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(420, 210);
            this.MinimumSize = new System.Drawing.Size(420, 210);
            this.Name = "NewDocumentDialog";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create a new Item...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label docNameLabel;
        private System.Windows.Forms.TextBox docNameTextBox;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label dbCreatedLabel;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.TextBox docValueTextBox;
        private System.Windows.Forms.Label docValueLabel;
    }
}