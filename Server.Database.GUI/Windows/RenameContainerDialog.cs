using Server.Database.Management;
using System;
using System.Windows.Forms;

namespace Server.Database.Windows
{
    public partial class RenameContainerDialog : Form
    {
        private string m_Database;
        private string m_Container;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="database">The database's name.</param>
        /// <param name="container">The container's name.</param>
        public RenameContainerDialog(string database, string container)
        {
            InitializeComponent();

            m_Database = database;
            m_Container = container;

            newContainerNameTB.Text = container;
            dbPathLabel.Text = $"Edit in: {m_Database} $ {m_Container}";
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
        /// Attemps to rename a container from a specified database.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCreateButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(newContainerNameTB.Text))
            {
                MessageBox.Show("Please enter a name for the container!",
                                "Uh Oh!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(m_Database))
                return;

            int code = DatabaseManager.RenameContainer(m_Database, m_Container, newContainerNameTB.Text);
            switch (code)
            {
                case 0:

                    MainWindow.GetInstance().RenameContainerNode(m_Database, m_Container, newContainerNameTB.Text);
                    this.Close();
                    break;
                case 1:
                    MessageBox.Show("A container with the same name is already loaded!",
                                    "Uh Oh!",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    break;
            }
        }

        /// <summary>
        /// Occurs when the ENTER key is down while focusing the <see cref="newContainerNameTB"/>.
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
        /// Occurs when a key is pressed while focusing the <see cref="newContainerNameTB"/>
        /// Checks for illegal character.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnTextBoxKeyPressed(object sender, KeyPressEventArgs e)
        {
            e.Handled = DatabaseManager.IsIllegalChar(e.KeyChar);
        }
    }
}