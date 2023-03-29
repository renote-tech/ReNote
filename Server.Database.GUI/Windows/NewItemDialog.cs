using Server.Database.Management;
using System;
using System.Windows.Forms;

namespace Server.Database.Windows
{
    public partial class NewItemDialog : Form
    {
        private string m_Database;
        private string m_Container;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="database">The database's name.</param>
        /// <param name="container">The container's name.</param>
        public NewItemDialog(string database, string container)
        {
            InitializeComponent();

            m_Database = database;
            m_Container = container;
            dbPathLabel.Text = $"Create in: {m_Database}/{m_Container}";
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
        /// Attemps to create an item for the specified container.
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

        /// <summary>
        /// Occurs when the ENTER key is down while focusing the <see cref="itemValueTB"/>.
        /// Calls the <see cref="createButton"/>'s click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                createButton.PerformClick();
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
        private void OnOpenEditorButtonClicked(object sender, EventArgs e)
        {
            JsonEditorWindow editor = new JsonEditorWindow("{}");
            editor.ShowDialog();
            if (!string.IsNullOrWhiteSpace(editor.Data))
                itemValueTB.Text = editor.Data;
        }
    }
}