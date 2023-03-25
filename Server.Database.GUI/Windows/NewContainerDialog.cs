using Server.Database.Management;
using System;
using System.Windows.Forms;

namespace Server.Database.Windows
{
    public partial class NewContainerDialog : Form
    {
        private string m_Database;

        public NewContainerDialog(string database)
        {
            InitializeComponent();

            m_Database = database;
            databaseLabel.Text = m_Database;
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnCreateButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(dbNameTextBox.Text))
            {
                MessageBox.Show("Please enter a name for the container!",
                                "Uh Oh!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(m_Database))
                return;

            int code = DatabaseManager.AddContainer(m_Database, dbNameTextBox.Text);
            switch( code)
            {
                case 0:
                    MainWindow.GetInstance().AddContainerNode(m_Database, dbNameTextBox.Text);
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
    }
}
