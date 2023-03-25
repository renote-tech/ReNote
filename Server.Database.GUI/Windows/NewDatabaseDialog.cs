using Server.Database.Management;
using System;
using System.Windows.Forms;

using RNDatabase = Server.ReNote.Data.Database;

namespace Server.Database.Windows
{
    public partial class NewDatabaseDialog : Form
    {
        public NewDatabaseDialog()
        {
            InitializeComponent();
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnCreateButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dbNameTextBox.Text))
            {
                MessageBox.Show("Please enter a name for the database!",
                                "Uh Oh!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            RNDatabase database = DatabaseManager.CreateDatabase(dbNameTextBox.Text);
            if(database == null)
            {
                MessageBox.Show("A database with the same name is already loaded! Unmount the existing one and try again.",
                "Uh Oh!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            MainWindow.GetInstance().AddDatabaseNode(dbNameTextBox.Text);
            this.Close();
        }
    }
}
