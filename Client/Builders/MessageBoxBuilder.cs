using System.Threading.Tasks;
using Avalonia.Controls;
using Client.Windows;

namespace Client.Builders
{
    internal class MessageBoxBuilder
    {
        private string m_Title;
        private string m_Message;
        private MessageBoxType m_MessageBoxType;
        private MessageBoxIcon m_MessageBoxIcon;

        public static MessageBoxBuilder Create()
        {
            return new MessageBoxBuilder();
        }

        public MessageBoxBuilder SetTitle(string title)
        {
            m_Title = title;
            return this;
        }

        public MessageBoxBuilder SetMessage(string message)
        {
            m_Message = message; 
            return this;
        }

        public MessageBoxBuilder SetType(MessageBoxType style)
        {
            m_MessageBoxType = style;
            return this;
        }

        public MessageBoxBuilder SetIcon(MessageBoxIcon icon)
        {
            m_MessageBoxIcon = icon;
            return this;
        }

        public MessageBox GetWindow()
        {
            return new MessageBox(m_Title, m_Message, m_MessageBoxType, m_MessageBoxIcon)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
        }

        public void Show(Window owner = default)
        {
            if (owner == default)
                owner = MainWindow.Instance;

            MessageBox dialog = new MessageBox(m_Title, m_Message, m_MessageBoxType, m_MessageBoxIcon);
            dialog.ShowDialog(owner);
        }

        public async Task<MessageBoxCode> ShowAsync(Window owner = default)
        {
            if (owner == default)
                owner = MainWindow.Instance;

            MessageBox dialog = new MessageBox(m_Title, m_Message, m_MessageBoxType, m_MessageBoxIcon);
            await dialog.ShowDialog(owner);

            return dialog.ExitCode;
        }
    }

    public enum MessageBoxType
    {
        OK         = 0,
        YES_CANCEL = 1
    }

    public enum MessageBoxIcon
    {
        INFO     = 0,
        QUESTION = 1,
        WARN     = 2,
        ERROR    = 3
    }

    public enum MessageBoxCode
    {
        OK     = 0,
        CANCEL = 1,
        CLOSED = 2
    }
}
