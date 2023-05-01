using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Client.Builders;

namespace Client.Windows
{
    public partial class MessageBox : Window
    {
        public MessageBoxCode ExitCode { get; set; }


        private static Bitmap s_QuestionIcon;
        private static Bitmap s_WarnIcon;
        private static Bitmap s_ErrorIcon;
        private static Bitmap s_InfoIcon;
        private bool m_HasExited = false;

        public MessageBox()
        {
            InitializeComponent();
        }

        public MessageBox(string title, string message, MessageBoxType boxType, MessageBoxIcon boxIcon)
        {
            InitializeComponent();
            InitializeWindow();

            Title = title;
            m_MessageLabel.Text = message;
            m_CancelButton.IsVisible = boxType == MessageBoxType.YES_CANCEL;

            switch (boxIcon)
            {
                case MessageBoxIcon.QUESTION:
                    m_IconImage.Source = s_QuestionIcon;
                    break;
                case MessageBoxIcon.WARN:
                    m_IconImage.Source = s_WarnIcon;
                    break;
                case MessageBoxIcon.ERROR:
                    m_IconImage.Source = s_ErrorIcon;
                    break;
                case MessageBoxIcon.INFO:
                default:
                    m_IconImage.Source = s_InfoIcon;
                    break;
            }
        }

        private void InitializeWindow()
        {
            base.OnInitialized();

            Closing += (sender, e) =>
            {
                if (!m_HasExited)
                {
                    ExitCode = MessageBoxCode.CLOSED;
                    m_HasExited = true;
                }
            };

            m_ConfirmButton.Click += (sender, e) => CloseDialog("CONFIRM");
            m_CancelButton.Click += (sender, e) => CloseDialog("CANCEL");

            IAssetLoader loader = AvaloniaLocator.Current.GetService<IAssetLoader>();

            s_QuestionIcon ??= new Bitmap(loader.Open(new Uri("avares://Client/Assets/question.png")));
            s_WarnIcon ??= new Bitmap(loader.Open(new Uri("avares://Client/Assets/warn.png")));
            s_ErrorIcon ??= new Bitmap(loader.Open(new Uri("avares://Client/Assets/error.png")));
            s_InfoIcon ??= new Bitmap(loader.Open(new Uri("avares://Client/Assets/info.png")));
        }

        private void CloseDialog(string closeType)
        {
            switch (closeType)
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

            m_HasExited = true;
            Close();
        }
    }
}