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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="database">The database's name.</param>
        /// <param name="container">The container's name.</param>
        /// <param name="itemName">The item's name.</param>
        /// <param name="itemValue">The item's value.</param>
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
        
        /// <summary>
        /// Occurs when the cancel button is clicked.
        /// Closes the dialog.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Occurs when the create button is clicked.
        /// Attemps to edit the item from a specified container.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
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

        /// <summary>
        /// Occurs when the ENTER key is down while focusing the <see cref="itemValueTB"/>.
        /// Calls the <see cref="editButton"/>'s click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                editButton.PerformClick();
        }

        /// <summary>
        /// Occurs when a key is pressed while focusing the <see cref="itemNameTB"/>.
        /// Checks for illegal character.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnTextBoxNameKeyPressed(object sender, KeyPressEventArgs e)
        {
            e.Handled = DatabaseManager.IsIllegalChar(e.KeyChar);
        }

        /// <summary>
        /// Occurs when the open editor button is clicked.
        /// Opens the JSON editor.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnOpenEditorClicked(object sender, EventArgs e)
        {
            JsonEditorWindow editor = new JsonEditorWindow(itemValueTB.Text);
            editor.ShowDialog();
            if (!string.IsNullOrWhiteSpace(editor.Data))
                itemValueTB.Text = editor.Data;
        }
    }
}