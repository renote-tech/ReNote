﻿using Server.Database.Management;
using System;
using System.Windows.Forms;

namespace Server.Database.Windows
{
    public partial class NewItemDialog : Form
    {
        private string m_Database;
        private string m_Container;

        public NewItemDialog(string database, string container)
        {
            InitializeComponent();

            m_Database = database;
            m_Container = container;
            dbPathLabel.Text = $"Create in: {m_Database}/{m_Container}";
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnCreateButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(itemNameTB.Text))
            {
                MessageBox.Show("Please enter a name for the item!",
                                "Uh Oh!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(itemValueTB.Text))
            {
                MessageBox.Show("Please enter a value for the item!",
                                "Uh Oh!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(m_Database))
                return;

            int code = DatabaseManager.AddItem(m_Database, m_Container, itemNameTB.Text, itemValueTB.Text);
            switch (code)
            {
                case 0:
                    MainWindow.GetInstance().RefreshDataView();
                    this.Close();
                    break;
                case 1:
                    MessageBox.Show("An item with the same name is already loaded! Right click on the existing one to edit it.",
                                    "Uh Oh!",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    break;
            }
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                createButton.PerformClick();
        }

        private void OnTextBoxNameKeyPressed(object sender, KeyPressEventArgs e)
        {
            e.Handled = DatabaseManager.IsIllegalChar(e.KeyChar);
        }

        private void OnOpenEditorButtonClicked(object sender, EventArgs e)
        {
            JsonEditorWindow editor = new JsonEditorWindow("{}");
            editor.ShowDialog();
            if (!string.IsNullOrWhiteSpace(editor.Data))
                itemValueTB.Text = editor.Data;
        }
    }
}