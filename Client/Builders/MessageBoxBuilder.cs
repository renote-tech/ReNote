using Avalonia.Controls;
using Client.Windows;

namespace Client.Builders
{
    internal class MessageBoxBuilder
    {
        private string Title { get; set; }
        private string Message { get; set; }
        private MessageBoxType MessageBoxType { get; set; }
        private MessageBoxIcon MessageBoxIcon { get; set; }

        public static MessageBoxBuilder Create()
        {
            return new MessageBoxBuilder();
        }

        public MessageBoxBuilder SetTitle(string title)
        {
            Title = title;
            return this;
        }

        public MessageBoxBuilder SetMessage(string message)
        {
            Message = message; 
            return this;
        }

        public MessageBoxBuilder SetType(MessageBoxType style)
        {
            MessageBoxType = style;
            return this;
        }

        public MessageBoxBuilder SetIcon(MessageBoxIcon icon)
        {
            MessageBoxIcon = icon;
            return this;
        }

        public MessageBoxCode Show(Window owner = default)
        {
            if (owner == default)
                owner = MainWindow.Instance;

            MessageBox dialog = new MessageBox(Title, Message, MessageBoxType, MessageBoxIcon);
            dialog.ShowDialog(owner);

            return dialog.ExitCode;
        }

        public async Task<MessageBoxCode> ShowAsync(Window owner = default)
        {
            if (owner == default)
                owner = MainWindow.Instance;

            MessageBox dialog = new MessageBox(Title, Message, MessageBoxType, MessageBoxIcon);
            await dialog.ShowDialog(owner);

            return dialog.ExitCode;
        }
    }

    public enum MessageBoxType
    {
        OK,
        YES_CANCEL
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
