using Server.Database.Management;
using System;
using System.Windows.Forms;

namespace Server.Database.Windows
{
    public partial class RenameContainerDialog : Form
    {
        private string m_Database;
        private string m_Container;

        public RenameContainerDialog(string database, string container)
        {
            InitializeComponent();

            m_Database = database;
            m_Container = container;

            newContainerNameTB.Text = container;
            dbPathLabel.Text = $"Edit in: {m_Database} $ {m_Container}";
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

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

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                createButton.PerformClick();
        }

        private void OnTextBoxKeyPressed(object sender, KeyPressEventArgs e)
        {
            e.Handled = DatabaseManager.IsIllegalChar(e.KeyChar);
        }
    }
}