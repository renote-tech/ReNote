namespace Server.Database.Windows
{
    partial class JsonEditorWindow
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
            this.jsonEditor = new System.Windows.Forms.RichTextBox();
            this.editorLabel = new System.Windows.Forms.Label();
            this.confirmButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.detailsLabel = new System.Windows.Forms.Label();
            this.detailsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // jsonEditor
            // 
            this.jsonEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jsonEditor.Location = new System.Drawing.Point(12, 12);
            this.jsonEditor.Name = "jsonEditor";
            this.jsonEditor.Size = new System.Drawing.Size(776, 397);
            this.jsonEditor.TabIndex = 0;
            this.jsonEditor.Text = "";
            this.jsonEditor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnEditorKeyPress);
            // 
            // editorLabel
            // 
            this.editorLabel.AutoSize = true;
            this.editorLabel.Location = new System.Drawing.Point(12, 20);
            this.editorLabel.Name = "editorLabel";
            this.editorLabel.Size = new System.Drawing.Size(0, 15);
            this.editorLabel.TabIndex = 1;
            // 
            // confirmButton
            // 
            this.confirmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.confirmButton.Location = new System.Drawing.Point(713, 415);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(75, 23);
            this.confirmButton.TabIndex = 2;
            this.confirmButton.Text = "OK";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.OnConfirmButtonClicked);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(632, 415);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.OnCancelButtonClicked);
            // 
            // detailsLabel
            // 
            this.detailsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.detailsLabel.AutoSize = true;
            this.detailsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.detailsLabel.Location = new System.Drawing.Point(12, 419);
            this.detailsLabel.Name = "detailsLabel";
            this.detailsLabel.Size = new System.Drawing.Size(161, 15);
            this.detailsLabel.TabIndex = 4;
            this.detailsLabel.Text = "Couldn\'t parse the JSON text.";
            this.detailsLabel.Visible = false;
            // 
            // detailsButton
            // 
            this.detailsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.detailsButton.Location = new System.Drawing.Point(179, 415);
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.Size = new System.Drawing.Size(75, 23);
            this.detailsButton.TabIndex = 5;
            this.detailsButton.Text = "See details";
            this.detailsButton.UseVisualStyleBackColor = true;
            this.detailsButton.Visible = false;
            this.detailsButton.Click += new System.EventHandler(this.OnDetailsButtonClicked);
            // 
            // JsonEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.detailsButton);
            this.Controls.Add(this.detailsLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.editorLabel);
            this.Controls.Add(this.jsonEditor);
            this.DoubleBuffered = true;
            this.Name = "JsonEditorWindow";
            this.ShowIcon = false;
            this.Text = "JSON Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox jsonEditor;
        private System.Windows.Forms.Label editorLabel;
        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label detailsLabel;
        private System.Windows.Forms.Button detailsButton;
    }
}