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
        private string m_CurrentItemName;
        private string m_CurrentItemValue;

        private ListViewItem m_CopiedDataItem;
        private string m_CopiedDataItemName;
        private int m_CopiedDataItemNumber;

        private static MainWindow s_Instance;

        public bool IsDiscardDialogsEnabled = true;

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        public MainWindow()
        {
            InitializeComponent();

            s_Instance = this;

            DatabaseTreeView.ImageList = new ImageList();
            DatabaseTreeView.ImageList.Images.Add(new Bitmap("./assets/drive.png"));
            DatabaseTreeView.ImageList.Images.Add(new Bitmap("./assets/file.png"));
            DatabaseTreeView.ImageList.Images.Add(new Bitmap("./assets/text.png"));
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(DatabaseTreeView.Handle, "explorer", null);
        }

        private void OnSelectedOpenDBFile(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FileInfo fileInfo = new FileInfo(openDatabaseDialog.FileName);
            string fileName = fileInfo.Name.Replace(fileInfo.Extension, "");

            TreeNode databaseNode = new TreeNode(fileName)
            {
                ContextMenuStrip = DatabaseItemContextMenu,
                ImageIndex = 1,
                SelectedImageIndex = 1,
                Tag = fileName
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

            int index = DatabaseTreeView.Nodes[0].Nodes.Add(databaseNode);

            Container[] containers = database.GetContainers();
            for (int i = 0; i < containers.Length; i++)
            {
                TreeNode containerNode = new TreeNode(containers[i].Name)
                {
                    ContextMenuStrip = ContainerContextMenu,
                    ImageIndex = 2,
                    SelectedImageIndex = 2,
                    Tag = $"{fileName}/{containers[i].Name}"
                };

                DatabaseTreeView.Nodes[0].Nodes[index].Nodes.Add(containerNode);
            }

            DatabaseTreeView.Nodes[0].Expand();
        }

        private void OnSelectedSaveDBFile(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string dbName = GetCurrentDBName();
            if (dbName == null)
                return;

            DatabaseManager.SetSaveLocation(dbName, saveDatabaseDialog.FileName);
            SaveAsync();
        }

        private void OnNewDatabaseMenuItemClicked(object sender, EventArgs e)
        {
            new NewDatabaseDialog().ShowDialog();
        }

        private void OnMountMenuItemClicked(object sender, EventArgs e)
        {
            openDatabaseDialog.ShowDialog();
        }

        private void OnExitMenuItemClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnExpandAllMenuItemClicked(object sender, EventArgs e)
        {
            DatabaseTreeView.ExpandAll();
        }

        private void OnShowDiscardDialogsMenuItemClicked(object sender, EventArgs e)
        {
            IsDiscardDialogsEnabled = showDiscardDialogsMenuItem.Checked;
        }

        private void OnSaveTreeItemClicked(object sender, EventArgs e)
        {
            SaveAsync();
        }

        private void OnSaveAsTreeItemClicked(object sender, EventArgs e)
        {
            saveDatabaseDialog.ShowDialog();
        }

        private void OnAddContainerMenuItemClicked(object sender, EventArgs e)
        {
            string dbName = GetCurrentDBName();
            if (dbName == null)
                return;

            new NewContainerDialog(dbName).ShowDialog();
        }

        private void OnUnmountDatabaseTreeItemClicked(object sender, EventArgs e)
        {
            string dbName = GetCurrentDBName();
            if (dbName == null)
                return;

            bool needSave = DatabaseManager.NeedSave(dbName);
            bool shouldReturn = false;

            if (IsDiscardDialogsEnabled && needSave)
                shouldReturn = MessageBox.Show("Are you sure you want to unmount this database, all changes will be discard unless you save it.",
                                               "Confirmation",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Warning) == DialogResult.No;

            if (shouldReturn)
                return;

            DatabaseManager.RemoveDatabase(dbName);
            DatabaseTreeView.Nodes[0].Nodes.Remove(DatabaseTreeView.SelectedNode);
            RefreshDataView();
        }

        private void OnExpandDatabaseTreeItemClicked(object sender, EventArgs e)
        {
            TreeNode treeNode = DatabaseTreeView.SelectedNode;
            treeNode.Expand();
        }

        private void OnCopyNameClicked(object sender, EventArgs e)
        {
            TreeNode treeNode = DatabaseTreeView.SelectedNode;
            Clipboard.SetText(treeNode.Text);
        }

        private void OnAddDataItemClicked(object sender, EventArgs e)
        {
            new NewItemDialog(m_CurrentDatabase,
                              m_CurrentContainer).ShowDialog();
        }

        private void OnRenameContainerTreeItemClicked(object sender, EventArgs e)
        {
            TreeNode treeNode = DatabaseTreeView.SelectedNode;
            string[] path = GetDatabasePath(treeNode);
            if (path == null)
                return;

            new RenameContainerDialog(path[0], path[1]).ShowDialog();
        }

        private void OnDeleteContainerTreeItemClicked(object sender, EventArgs e)
        {
            TreeNode treeNode = DatabaseTreeView.SelectedNode;
            string[] path = GetDatabasePath(treeNode);
            if (path == null)
                return;

            if (!IsDiscardDialogsEnabled)
            {
                ContainerClean(path[0], path[1], treeNode);
                return;
            }

            DialogResult result = MessageBox.Show("Do you really want to delete this container?",
                                      "Confirmation",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            ContainerClean(path[0], path[1], treeNode);
        }

        private void OnAddDataMenuItemClicked(object sender, EventArgs e)
        {
            TreeNode treeNode = DatabaseTreeView.SelectedNode;
            string[] path = GetDatabasePath(treeNode);
            if (path == null)
                return;

            new NewItemDialog(path[0], path[1]).ShowDialog();
        }

        private void OnPasteDataMenuItemClicked(object sender, EventArgs e)
        {
            PasteDataItem();
        }

        private void OnCopyDataMenuItemClicked(object sender, EventArgs e)
        {
            CopyDataItem();
        }

        private void OnEditItemMenuItem(object sender, EventArgs e)
        {
            new EditItemDialog(m_CurrentDatabase,
                               m_CurrentContainer,
                               m_CurrentItemName,
                               m_CurrentItemValue).ShowDialog();
        }

        private void OnDeleteItemMenuItemClicked(object sender, EventArgs e)
        {
            if (!IsDiscardDialogsEnabled)
            {
                DatabaseManager.DeleteItem(m_CurrentDatabase, m_CurrentContainer, m_CurrentItemName);
                RefreshDataView();
                return;
            }

            DialogResult result = MessageBox.Show("Do you really want to delete this item?",
                                                  "Confirmation",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            ItemClean();
        }

        private void OnCopyItemNameMenuItemClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(m_CurrentItemName))
                return;

            Clipboard.SetText(m_CurrentItemName);
        }

        private void OnCopyItemValueMenuItemClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(m_CurrentItemValue))
                return;

            Clipboard.SetText(m_CurrentItemValue);
        }

        private void OnTreeNodeItemClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                DatabaseTreeView.SelectedNode = e.Node;
        }

        private void OnTreeViewItemClicked(object sender, TreeViewEventArgs e)
        {
            string[] path = GetDatabasePath(e.Node);
            if (path == null)
                return;

            Dictionary<string, string> data = DatabaseManager.GetItems(path[0], path[1]);
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

            m_CurrentDatabase = path[0];
            m_CurrentContainer = path[1];

            OnDataViewChanged();
        }

        private void OnDataKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                DeleteDataItem();
            else if (e.Control && e.KeyCode == Keys.C)
                CopyDataItem();
            else if (e.Control && e.KeyCode == Keys.V)
                PasteDataItem();
        }

        private void OnItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                m_CurrentItemName = e.Item.Text;
                m_CurrentItemValue = e.Item.SubItems[1].Text;
                databaseDataView.ContextMenuStrip = databaseDataItemContextMenu;
                return;
            }

            m_CurrentItemName = null;
            m_CurrentItemValue = null;
            databaseDataView.ContextMenuStrip = databaseDataContextMenu;
        }

        private void OnDataViewSizeChanged(object sender, EventArgs e)
        {
            // databaseDataView.Columns[1] represents the value column
            databaseDataView.Columns[1].Width = databaseDataView.Width - 135;
        }

        private void OnDataContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool canCreateItem = !(string.IsNullOrWhiteSpace(m_CurrentDatabase) || string.IsNullOrWhiteSpace(m_CurrentContainer));
            bool canPasteItem = (canCreateItem && m_CopiedDataItem != null);
            databaseDataContextMenu.Items[0].Enabled = canCreateItem;
            databaseDataContextMenu.Items[1].Enabled = canPasteItem;
        }

        private void OnDataViewChanged()
        {
            statusItemsCountLabel.Text = $"{databaseDataView.Items.Count} item{(databaseDataView.Items.Count == 1 ? "" : "s")}";
        }

        private void CopyDataItem()
        {
            if (databaseDataView.SelectedItems.Count == 0)
                return;

            m_CopiedDataItem = (ListViewItem)databaseDataView.SelectedItems[0].Clone();
            m_CopiedDataItemName = m_CopiedDataItem.Text;
            m_CopiedDataItemNumber = 0;
        }

        private void PasteDataItem()
        {
            if (m_CopiedDataItem == null)
                return;

            for (int i = 0; i < databaseDataView.Items.Count; i++)
            {
                if (databaseDataView.Items[i].Text == m_CopiedDataItem.Text)
                {
                    m_CopiedDataItem = (ListViewItem)m_CopiedDataItem.Clone();
                    break;
                }
            }

            m_CopiedDataItemNumber++;

            string itemName = $"{m_CopiedDataItemName}_{m_CopiedDataItemNumber}";
            m_CopiedDataItem.Text = itemName;

            DatabaseManager.AddItem(m_CurrentDatabase, m_CurrentContainer, itemName, m_CopiedDataItem.SubItems[1].Text);
            databaseDataView.Items.Add(m_CopiedDataItem);
        }

        private void ContainerClean(string dbName, string container, TreeNode treeNode)
        {
            m_CurrentDatabase = null;
            m_CurrentContainer = null;
            m_CurrentItemName = null;
            m_CurrentItemValue = null;

            DatabaseManager.DeleteContainer(dbName, container);
            DatabaseTreeView.Nodes[0].Nodes.Remove(treeNode);
            RefreshDataView();
        }

        private void ItemClean()
        {
            DatabaseManager.DeleteItem(m_CurrentDatabase, m_CurrentContainer, m_CurrentItemName);
            RefreshDataView();

            m_CurrentItemName = null;
            m_CurrentItemValue = null;
        }

        private void DeleteDataItem()
        {
            if (databaseDataView.SelectedItems.Count == 0)
                return;

            databaseDataItemContextMenu.Items[2].PerformClick();
        }

        private async void SaveAsync()
        {
            string dbName = GetCurrentDBName();
            if (string.IsNullOrWhiteSpace(dbName))
                return;

            int code = await DatabaseManager.SaveDatabaseAsync(dbName);
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

        private string GetCurrentDBName()
        {
            TreeNode treeNode = DatabaseTreeView.SelectedNode;
            if (treeNode.Tag == null)
                return null;

            return treeNode.Tag.ToString();
        }

        private string[] GetDatabasePath(TreeNode treeNode)
        {
            if (treeNode.Tag == null)
                return null;

            string dbPath = treeNode.Tag.ToString();
            if (!dbPath.Contains('/'))
                return null;

            string[] dbPathParts = dbPath.Split('/');
            if (dbPathParts.Length != 2)
                return null;

            return dbPathParts;
        }

        public void AddDatabaseNode(string databaseName)
        {
            TreeNode databaseNode = new TreeNode(databaseName)
            {
                ContextMenuStrip = DatabaseItemContextMenu,
                ImageIndex = 1,
                SelectedImageIndex = 1,
                Tag = databaseName
            };

            DatabaseTreeView.Nodes[0].Nodes.Add(databaseNode);
        }

        public int AddContainerNode(string dbName, string containerName)
        {
            TreeNode treeNode = null;
            int dbIndex = -1;
            for (int i = 0; i < DatabaseTreeView.Nodes[0].Nodes.Count; i++)
            {
                object tag = DatabaseTreeView.Nodes[0].Nodes[i].Tag;
                if (tag == null || tag.ToString() != dbName)
                    continue;

                treeNode = DatabaseTreeView.Nodes[0].Nodes[i];
                dbIndex = treeNode.Index;
                break;
            }

            if (treeNode == null)
                return dbIndex;

            TreeNode containerNode = new TreeNode(containerName)
            {
                ContextMenuStrip = ContainerContextMenu,
                ImageIndex = 2,
                SelectedImageIndex = 2,
                Tag = $"{dbName}/{containerName}"
            };

            treeNode.Nodes.Add(containerNode);
            return dbIndex;
        }

        public void RenameContainerNode(string dbName, string containerName, string newContainerName)
        {
            TreeNode dbNode = null;
            for (int i = 0; i < DatabaseTreeView.Nodes[0].Nodes.Count; i++)
            {
                object tag = DatabaseTreeView.Nodes[0].Nodes[i].Tag;
                if (tag == null || tag.ToString() != dbName)
                    continue;

                dbNode = DatabaseTreeView.Nodes[0].Nodes[i];
                break;
            }

            for (int i = 0; i < dbNode.Nodes.Count; i++)
            {
                object tag = dbNode.Nodes[i].Tag;
                if (tag == null)
                    continue;

                string path = tag.ToString();
                if (!path.Contains('/'))
                    continue;

                string[] parts = path.Split('/');
                if (parts[0] != dbName || parts[1] != containerName)
                    continue;

                dbNode.Nodes[i].Tag = $"{dbName}/{newContainerName}";
                dbNode.Nodes[i].Text = newContainerName;
            }
        }

        public void RefreshDataView()
        {
            databaseDataView.Items.Clear();
            statusItemsCountLabel.Text = string.Empty;
            databaseDataView.ContextMenuStrip = databaseDataContextMenu;

            if (string.IsNullOrWhiteSpace(m_CurrentDatabase) || string.IsNullOrWhiteSpace(m_CurrentContainer))
                return;

            Dictionary<string, string> data = DatabaseManager.GetItems(m_CurrentDatabase, m_CurrentContainer);
            if (data == null)
                return;

            for (int i = 0; i < data.Count; i++)
            {
                KeyValuePair<string, string> keyValuePair = data.ElementAt(i);
                ListViewItem dataViewItem = new ListViewItem(keyValuePair.Key);
                dataViewItem.SubItems.Add(keyValuePair.Value);

                databaseDataView.Items.Add(dataViewItem);
            }

            OnDataViewChanged();
        }

        public static MainWindow GetInstance() => s_Instance;
    }
}