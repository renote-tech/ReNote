using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Client.Builders;

namespace Client.Windows
{
    public partial class MessageBox : Window
    {
        public MessageBoxCode ExitCode { get; set; }

        private bool isButtonExit = false;
        
        public MessageBox()
        {
            InitializeComponent();
        }

        public MessageBox(string title, string message, MessageBoxType style, MessageBoxIcon icon)
        {
            InitializeComponent();

            Title = title;
            boxMessage.Text = message;
            
            switch(style)
            {
                case MessageBoxType.OK:
                    boxCancel.IsVisible = false;
                    boxConfirm.Content = "OK";
                    break;
                case MessageBoxType.YES_CANCEL:
                default:
                    boxCancel.IsVisible = true;
                    boxCancel.Content = "Cancel";
                    break;
            }

            IAssetLoader loader = AvaloniaLocator.Current.GetService<IAssetLoader>();

            switch(icon)
            {
                case MessageBoxIcon.QUESTION:
                    boxIcon.Source = new Bitmap(loader.Open(new Uri("avares://Client/Assets/question.png")));
                    break;
                case MessageBoxIcon.WARN:
                    boxIcon.Source = new Bitmap(loader.Open(new Uri("avares://Client/Assets/warn.png")));
                    break;
                case MessageBoxIcon.ERROR:
                    boxIcon.Source = new Bitmap(loader.Open(new Uri("avares://Client/Assets/error.png")));
                    break;
                case MessageBoxIcon.INFO:
                default:
                    boxIcon.Source = new Bitmap(loader.Open(new Uri("avares://Client/Assets/info.png")));
                    break;
            }
        }

        private void OnConfirmClicked(object sender, RoutedEventArgs e)
        {
            if (sender is not Button)
                return;

            isButtonExit = true;
            switch(((Button)sender).Tag)
            {
                case "CONFIRM":
                    ExitCode = MessageBoxCode.OK;
                    break;
                case "CANCEL":
                    ExitCode = MessageBoxCode.CANCEL;
                    break;
                default:
                    ExitCode = MessageBoxCode.CLOSED;
                    break;
            }

            Close();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if(!isButtonExit)
                ExitCode = MessageBoxCode.CLOSED;
        }
    }
}