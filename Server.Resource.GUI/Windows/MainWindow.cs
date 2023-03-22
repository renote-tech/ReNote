using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Server.Resource.GUI.Enums;
using Server.Resource.GUI.Utils;

namespace Server.Resource.GUI
{
    public partial class MainWindow : Form
    {
        private byte[] _dataArray;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            bool isUserId = permissionComboBox.SelectedIndex == 2;
            userIdBox.Enabled = isUserId;
            userSelectorButton.Enabled = isUserId;
        }

        private void OnImportButtonClicked(object sender, EventArgs e)
        {
            importFileDialog.ShowDialog();
        }

        private void OnUserOpenedFile(object sender, CancelEventArgs e)
        {
            if (importFileDialog.FileName == null)
                return;

            FileInfo fileInfo = new FileInfo(importFileDialog.FileName);
            bool isText = FileHelper.GetKnownFileType(fileInfo.Extension) == ResourceType.TEXT;

            previewTextBox.Visible = isText;
            previewImageBox.Visible = !isText;

            try
            {
                _dataArray = File.ReadAllBytes(fileInfo.FullName);
                if (isText)
                {
                    previewTextBox.Text = Encoding.UTF8.GetString(_dataArray);
                    return;
                }

                using MemoryStream stream = new MemoryStream(_dataArray);
                previewImageBox.Image = Image.FromStream(stream);
            } catch(Exception ex) {
                MessageBox.Show(ex.ToString(), 
                                "Uh Oh! An error occured",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}
