using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.Database.Management;
using Server.ReNote.Data;

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

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            s_Instance = this;

            DatabaseTreeView.ImageList = new ImageList();
            DatabaseTreeView.ImageList.Images.Add(new Bitmap("./assets/drive.png"));
            DatabaseTreeView.ImageList.Images.Add(new Bitmap("./assets/file.png"));
            DatabaseTreeView.ImageList.Images.Add(new Bitmap("./assets/text.png"));
        }

        /// <summary>
        /// Modifies the style of the <see cref="DatabaseTreeView"/>.
        /// </summary>
        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(DatabaseTreeView.Handle, "explorer", null);
        }

        /// <summary>
        /// Occurs when the user clicked OK in the open dialog.
        /// Opens a database file.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
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

        /// <summary>
        /// Occurs when the user clicked OK in the save dialog.
        /// Saves a database.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnSelectedSaveDBFile(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string dbName = GetCurrentDBName();
            if (dbName == null)
                return;

            DatabaseManager.SetSaveLocation(dbName, saveDatabaseDialog.FileName);
            await SaveAsync();
        }

        /// <summary>
        /// Occurs when the new database item is clicked.
        /// Creates a new database.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnNewDatabaseMenuItemClicked(object sender, EventArgs e)
        {
            new NewDatabaseDialog().ShowDialog();
        }

        /// <summary>
        /// Occurs when the mount database item is clicked.
        /// Mounts a database.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnMountDatabaseMenuItemClicked(object sender, EventArgs e)
        {
            openDatabaseDialog.ShowDialog();
        }

        /// <summary>
        /// Occurs when the exit item is clicked.
        /// Closes the application.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnExitMenuItemClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Occurs when the expand all item is clicked.
        /// Expands the <see cref="DatabaseTreeView"/> completely.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnExpandAllMenuItemClicked(object sender, EventArgs e)
        {
            DatabaseTreeView.ExpandAll();
        }

        /// <summary>
        /// Occurs when the show discard dialogs item is clicked.
        /// Enables or disables discard dialogs when deleting or unmounting.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnShowDiscardDialogsMenuItemClicked(object sender, EventArgs e)
        {
            IsDiscardDialogsEnabled = showDiscardDialogsMenuItem.Checked;
        }

        /// <summary>
        /// Occurs when the save item is clicked.
        /// Saves a database.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void OnSaveTreeItemClicked(object sender, EventArgs e)
        {
            await SaveAsync();
        }

        /// <summary>
        /// Occurs when the save as item is clicked.
        /// Saves a database as.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveAsTreeItemClicked(object sender, EventArgs e)
        {
            saveDatabaseDialog.ShowDialog();
        }

        /// <summary>
        /// Occurs when the add container item is clicked.
        /// Adds a container to a specified database.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnAddContainerMenuItemClicked(object sender, EventArgs e)
        {
            string dbName = GetCurrentDBName();
            if (dbName == null)
                return;

            new NewContainerDialog(dbName).ShowDialog();
        }

        /// <summary>
        /// Occurs when the unmount item is clicked.
        /// Unmounts a database.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
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

        /// <summary>
        /// Occurs when the expand item is clicked.
        /// Expand a database tree node.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnExpandDatabaseTreeItemClicked(object sender, EventArgs e)
        {
            TreeNode treeNode = DatabaseTreeView.SelectedNode;
            treeNode.Expand();
        }

        /// <summary>
        /// Occurs when the copy name item is clicked.
        /// Copies the name of a database or container to the clipboard.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCopyNameClicked(object sender, EventArgs e)
        {
            TreeNode treeNode = DatabaseTreeView.SelectedNode;
            Clipboard.SetText(treeNode.Text);
        }

        /// <summary>
        /// Occurs when the add data item is clicked.
        /// Adds an item to a specified container.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnAddDataTreeItemClicked(object sender, EventArgs e)
        {
            new NewItemDialog(m_CurrentDatabase,
                              m_CurrentContainer).ShowDialog();
        }

        /// <summary>
        /// Occurs when the rename container item is clicked.
        /// Renames a container.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnRenameContainerTreeItemClicked(object sender, EventArgs e)
        {
            TreeNode treeNode = DatabaseTreeView.SelectedNode;
            string[] path = GetDatabasePath(treeNode);
            if (path == null)
                return;

            new RenameContainerDialog(path[0], path[1]).ShowDialog();
        }

        /// <summary>
        /// Occurs when the delete container item is clicked.
        /// Deletes a container.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
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

        /// <summary>
        /// Occurs when the add data item is clicked.
        /// Adds an item to a specified container.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnAddDataMenuItemClicked(object sender, EventArgs e)
        {
            TreeNode treeNode = DatabaseTreeView.SelectedNode;
            string[] path = GetDatabasePath(treeNode);
            if (path == null)
                return;

            new NewItemDialog(path[0], path[1]).ShowDialog();
        }

        /// <summary>
        /// Occurs when the paste item is clicked.
        /// Pastes an item to a specified container.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnPasteDataMenuItemClicked(object sender, EventArgs e)
        {
            PasteDataItem();
        }

        /// <summary>
        /// Occurs when the copy item is clicked.
        /// Copies an item from a specified container.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCopyDataMenuItemClicked(object sender, EventArgs e)
        {
            CopyDataItem();
        }

        /// <summary>
        /// Occurs when the edit item is clicked.
        /// Edits an item from a specified container.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnEditItemMenuItem(object sender, EventArgs e)
        {
            new EditItemDialog(m_CurrentDatabase,
                               m_CurrentContainer,
                               m_CurrentItemName,
                               m_CurrentItemValue).ShowDialog();
        }

        /// <summary>
        /// Occurs when the delete item is clicked.
        /// Deletes an item from a specified item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
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

        /// <summary>
        /// Occurs when the copy name item is clicked.
        /// Copies the name of a specified item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCopyItemNameMenuItemClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(m_CurrentItemName))
                return;

            Clipboard.SetText(m_CurrentItemName);
        }

        /// <summary>
        /// Occurs when the copy value item is clicked.
        /// Copies the value of a specified item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCopyItemValueMenuItemClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(m_CurrentItemValue))
                return;

            Clipboard.SetText(m_CurrentItemValue);
        }

        /// <summary>
        /// Occurs when an item is selected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnTreeNodeItemClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                DatabaseTreeView.SelectedNode = e.Node;
        }

        /// <summary>
        /// Occurs after the selection of an item in the <see cref="DatabaseTreeView"/>.
        /// Shows the data of a container.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeNodeItemAfterSelected(object sender, TreeViewEventArgs e)
        {
            string[] path = GetDatabasePath(e.Node);
            if (path == null)
                return;

            m_CurrentDatabase = path[0];
            m_CurrentContainer = path[1];

            RefreshDataView();
        }

        /// <summary>
        /// Occurs when a key is down when the <see cref="databaseDataView"/> is focused.
        /// Executes a specific action based on the key.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDatabaseDataViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                DeleteDataItem();
            else if (e.Control && e.KeyCode == Keys.C)
                CopyDataItem();
            else if (e.Control && e.KeyCode == Keys.V)
                PasteDataItem();
        }

        /// <summary>
        /// Occurs when the item selection has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
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

        /// <summary>
        /// Occurs when the <see cref="databaseDataView"/> size has changed.
        /// Resize the second column of the <see cref="databaseDataView"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDataViewSizeChanged(object sender, EventArgs e)
        {
            // databaseDataView.Columns[1] represents the value column
            databaseDataView.Columns[1].Width = databaseDataView.Width - 135;
        }

        /// <summary>
        /// Occurs when the item context menu is opening.
        /// Enables or disables the items of the <see cref="databaseDataContextMenu"/> based on the current state.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnDataContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool canCreateItem = !(string.IsNullOrWhiteSpace(m_CurrentDatabase) || string.IsNullOrWhiteSpace(m_CurrentContainer));
            bool canPasteItem = (canCreateItem && m_CopiedDataItem != null);
            databaseDataContextMenu.Items[0].Enabled = canCreateItem;
            databaseDataContextMenu.Items[1].Enabled = canPasteItem;
        }

        /// <summary>
        /// Updates the item count when the <see cref="databaseDataView"/>'s data is refreshed.
        /// </summary>
        private void OnDataViewChanged()
        {
            statusItemsCountLabel.Text = $"{databaseDataView.Items.Count} item{(databaseDataView.Items.Count == 1 ? "" : "s")}";
        }

        /// <summary>
        /// Copies an item.
        /// </summary>
        private void CopyDataItem()
        {
            if (databaseDataView.SelectedItems.Count == 0)
                return;

            m_CopiedDataItem = (ListViewItem)databaseDataView.SelectedItems[0].Clone();
            m_CopiedDataItemName = m_CopiedDataItem.Text;
            m_CopiedDataItemNumber = 0;
        }

        /// <summary>
        /// Pastes a copied item.
        /// </summary>
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

        /// <summary>
        /// Deletes a container and clean up.
        /// </summary>
        /// <param name="dbName">The database name.</param>
        /// <param name="containerName">The container name.</param>
        /// <param name="treeNode">The tree node.</param>
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

        /// <summary>
        /// Deletes an item and clean up.
        /// </summary>
        private void ItemClean()
        {
            DatabaseManager.DeleteItem(m_CurrentDatabase, m_CurrentContainer, m_CurrentItemName);
            RefreshDataView();

            m_CurrentItemName = null;
            m_CurrentItemValue = null;
        }

        /// <summary>
        /// Performs an item deletion.
        /// </summary>
        private void DeleteDataItem()
        {
            if (databaseDataView.SelectedItems.Count == 0)
                return;

            databaseDataItemContextMenu.Items[2].PerformClick();
        }

        /// <summary>
        /// Saves asynchronously the database.
        /// </summary>
        private async Task SaveAsync()
        {
            string dbName = GetCurrentDBName();
            if (string.IsNullOrWhiteSpace(dbName))
                return;

            int code = await DatabaseManager.SaveDatabaseAsync(dbName);
            if (code == 1)
                saveDatabaseDialog.ShowDialog();
        }

        /// <summary>
        /// Gets the database's name based on the selected node.
        /// </summary>
        /// <returns><see cref="string"/></returns>
        private string GetCurrentDBName()
        {
            TreeNode treeNode = DatabaseTreeView.SelectedNode;
            if (treeNode.Tag == null)
                return null;

            return treeNode.Tag.ToString();
        }

        /// <summary>
        /// Adds a database node.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
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

        /// <summary>
        /// Adds a container node in the <see cref="databaseTreeView"/>.
        /// Returns the index of the container node.
        /// </summary>
        /// <param name="dbName">The database name.</param>
        /// <param name="containerName">The container name.</param>
        /// <returns><see cref="int"/></returns>
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

        /// <summary>
        /// Renames a container node in the <see cref="DatabaseTreeView"/>.
        /// </summary>
        /// <param name="dbName">The database name.</param>
        /// <param name="containerName">The container name.</param>
        /// <param name="newContainerName">The new container name.</param>
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

        /// <summary>
        /// Refreshes the data within the <see cref="databaseDataView"/>.
        /// </summary>
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

        /// <summary>
        /// Gets the database's path (db/container) based on the specified node.
        /// </summary>
        /// <param name="treeNode">The tree node.</param>
        /// <returns><see cref="string"/>[]</returns>
        private static string[] GetDatabasePath(TreeNode treeNode)
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

        /// <summary>
        /// Main Window Instance.
        /// </summary>
        /// <returns><see cref="MainWindow"/></returns>
        public static MainWindow GetInstance() => s_Instance;
    }
}