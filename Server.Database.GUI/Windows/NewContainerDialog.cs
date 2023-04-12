using Server.Database.Management;
using System;
using System.Windows.Forms;

namespace Server.Database.Windows
{
    public partial class NewContainerDialog : Form
    {
        private readonly string m_Database;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="database">The database's name.</param>
        public NewContainerDialog(string database)
        {
            InitializeComponent();

            m_Database = database;
            dbPathLabel.Text = $"Create in: {m_Database}";
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
        /// Attemps to creates and add a container to the specified database.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCreateButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(containerNameTB.Text))
            {
                MessageBox.Show("Please enter a name for the container!",
                                "Uh Oh!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(m_Database))
                return;

            int code = DatabaseManager.AddContainer(m_Database, containerNameTB.Text);
            switch (code)
            {
                case 0:
                    int index = MainWindow.GetInstance().AddContainerNode(m_Database, containerNameTB.Text);
                    if (index != -1)
                        MainWindow.GetInstance().DatabaseTreeView.Nodes[0].Nodes[index].Expand();
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
        /// Occurs when the ENTER key is down.
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
        /// Occurs when a key is pressed while focusing the <see cref="containerNameTB"/>.
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