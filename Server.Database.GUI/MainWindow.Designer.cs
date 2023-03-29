using System.Windows.Forms;

namespace Server.Database.Windows
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Databases");
            this.rootItemContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newDatabaseTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mountDatabaseTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDatabaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mountDatabaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSeperator = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDiscardDialogsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DatabaseTreeView = new System.Windows.Forms.TreeView();
            this.openDatabaseDialog = new System.Windows.Forms.OpenFileDialog();
            this.DatabaseItemContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addContainerTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unmountDatabaseTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseItemSeperator = new System.Windows.Forms.ToolStripSeparator();
            this.expandDatabaseTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyDatabaseNameTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseDataView = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnValue = new System.Windows.Forms.ColumnHeader();
            this.databaseDataContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContainerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addItemTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameContainerTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteContainerTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.containerSeperator = new System.Windows.Forms.ToolStripSeparator();
            this.copyContainerNameTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseDataItemContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemSeperator = new System.Windows.Forms.ToolStripSeparator();
            this.copyDataNameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyDataValueMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDatabaseDialog = new System.Windows.Forms.SaveFileDialog();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusItemsCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.topPanel = new System.Windows.Forms.Panel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.rootItemContextMenu.SuspendLayout();
            this.menuBar.SuspendLayout();
            this.DatabaseItemContextMenu.SuspendLayout();
            this.databaseDataContextMenu.SuspendLayout();
            this.ContainerContextMenu.SuspendLayout();
            this.databaseDataItemContextMenu.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // rootItemContextMenu
            // 
            this.rootItemContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDatabaseTreeItem,
            this.mountDatabaseTreeItem});
            this.rootItemContextMenu.Name = "rootItemContextMenu";
            this.rootItemContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.rootItemContextMenu.Size = new System.Drawing.Size(161, 48);
            // 
            // newDatabaseTreeItem
            // 
            this.newDatabaseTreeItem.Name = "newDatabaseTreeItem";
            this.newDatabaseTreeItem.Size = new System.Drawing.Size(160, 22);
            this.newDatabaseTreeItem.Text = "New database";
            this.newDatabaseTreeItem.Click += new System.EventHandler(this.OnNewDatabaseMenuItemClicked);
            // 
            // mountDatabaseTreeItem
            // 
            this.mountDatabaseTreeItem.Name = "mountDatabaseTreeItem";
            this.mountDatabaseTreeItem.Size = new System.Drawing.Size(160, 22);
            this.mountDatabaseTreeItem.Text = "Mount database";
            this.mountDatabaseTreeItem.Click += new System.EventHandler(this.OnMountMenuItemClicked);
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.viewMenuItem,
            this.optionsMenuItem});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuBar.Size = new System.Drawing.Size(944, 24);
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDatabaseMenuItem,
            this.mountDatabaseMenuItem,
            this.fileSeperator,
            this.exitMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileMenuItem.Text = "File";
            // 
            // newDatabaseMenuItem
            // 
            this.newDatabaseMenuItem.Name = "newDatabaseMenuItem";
            this.newDatabaseMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newDatabaseMenuItem.Size = new System.Drawing.Size(204, 22);
            this.newDatabaseMenuItem.Text = "New Database";
            this.newDatabaseMenuItem.Click += new System.EventHandler(this.OnNewDatabaseMenuItemClicked);
            // 
            // mountDatabaseMenuItem
            // 
            this.mountDatabaseMenuItem.Name = "mountDatabaseMenuItem";
            this.mountDatabaseMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mountDatabaseMenuItem.Size = new System.Drawing.Size(204, 22);
            this.mountDatabaseMenuItem.Text = "Mount Database";
            this.mountDatabaseMenuItem.Click += new System.EventHandler(this.OnMountMenuItemClicked);
            // 
            // fileSeperator
            // 
            this.fileSeperator.Name = "fileSeperator";
            this.fileSeperator.Size = new System.Drawing.Size(201, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitMenuItem.Size = new System.Drawing.Size(204, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.OnExitMenuItemClicked);
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandAllMenuItem});
            this.viewMenuItem.Name = "viewMenuItem";
            this.viewMenuItem.Size = new System.Drawing.Size(39, 20);
            this.viewMenuItem.Text = "Edit";
            // 
            // expandAllMenuItem
            // 
            this.expandAllMenuItem.Name = "expandAllMenuItem";
            this.expandAllMenuItem.Size = new System.Drawing.Size(128, 22);
            this.expandAllMenuItem.Text = "Expand all";
            this.expandAllMenuItem.Click += new System.EventHandler(this.OnExpandAllMenuItemClicked);
            // 
            // optionsMenuItem
            // 
            this.optionsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDiscardDialogsMenuItem});
            this.optionsMenuItem.Name = "optionsMenuItem";
            this.optionsMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsMenuItem.Text = "Options";
            // 
            // showDiscardDialogsMenuItem
            // 
            this.showDiscardDialogsMenuItem.Checked = true;
            this.showDiscardDialogsMenuItem.CheckOnClick = true;
            this.showDiscardDialogsMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showDiscardDialogsMenuItem.Name = "showDiscardDialogsMenuItem";
            this.showDiscardDialogsMenuItem.Size = new System.Drawing.Size(185, 22);
            this.showDiscardDialogsMenuItem.Text = "Show discard dialogs";
            this.showDiscardDialogsMenuItem.Click += new System.EventHandler(this.OnShowDiscardDialogsMenuItemClicked);
            // 
            // DatabaseTreeView
            // 
            this.DatabaseTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DatabaseTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DatabaseTreeView.HideSelection = false;
            this.DatabaseTreeView.Location = new System.Drawing.Point(0, 27);
            this.DatabaseTreeView.Name = "DatabaseTreeView";
            treeNode1.ContextMenuStrip = this.rootItemContextMenu;
            treeNode1.Name = "RootNode";
            treeNode1.Text = "Databases";
            this.DatabaseTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.DatabaseTreeView.PathSeparator = "";
            this.DatabaseTreeView.Size = new System.Drawing.Size(215, 454);
            this.DatabaseTreeView.TabIndex = 1;
            this.DatabaseTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeViewItemClicked);
            this.DatabaseTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnTreeNodeItemClicked);
            // 
            // openDatabaseDialog
            // 
            this.openDatabaseDialog.Filter = "Database Files|*.dat";
            this.openDatabaseDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OnSelectedOpenDBFile);
            // 
            // DatabaseItemContextMenu
            // 
            this.DatabaseItemContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveTreeItem,
            this.saveAsTreeItem,
            this.addContainerTreeItem,
            this.unmountDatabaseTreeItem,
            this.databaseItemSeperator,
            this.expandDatabaseTreeItem,
            this.copyDatabaseNameTreeItem});
            this.DatabaseItemContextMenu.Name = "databaseItemContextMenu";
            this.DatabaseItemContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.DatabaseItemContextMenu.Size = new System.Drawing.Size(150, 142);
            // 
            // saveTreeItem
            // 
            this.saveTreeItem.Name = "saveTreeItem";
            this.saveTreeItem.Size = new System.Drawing.Size(149, 22);
            this.saveTreeItem.Text = "Save";
            this.saveTreeItem.Click += new System.EventHandler(this.OnSaveTreeItemClicked);
            // 
            // saveAsTreeItem
            // 
            this.saveAsTreeItem.Name = "saveAsTreeItem";
            this.saveAsTreeItem.Size = new System.Drawing.Size(149, 22);
            this.saveAsTreeItem.Text = "Save as";
            this.saveAsTreeItem.Click += new System.EventHandler(this.OnSaveAsTreeItemClicked);
            // 
            // addContainerTreeItem
            // 
            this.addContainerTreeItem.Name = "addContainerTreeItem";
            this.addContainerTreeItem.Size = new System.Drawing.Size(149, 22);
            this.addContainerTreeItem.Text = "Add container";
            this.addContainerTreeItem.Click += new System.EventHandler(this.OnAddContainerMenuItemClicked);
            // 
            // unmountDatabaseTreeItem
            // 
            this.unmountDatabaseTreeItem.Name = "unmountDatabaseTreeItem";
            this.unmountDatabaseTreeItem.Size = new System.Drawing.Size(149, 22);
            this.unmountDatabaseTreeItem.Text = "Unmount";
            this.unmountDatabaseTreeItem.Click += new System.EventHandler(this.OnUnmountDatabaseTreeItemClicked);
            // 
            // databaseItemSeperator
            // 
            this.databaseItemSeperator.Name = "databaseItemSeperator";
            this.databaseItemSeperator.Size = new System.Drawing.Size(146, 6);
            // 
            // expandDatabaseTreeItem
            // 
            this.expandDatabaseTreeItem.Name = "expandDatabaseTreeItem";
            this.expandDatabaseTreeItem.Size = new System.Drawing.Size(149, 22);
            this.expandDatabaseTreeItem.Text = "Expand";
            this.expandDatabaseTreeItem.Click += new System.EventHandler(this.OnExpandDatabaseTreeItemClicked);
            // 
            // copyDatabaseNameTreeItem
            // 
            this.copyDatabaseNameTreeItem.Name = "copyDatabaseNameTreeItem";
            this.copyDatabaseNameTreeItem.Size = new System.Drawing.Size(149, 22);
            this.copyDatabaseNameTreeItem.Text = "Copy name";
            this.copyDatabaseNameTreeItem.Click += new System.EventHandler(this.OnCopyNameClicked);
            // 
            // databaseDataView
            // 
            this.databaseDataView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.databaseDataView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.databaseDataView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnValue});
            this.databaseDataView.FullRowSelect = true;
            this.databaseDataView.Location = new System.Drawing.Point(216, 27);
            this.databaseDataView.MultiSelect = false;
            this.databaseDataView.Name = "databaseDataView";
            this.databaseDataView.Size = new System.Drawing.Size(728, 454);
            this.databaseDataView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.databaseDataView.TabIndex = 3;
            this.databaseDataView.UseCompatibleStateImageBehavior = false;
            this.databaseDataView.View = System.Windows.Forms.View.Details;
            this.databaseDataView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.OnItemSelectionChanged);
            this.databaseDataView.SizeChanged += new System.EventHandler(this.OnDataViewSizeChanged);
            this.databaseDataView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnDataKeyDown);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 130;
            // 
            // columnValue
            // 
            this.columnValue.Text = "Value";
            this.columnValue.Width = 595;
            // 
            // databaseDataContextMenu
            // 
            this.databaseDataContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addDataMenuItem,
            this.pasteDataMenuItem});
            this.databaseDataContextMenu.Name = "databaseDataContextMenu";
            this.databaseDataContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.databaseDataContextMenu.Size = new System.Drawing.Size(124, 48);
            this.databaseDataContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.OnDataContextMenuOpening);
            // 
            // addDataMenuItem
            // 
            this.addDataMenuItem.Name = "addDataMenuItem";
            this.addDataMenuItem.Size = new System.Drawing.Size(123, 22);
            this.addDataMenuItem.Text = "Add item";
            this.addDataMenuItem.Click += new System.EventHandler(this.OnAddDataItemClicked);
            // 
            // pasteDataMenuItem
            // 
            this.pasteDataMenuItem.Enabled = false;
            this.pasteDataMenuItem.Name = "pasteDataMenuItem";
            this.pasteDataMenuItem.Size = new System.Drawing.Size(123, 22);
            this.pasteDataMenuItem.Text = "Paste";
            this.pasteDataMenuItem.Click += new System.EventHandler(this.OnPasteDataMenuItemClicked);
            // 
            // ContainerContextMenu
            // 
            this.ContainerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addItemTreeItem,
            this.renameContainerTreeItem,
            this.deleteContainerTreeItem,
            this.containerSeperator,
            this.copyContainerNameTreeItem});
            this.ContainerContextMenu.Name = "rootDocumentContextMenu";
            this.ContainerContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ContainerContextMenu.Size = new System.Drawing.Size(136, 98);
            // 
            // addItemTreeItem
            // 
            this.addItemTreeItem.Name = "addItemTreeItem";
            this.addItemTreeItem.Size = new System.Drawing.Size(135, 22);
            this.addItemTreeItem.Text = "Add item";
            this.addItemTreeItem.Click += new System.EventHandler(this.OnAddDataMenuItemClicked);
            // 
            // renameContainerTreeItem
            // 
            this.renameContainerTreeItem.Name = "renameContainerTreeItem";
            this.renameContainerTreeItem.Size = new System.Drawing.Size(135, 22);
            this.renameContainerTreeItem.Text = "Rename";
            this.renameContainerTreeItem.Click += new System.EventHandler(this.OnRenameContainerTreeItemClicked);
            // 
            // deleteContainerTreeItem
            // 
            this.deleteContainerTreeItem.Name = "deleteContainerTreeItem";
            this.deleteContainerTreeItem.Size = new System.Drawing.Size(135, 22);
            this.deleteContainerTreeItem.Text = "Delete";
            this.deleteContainerTreeItem.Click += new System.EventHandler(this.OnDeleteContainerTreeItemClicked);
            // 
            // containerSeperator
            // 
            this.containerSeperator.Name = "containerSeperator";
            this.containerSeperator.Size = new System.Drawing.Size(132, 6);
            // 
            // copyContainerNameTreeItem
            // 
            this.copyContainerNameTreeItem.Name = "copyContainerNameTreeItem";
            this.copyContainerNameTreeItem.Size = new System.Drawing.Size(135, 22);
            this.copyContainerNameTreeItem.Text = "Copy name";
            this.copyContainerNameTreeItem.Click += new System.EventHandler(this.OnCopyNameClicked);
            // 
            // databaseDataItemContextMenu
            // 
            this.databaseDataItemContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyDataMenuItem,
            this.editDataMenuItem,
            this.deleteDataMenuItem,
            this.itemSeperator,
            this.copyDataNameMenuItem,
            this.copyDataValueMenuItem});
            this.databaseDataItemContextMenu.Name = "databaseDataItemContextMenu";
            this.databaseDataItemContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.databaseDataItemContextMenu.Size = new System.Drawing.Size(136, 120);
            // 
            // copyDataMenuItem
            // 
            this.copyDataMenuItem.Name = "copyDataMenuItem";
            this.copyDataMenuItem.Size = new System.Drawing.Size(135, 22);
            this.copyDataMenuItem.Text = "Copy";
            this.copyDataMenuItem.Click += new System.EventHandler(this.OnCopyDataMenuItemClicked);
            // 
            // editDataMenuItem
            // 
            this.editDataMenuItem.Name = "editDataMenuItem";
            this.editDataMenuItem.Size = new System.Drawing.Size(135, 22);
            this.editDataMenuItem.Text = "Edit";
            this.editDataMenuItem.Click += new System.EventHandler(this.OnEditItemMenuItem);
            // 
            // deleteDataMenuItem
            // 
            this.deleteDataMenuItem.Name = "deleteDataMenuItem";
            this.deleteDataMenuItem.Size = new System.Drawing.Size(135, 22);
            this.deleteDataMenuItem.Text = "Delete";
            this.deleteDataMenuItem.Click += new System.EventHandler(this.OnDeleteItemMenuItemClicked);
            // 
            // itemSeperator
            // 
            this.itemSeperator.Name = "itemSeperator";
            this.itemSeperator.Size = new System.Drawing.Size(132, 6);
            // 
            // copyDataNameMenuItem
            // 
            this.copyDataNameMenuItem.Name = "copyDataNameMenuItem";
            this.copyDataNameMenuItem.Size = new System.Drawing.Size(135, 22);
            this.copyDataNameMenuItem.Text = "Copy name";
            this.copyDataNameMenuItem.Click += new System.EventHandler(this.OnCopyItemNameMenuItemClicked);
            // 
            // copyDataValueMenuItem
            // 
            this.copyDataValueMenuItem.Name = "copyDataValueMenuItem";
            this.copyDataValueMenuItem.Size = new System.Drawing.Size(135, 22);
            this.copyDataValueMenuItem.Text = "Copy value";
            this.copyDataValueMenuItem.Click += new System.EventHandler(this.OnCopyItemValueMenuItemClicked);
            // 
            // saveDatabaseDialog
            // 
            this.saveDatabaseDialog.Filter = "Database Files|*.dat";
            this.saveDatabaseDialog.Title = "Save...";
            this.saveDatabaseDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OnSelectedSaveDBFile);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusItemsCountLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 479);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(944, 22);
            this.statusBar.TabIndex = 5;
            this.statusBar.Text = "statusStrip1";
            // 
            // statusItemsCountLabel
            // 
            this.statusItemsCountLabel.BackColor = System.Drawing.SystemColors.Control;
            this.statusItemsCountLabel.Margin = new System.Windows.Forms.Padding(0);
            this.statusItemsCountLabel.Name = "statusItemsCountLabel";
            this.statusItemsCountLabel.Size = new System.Drawing.Size(0, 0);
            // 
            // topPanel
            // 
            this.topPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topPanel.BackColor = System.Drawing.Color.White;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(944, 27);
            this.topPanel.TabIndex = 6;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomPanel.BackColor = System.Drawing.Color.White;
            this.bottomPanel.Location = new System.Drawing.Point(0, 474);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(944, 27);
            this.bottomPanel.TabIndex = 7;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(944, 501);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.databaseDataView);
            this.Controls.Add(this.DatabaseTreeView);
            this.Controls.Add(this.menuBar);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.bottomPanel);
            this.MainMenuStrip = this.menuBar;
            this.MinimumSize = new System.Drawing.Size(960, 540);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReNote Database Editor";
            this.rootItemContextMenu.ResumeLayout(false);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.DatabaseItemContextMenu.ResumeLayout(false);
            this.databaseDataContextMenu.ResumeLayout(false);
            this.ContainerContextMenu.ResumeLayout(false);
            this.databaseDataItemContextMenu.ResumeLayout(false);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public TreeView DatabaseTreeView;
        public ContextMenuStrip DatabaseItemContextMenu;
        public ContextMenuStrip ContainerContextMenu;

        private MenuStrip menuBar;
        private ToolStripMenuItem fileMenuItem;
        private ToolStripMenuItem mountDatabaseMenuItem;
        private ToolStripMenuItem exitMenuItem;
        private ContextMenuStrip rootItemContextMenu;
        private ToolStripMenuItem mountDatabaseTreeItem;
        private OpenFileDialog openDatabaseDialog;
        private ToolStripMenuItem unmountDatabaseTreeItem;
        private ToolStripMenuItem copyDatabaseNameTreeItem;
        private ToolStripMenuItem expandDatabaseTreeItem;
        private ToolStripSeparator databaseItemSeperator;
        private ToolStripMenuItem newDatabaseMenuItem;
        private ToolStripMenuItem viewMenuItem;
        private ListView databaseDataView;
        private ColumnHeader columnName;
        private ColumnHeader columnValue;
        private ToolStripMenuItem renameContainerTreeItem;
        private ToolStripMenuItem deleteContainerTreeItem;
        private ToolStripSeparator containerSeperator;
        private ToolStripMenuItem copyContainerNameTreeItem;
        private ToolStripMenuItem addContainerTreeItem;
        private ToolStripMenuItem addItemTreeItem;
        private ContextMenuStrip databaseDataContextMenu;
        private ToolStripMenuItem addDataMenuItem;
        private ContextMenuStrip databaseDataItemContextMenu;
        private ToolStripMenuItem editDataMenuItem;
        private ToolStripMenuItem deleteDataMenuItem;
        private ToolStripSeparator itemSeperator;
        private ToolStripMenuItem copyDataNameMenuItem;
        private ToolStripMenuItem copyDataValueMenuItem;
        private ToolStripMenuItem saveTreeItem;
        private SaveFileDialog saveDatabaseDialog;
        private ToolStripSeparator fileSeperator;
        private ToolStripMenuItem newDatabaseTreeItem;
        private ToolStripMenuItem expandAllMenuItem;
        private StatusStrip statusBar;
        private ToolStripStatusLabel statusItemsCountLabel;
        private ToolStripMenuItem optionsMenuItem;
        private ToolStripMenuItem showDiscardDialogsMenuItem;
        private Panel topPanel;
        private Panel bottomPanel;
        private ToolStripMenuItem copyDataMenuItem;
        private ToolStripMenuItem pasteDataMenuItem;
        private ToolStripMenuItem saveAsTreeItem;
    }
}