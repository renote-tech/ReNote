using System.Windows.Forms;

namespace Server.Resource.Windows
{
    partial class MainWindow
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
            this.importButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.modifiersBox = new System.Windows.Forms.GroupBox();
            this.userSelectorButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.permissionLabel = new System.Windows.Forms.Label();
            this.userIdBox = new System.Windows.Forms.TextBox();
            this.permissionComboBox = new System.Windows.Forms.ComboBox();
            this.previewBox = new System.Windows.Forms.GroupBox();
            this.previewImageBox = new System.Windows.Forms.PictureBox();
            this.previewTextBox = new System.Windows.Forms.TextBox();
            this.importFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.modifiersBox.SuspendLayout();
            this.previewBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.importButton.Location = new System.Drawing.Point(12, 400);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(152, 36);
            this.importButton.TabIndex = 8;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.OnImportButtonClicked);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(636, 400);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(152, 36);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // modifiersBox
            // 
            this.modifiersBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modifiersBox.Controls.Add(this.userSelectorButton);
            this.modifiersBox.Controls.Add(this.label1);
            this.modifiersBox.Controls.Add(this.permissionLabel);
            this.modifiersBox.Controls.Add(this.userIdBox);
            this.modifiersBox.Controls.Add(this.permissionComboBox);
            this.modifiersBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.modifiersBox.Location = new System.Drawing.Point(636, 15);
            this.modifiersBox.Name = "modifiersBox";
            this.modifiersBox.Size = new System.Drawing.Size(152, 370);
            this.modifiersBox.TabIndex = 6;
            this.modifiersBox.TabStop = false;
            this.modifiersBox.Text = "Modifiers";
            // 
            // userSelectorButton
            // 
            this.userSelectorButton.Enabled = false;
            this.userSelectorButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.userSelectorButton.Location = new System.Drawing.Point(112, 123);
            this.userSelectorButton.Name = "userSelectorButton";
            this.userSelectorButton.Size = new System.Drawing.Size(34, 29);
            this.userSelectorButton.TabIndex = 4;
            this.userSelectorButton.Text = "...";
            this.userSelectorButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(6, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "User ID";
            // 
            // permissionLabel
            // 
            this.permissionLabel.AutoSize = true;
            this.permissionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.permissionLabel.Location = new System.Drawing.Point(6, 42);
            this.permissionLabel.Name = "permissionLabel";
            this.permissionLabel.Size = new System.Drawing.Size(65, 15);
            this.permissionLabel.TabIndex = 2;
            this.permissionLabel.Text = "Permission";
            // 
            // userIdBox
            // 
            this.userIdBox.Enabled = false;
            this.userIdBox.Location = new System.Drawing.Point(6, 123);
            this.userIdBox.MaxLength = 9;
            this.userIdBox.Name = "userIdBox";
            this.userIdBox.Size = new System.Drawing.Size(100, 29);
            this.userIdBox.TabIndex = 1;
            // 
            // permissionComboBox
            // 
            this.permissionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.permissionComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.permissionComboBox.FormattingEnabled = true;
            this.permissionComboBox.Items.AddRange(new object[] {
            "Public",
            "Shared",
            "User"});
            this.permissionComboBox.Location = new System.Drawing.Point(6, 62);
            this.permissionComboBox.Name = "permissionComboBox";
            this.permissionComboBox.Size = new System.Drawing.Size(140, 29);
            this.permissionComboBox.TabIndex = 0;
            this.permissionComboBox.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // 
            // previewBox
            // 
            this.previewBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewBox.Controls.Add(this.previewImageBox);
            this.previewBox.Controls.Add(this.previewTextBox);
            this.previewBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.previewBox.Location = new System.Drawing.Point(12, 15);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(618, 370);
            this.previewBox.TabIndex = 5;
            this.previewBox.TabStop = false;
            this.previewBox.Text = "Preview";
            // 
            // previewImageBox
            // 
            this.previewImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.previewImageBox.Location = new System.Drawing.Point(6, 28);
            this.previewImageBox.Name = "previewImageBox";
            this.previewImageBox.Size = new System.Drawing.Size(606, 336);
            this.previewImageBox.TabIndex = 0;
            this.previewImageBox.TabStop = false;
            this.previewImageBox.Visible = false;
            // 
            // previewTextBox
            // 
            this.previewTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.previewTextBox.Location = new System.Drawing.Point(6, 28);
            this.previewTextBox.Multiline = true;
            this.previewTextBox.Name = "previewTextBox";
            this.previewTextBox.ReadOnly = true;
            this.previewTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.previewTextBox.Size = new System.Drawing.Size(606, 336);
            this.previewTextBox.TabIndex = 9;
            this.previewTextBox.Visible = false;
            // 
            // importFileDialog
            // 
            this.importFileDialog.RestoreDirectory = true;
            this.importFileDialog.Title = "Select a file to import...";
            this.importFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OnUserOpenedFile);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.modifiersBox);
            this.Controls.Add(this.previewBox);
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "Modifiers Manager";
            this.modifiersBox.ResumeLayout(false);
            this.modifiersBox.PerformLayout();
            this.previewBox.ResumeLayout(false);
            this.previewBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button importButton;
        private Button saveButton;
        private GroupBox modifiersBox;
        private ComboBox permissionComboBox;
        private GroupBox previewBox;
        private TextBox userIdBox;
        private Label permissionLabel;
        private Label label1;
        private PictureBox previewImageBox;
        private Button userSelectorButton;
        private OpenFileDialog importFileDialog;
        private TextBox previewTextBox;
    }
}