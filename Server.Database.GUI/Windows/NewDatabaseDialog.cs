using System;
using System.Windows.Forms;

using Server.Database.Management;
using RNDatabase = Server.ReNote.Data.Database;

namespace Server.Database.Windows
{
    public partial class NewDatabaseDialog : Form
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public NewDatabaseDialog()
        {
            InitializeComponent();
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
        /// Attemps to create a database.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCreateButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dbNameTB.Text))
            {
                MessageBox.Show("Please enter a name for the database!",
                                "Uh Oh!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            RNDatabase database = DatabaseManager.CreateDatabase(dbNameTB.Text);
            if (database == null)
            {
                MessageBox.Show("A database with the same name is already loaded! Unmount the existing one and try again.",
                "Uh Oh!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            MainWindow.GetInstance().AddDatabaseNode(dbNameTB.Text);
            MainWindow.GetInstance().DatabaseTreeView.Nodes[0].Expand();
            this.Close();
        }

        /// <summary>
        /// Occurs when the ENTER key is down while focusing the <see cref="dbNameTB"/>.
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
        /// Occurs when a key is pressed while focusing the <see cref="dbNameTB"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnTextBoxKeyPressed(object sender, KeyPressEventArgs e)
        {
            e.Handled = DatabaseManager.IsIllegalChar(e.KeyChar);
        }
    }
}