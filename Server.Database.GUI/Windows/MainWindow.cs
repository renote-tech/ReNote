using Server.Database.Management;
using Server.ReNote.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using RNDatabase = Server.ReNote.Data.Database;

namespace Server.Database.Windows
{
    public partial class MainWindow : Form
    {
        private string m_CurrentDatabase;
        private string m_CurrentContainer;

        private static MainWindow s_Instance;

        public MainWindow()
        {
            InitializeComponent();

            s_Instance = this;

            DatabaseView.ImageList = new ImageList();
            DatabaseView.ImageList.Images.Add(new Bitmap("./assets/drive.png"));
            DatabaseView.ImageList.Images.Add(new Bitmap("./assets/file.png"));
            DatabaseView.ImageList.Images.Add(new Bitmap("./assets/text.png"));
        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(DatabaseView.Handle, "explorer", null);
        }

        private void OnSelectedOpenDBFile(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FileInfo fileInfo = new FileInfo(openDatabaseDialog.FileName);
            string fileName = fileInfo.Name.Replace(fileInfo.Extension, "");

            TreeNode databaseNode = new TreeNode(fileName)
            {
                ContextMenuStrip   = DatabaseItemContextMenu,
                ImageIndex         = 1,
                SelectedImageIndex = 1,
                Tag                = fileName
            };

            RNDatabase database = DatabaseManager.AddDatabase(fileName, fileInfo.FullName);
            if (database == null)
            {
                MessageBox.Show("A database with the same name is already loaded! Unmount the existing one and try again.",
                "Uh Oh!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            int index = DatabaseView.Nodes[0].Nodes.Add(databaseNode);

            Container[] containers = database.GetContainers();
            for(int i = 0; i < containers.Length; i++)
            {
                TreeNode containerNode = new TreeNode(containers[i].Name)
                {
                    ContextMenuStrip   = RootDocumentContextMenu,
                    ImageIndex         = 2,
                    SelectedImageIndex = 2,
                    Tag                = $"{fileName}/{containers[i].Name}"
                };

                DatabaseView.Nodes[0].Nodes[index].Nodes.Add(containerNode);
            }

            DatabaseView.Nodes[0].Expand();
        }

        private void OnSelectedSaveDBFile(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TreeNode treeNode = DatabaseView.SelectedNode;
            if (treeNode.Tag == null)
                return;

            string database = treeNode.Tag.ToString();
            DatabaseManager.SetSaveLocation(database, saveDatabaseDialog.FileName);
            SaveAsync();
        }

        private void OnMountMenuItemClicked(object sender, EventArgs e)
        {
            openDatabaseDialog.ShowDialog();
        }

        private void OnExitMenuItemClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnNewDatabaseMenuItemClicked(object sender, EventArgs e)
        {
            new NewDatabaseDialog().ShowDialog();
        }

        private void OnAddRootDocumentMenuItemClicked(object sender, EventArgs e)
        {
            TreeNode treeNode = DatabaseView.SelectedNode;
            if (treeNode.Tag == null)
                return;

            new NewContainerDialog((string)treeNode.Tag).ShowDialog();
        }

        private void OnAddDocumentMenuItemClicked(object sender, EventArgs e)
        {
            TreeNode treeNode = DatabaseView.SelectedNode;
            if (treeNode.Tag == null)
                return;

            string[] parts = treeNode.Tag.ToString()
                                         .Split('/');
            if (parts.Length != 2)
                return;

            new NewItemDialog(parts[0], parts[1]).ShowDialog();
        }

        private void OnSaveMenuItemClicked(object sender, EventArgs e)
        {
            SaveAsync();
        }

        private void OnAddDocumentDataItemClicked(object sender, EventArgs e)
        {
            new NewItemDialog(m_CurrentDatabase, m_CurrentContainer).ShowDialog();
        }

        private void OnTreeNodeItemClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                DatabaseView.SelectedNode = e.Node;
        }

        private void OnTreeViewItemClicked(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
                return;

            string tag = e.Node.Tag.ToString();
            if (!tag.Contains('/'))
                return;

            string[] parts = tag.Split('/');
            if (parts.Length != 2)
                return;

            Dictionary<string, string> data = DatabaseManager.GetItems(parts[0], parts[1]);
            if (data == null)
                return;

            databaseDataView.Items.Clear();

            for (int i = 0; i < data.Count; i++)
            {
                KeyValuePair<string, string> keyValuePair = data.ElementAt(i);
                ListViewItem dataViewItem = new ListViewItem(keyValuePair.Key);
                dataViewItem.SubItems.Add(keyValuePair.Value);

                databaseDataView.Items.Add(dataViewItem);
            }

            m_CurrentDatabase = parts[0];
            m_CurrentContainer = parts[1];
        }

        private void OnDataViewSizeChanged(object sender, EventArgs e)
        {
            // databaseDataView.Columns[1] represents the value column
            databaseDataView.Columns[1].Width = databaseDataView.Width - 135;
        }

        private void OnDataMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ListViewItem item = databaseDataView.GetItemAt(e.X, e.Y);
            if (item == null)
                databaseDataView.ContextMenuStrip = databaseDataContextMenu;
            else
                databaseDataView.ContextMenuStrip = databaseDataItemContextMenu;
        }

        private void OnDataContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool canCreateItem = !(string.IsNullOrWhiteSpace(m_CurrentDatabase) || string.IsNullOrWhiteSpace(m_CurrentContainer));
            databaseDataContextMenu.Items[0].Enabled = canCreateItem;
        }

        private async void SaveAsync()
        {
            TreeNode treeNode = DatabaseView.SelectedNode;
            if (treeNode.Tag == null)
                return;

            string database = treeNode.Tag.ToString();
            int code = await DatabaseManager.SaveDatabaseAsync(database);
            switch (code)
            {
                case 1:
                    saveDatabaseDialog.ShowDialog();
                    break;
                case 2:
                    MessageBox.Show("In order to save it, please add at least one container.",
                                    "Uh Oh!",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    break;
            }
        }

        public void AddDatabaseNode(string databaseName)
        {
            TreeNode databaseNode = new TreeNode(databaseName)
            {
                ContextMenuStrip   = DatabaseItemContextMenu,
                ImageIndex         = 1,
                SelectedImageIndex = 1,
                Tag                = databaseName
            };

            DatabaseView.Nodes[0].Nodes.Add(databaseNode);
        }

        public void AddContainerNode(string databaseName, string container)
        {
            TreeNode treeNode = null;
            for (int i = 0; i < DatabaseView.Nodes[0].Nodes.Count; i++)
            {
                object tag = DatabaseView.Nodes[0].Nodes[i].Tag;
                if (tag == null || tag.ToString() != databaseName)
                    continue;

                treeNode = DatabaseView.Nodes[0].Nodes[i];
            }

            if (treeNode == null)
                return;

            TreeNode containerNode = new TreeNode(container)
            {
                ContextMenuStrip   = RootDocumentContextMenu,
                ImageIndex         = 2,
                SelectedImageIndex = 2,
                Tag                = $"{databaseName}/{container}"
            };

            treeNode.Nodes.Add(containerNode);
        }

        public void RefreshDataView()
        {
            Dictionary<string, string> data = DatabaseManager.GetItems(m_CurrentDatabase, m_CurrentContainer);
            if (data == null)
                return;

            databaseDataView.Items.Clear();

            for (int i = 0; i < data.Count; i++)
            {
                KeyValuePair<string, string> keyValuePair = data.ElementAt(i);
                ListViewItem dataViewItem = new ListViewItem(keyValuePair.Key);
                dataViewItem.SubItems.Add(keyValuePair.Value);

                databaseDataView.Items.Add(dataViewItem);
            }
        }

        public static MainWindow GetInstance() => s_Instance; 
    }
}