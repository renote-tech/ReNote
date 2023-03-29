using System;
using System.Windows.Forms;
using Server.Database.Windows;

namespace Server.Database.GUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainWindow());
        }
    }
}