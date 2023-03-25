using Server.Database.Management;
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
            pathLabel.Text = $"{m_Database}/{m_Container}";
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnCreateButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(docNameTextBox.Text))
            {
                MessageBox.Show("Please enter a name for the item!",
                                "Uh Oh!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(docValueTextBox.Text))
            {
                MessageBox.Show("Please enter a value for the item!",
                                "Uh Oh!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(m_Database))
                return;

            int code = DatabaseManager.AddItem(m_Database, m_Container, docNameTextBox.Text, docValueTextBox.Text);
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
    }
}
