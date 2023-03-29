using Server.Database.Management;
using System;
using System.Windows.Forms;

namespace Server.Database.Windows
{
    public partial class EditItemDialog : Form
    {
        private string m_Database;
        private string m_Container;
        private string m_ItemName;
        private string m_ItemValue;

        public EditItemDialog(string database, string container, string itemName, string itemValue)
        {
            InitializeComponent();

            m_Database = database;
            m_Container = container;
            m_ItemName = itemName;
            m_ItemValue = itemValue;

            dbPathLabel.Text = $"Edit in: {m_Database}/{m_Container} $ {m_ItemName}";
            itemNameTB.Text = m_ItemName;
            itemValueTB.Text = m_ItemValue;
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

            if (string.IsNullOrWhiteSpace(m_Database) || string.IsNullOrWhiteSpace(m_Container))
                return;

            if (m_ItemName == itemNameTB.Text && m_ItemName == itemValueTB.Text)
                this.Close();

            int code = DatabaseManager.EditItem(m_Database, m_Container, m_ItemName, itemNameTB.Text, itemValueTB.Text);
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
                editButton.PerformClick();
        }

        private void OnTextBoxNameKeyPressed(object sender, KeyPressEventArgs e)
        {
            e.Handled = DatabaseManager.IsIllegalChar(e.KeyChar);
        }

        private void OnOpenEditorClicked(object sender, EventArgs e)
        {
            JsonEditorWindow editor = new JsonEditorWindow(itemValueTB.Text);
            editor.ShowDialog();
            if (!string.IsNullOrWhiteSpace(editor.Data))
                itemValueTB.Text = editor.Data;
        }
    }
}