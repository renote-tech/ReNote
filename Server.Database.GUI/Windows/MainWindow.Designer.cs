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
            this.mountDatabaseTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unmountAllTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDatabaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mountDatabaseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DatabaseView = new System.Windows.Forms.TreeView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.openDatabaseDialog = new System.Windows.Forms.OpenFileDialog();
            this.DatabaseItemContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRootDocumentTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unmountTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.expandTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyRootDocumentNameTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseDataView = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnValue = new System.Windows.Forms.ColumnHeader();
            this.databaseDataContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addDocumentDataItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RootDocumentContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addDocumentTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTreeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.copyDocumentNameTreemItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseDataItemContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.copyNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDatabaseDialog = new System.Windows.Forms.SaveFileDialog();
            this.rootItemContextMenu.SuspendLayout();
            this.menuBar.SuspendLayout();
            this.DatabaseItemContextMenu.SuspendLayout();
            this.databaseDataContextMenu.SuspendLayout();
            this.RootDocumentContextMenu.SuspendLayout();
            this.databaseDataItemContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // rootItemContextMenu
            // 
            this.rootItemContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mountDatabaseTreeItem,
            this.unmountAllTreeItem});
            this.rootItemContextMenu.Name = "rootItemContextMenu";
            this.rootItemContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.rootItemContextMenu.Size = new System.Drawing.Size(161, 48);
            // 
            // mountDatabaseTreeItem
            // 
            this.mountDatabaseTreeItem.Name = "mountDatabaseTreeItem";
            this.mountDatabaseTreeItem.Size = new System.Drawing.Size(160, 22);
            this.mountDatabaseTreeItem.Text = "Mount database";
            this.mountDatabaseTreeItem.Click += new System.EventHandler(this.OnMountMenuItemClicked);
            // 
            // unmountAllTreeItem
            // 
            this.unmountAllTreeItem.Name = "unmountAllTreeItem";
            this.unmountAllTreeItem.Size = new System.Drawing.Size(160, 22);
            this.unmountAllTreeItem.Text = "Unmount all";
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuBar.Size = new System.Drawing.Size(800, 24);
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDatabaseMenuItem,
            this.mountDatabaseMenuItem,
            this.saveToolStripMenuItem1,
            this.saveAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
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
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(204, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.saveAllToolStripMenuItem.Text = "Save all";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(201, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitMenuItem.Size = new System.Drawing.Size(204, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.OnExitMenuItemClicked);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // DatabaseView
            // 
            this.DatabaseView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DatabaseView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DatabaseView.HideSelection = false;
            this.DatabaseView.Location = new System.Drawing.Point(0, 48);
            this.DatabaseView.Name = "DatabaseView";
            treeNode1.ContextMenuStrip = this.rootItemContextMenu;
            treeNode1.Name = "RootNode";
            treeNode1.Text = "Databases";
            this.DatabaseView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.DatabaseView.PathSeparator = "";
            this.DatabaseView.Size = new System.Drawing.Size(210, 401);
            this.DatabaseView.TabIndex = 1;
            this.DatabaseView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeViewItemClicked);
            this.DatabaseView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnTreeNodeItemClicked);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(0, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(800, 23);
            this.textBox1.TabIndex = 2;
            // 
            // openDatabaseDialog
            // 
            this.openDatabaseDialog.Filter = "Database Files|*.dat";
            this.openDatabaseDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OnSelectedOpenDBFile);
            // 
            // DatabaseItemContextMenu
            // 
            this.DatabaseItemContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveMenuItem,
            this.addRootDocumentTreeItem,
            this.unmountTreeItem,
            this.toolStripSeparator2,
            this.expandTreeItem,
            this.copyRootDocumentNameTreeItem});
            this.DatabaseItemContextMenu.Name = "databaseItemContextMenu";
            this.DatabaseItemContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.DatabaseItemContextMenu.Size = new System.Drawing.Size(150, 120);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Name = "saveMenuItem";
            this.saveMenuItem.Size = new System.Drawing.Size(149, 22);
            this.saveMenuItem.Text = "Save database";
            this.saveMenuItem.Click += new System.EventHandler(this.OnSaveMenuItemClicked);
            // 
            // addRootDocumentTreeItem
            // 
            this.addRootDocumentTreeItem.Name = "addRootDocumentTreeItem";
            this.addRootDocumentTreeItem.Size = new System.Drawing.Size(149, 22);
            this.addRootDocumentTreeItem.Text = "Add container";
            this.addRootDocumentTreeItem.Click += new System.EventHandler(this.OnAddRootDocumentMenuItemClicked);
            // 
            // unmountTreeItem
            // 
            this.unmountTreeItem.Name = "unmountTreeItem";
            this.unmountTreeItem.Size = new System.Drawing.Size(149, 22);
            this.unmountTreeItem.Text = "Unmount";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(146, 6);
            // 
            // expandTreeItem
            // 
            this.expandTreeItem.Name = "expandTreeItem";
            this.expandTreeItem.Size = new System.Drawing.Size(149, 22);
            this.expandTreeItem.Text = "Expand";
            // 
            // copyRootDocumentNameTreeItem
            // 
            this.copyRootDocumentNameTreeItem.Name = "copyRootDocumentNameTreeItem";
            this.copyRootDocumentNameTreeItem.Size = new System.Drawing.Size(149, 22);
            this.copyRootDocumentNameTreeItem.Text = "Copy name";
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
            this.databaseDataView.Location = new System.Drawing.Point(216, 48);
            this.databaseDataView.Name = "databaseDataView";
            this.databaseDataView.Size = new System.Drawing.Size(584, 401);
            this.databaseDataView.TabIndex = 3;
            this.databaseDataView.UseCompatibleStateImageBehavior = false;
            this.databaseDataView.View = System.Windows.Forms.View.Details;
            this.databaseDataView.SizeChanged += new System.EventHandler(this.OnDataViewSizeChanged);
            this.databaseDataView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnDataMouseDown);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 130;
            // 
            // columnValue
            // 
            this.columnValue.Text = "Value";
            this.columnValue.Width = 450;
            // 
            // databaseDataContextMenu
            // 
            this.databaseDataContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addDocumentDataItem});
            this.databaseDataContextMenu.Name = "databaseDataContextMenu";
            this.databaseDataContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.databaseDataContextMenu.Size = new System.Drawing.Size(124, 26);
            this.databaseDataContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.OnDataContextMenuOpening);
            // 
            // addDocumentDataItem
            // 
            this.addDocumentDataItem.Name = "addDocumentDataItem";
            this.addDocumentDataItem.Size = new System.Drawing.Size(123, 22);
            this.addDocumentDataItem.Text = "Add item";
            this.addDocumentDataItem.Click += new System.EventHandler(this.OnAddDocumentDataItemClicked);
            // 
            // RootDocumentContextMenu
            // 
            this.RootDocumentContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addDocumentTreeItem,
            this.renameTreeItem,
            this.deleteTreeItem,
            this.toolStripSeparator3,
            this.copyDocumentNameTreemItem});
            this.RootDocumentContextMenu.Name = "rootDocumentContextMenu";
            this.RootDocumentContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.RootDocumentContextMenu.Size = new System.Drawing.Size(136, 98);
            // 
            // addDocumentTreeItem
            // 
            this.addDocumentTreeItem.Name = "addDocumentTreeItem";
            this.addDocumentTreeItem.Size = new System.Drawing.Size(135, 22);
            this.addDocumentTreeItem.Text = "Add item";
            this.addDocumentTreeItem.Click += new System.EventHandler(this.OnAddDocumentMenuItemClicked);
            // 
            // renameTreeItem
            // 
            this.renameTreeItem.Name = "renameTreeItem";
            this.renameTreeItem.Size = new System.Drawing.Size(135, 22);
            this.renameTreeItem.Text = "Rename";
            // 
            // deleteTreeItem
            // 
            this.deleteTreeItem.Name = "deleteTreeItem";
            this.deleteTreeItem.Size = new System.Drawing.Size(135, 22);
            this.deleteTreeItem.Text = "Delete";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(132, 6);
            // 
            // copyDocumentNameTreemItem
            // 
            this.copyDocumentNameTreemItem.Name = "copyDocumentNameTreemItem";
            this.copyDocumentNameTreemItem.Size = new System.Drawing.Size(135, 22);
            this.copyDocumentNameTreemItem.Text = "Copy name";
            // 
            // databaseDataItemContextMenu
            // 
            this.databaseDataItemContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem1,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator4,
            this.copyNameToolStripMenuItem,
            this.copyValueToolStripMenuItem});
            this.databaseDataItemContextMenu.Name = "databaseDataItemContextMenu";
            this.databaseDataItemContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.databaseDataItemContextMenu.Size = new System.Drawing.Size(136, 98);
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.editToolStripMenuItem1.Text = "Edit";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(132, 6);
            // 
            // copyNameToolStripMenuItem
            // 
            this.copyNameToolStripMenuItem.Name = "copyNameToolStripMenuItem";
            this.copyNameToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.copyNameToolStripMenuItem.Text = "Copy name";
            // 
            // copyValueToolStripMenuItem
            // 
            this.copyValueToolStripMenuItem.Name = "copyValueToolStripMenuItem";
            this.copyValueToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.copyValueToolStripMenuItem.Text = "Copy value";
            // 
            // saveDatabaseDialog
            // 
            this.saveDatabaseDialog.Filter = "Database Files|*.dat";
            this.saveDatabaseDialog.Title = "Save...";
            this.saveDatabaseDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OnSelectedSaveDBFile);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.databaseDataView);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.DatabaseView);
            this.Controls.Add(this.menuBar);
            this.MainMenuStrip = this.menuBar;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReNote Database Editor";
            this.rootItemContextMenu.ResumeLayout(false);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.DatabaseItemContextMenu.ResumeLayout(false);
            this.databaseDataContextMenu.ResumeLayout(false);
            this.RootDocumentContextMenu.ResumeLayout(false);
            this.databaseDataItemContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public TreeView DatabaseView;
        public ContextMenuStrip DatabaseItemContextMenu;
        public ContextMenuStrip RootDocumentContextMenu;

        private MenuStrip menuBar;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem mountDatabaseMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitMenuItem;
        private TextBox textBox1;
        private ContextMenuStrip rootItemContextMenu;
        private ToolStripMenuItem mountDatabaseTreeItem;
        private OpenFileDialog openDatabaseDialog;
        private ToolStripMenuItem unmountTreeItem;
        private ToolStripMenuItem copyRootDocumentNameTreeItem;
        private ToolStripMenuItem expandTreeItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem newDatabaseMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ListView databaseDataView;
        private ColumnHeader columnName;
        private ColumnHeader columnValue;
        private ToolStripMenuItem renameTreeItem;
        private ToolStripMenuItem deleteTreeItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem copyDocumentNameTreemItem;
        private ToolStripMenuItem unmountAllTreeItem;
        private ToolStripMenuItem addRootDocumentTreeItem;
        private ToolStripMenuItem addDocumentTreeItem;
        private ContextMenuStrip databaseDataContextMenu;
        private ToolStripMenuItem addDocumentDataItem;
        private ContextMenuStrip databaseDataItemContextMenu;
        private ToolStripMenuItem editToolStripMenuItem1;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem copyNameToolStripMenuItem;
        private ToolStripMenuItem copyValueToolStripMenuItem;
        private ToolStripMenuItem saveMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem1;
        private ToolStripMenuItem saveAllToolStripMenuItem;
        private SaveFileDialog saveDatabaseDialog;
    }
}