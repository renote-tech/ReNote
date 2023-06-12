using System;
using System.IO;
using System.Net.Sockets;
using Server.Common;
using Server.ReNote.Management;
using Server.Web.Api;
using Server.Web.Static;

namespace Server
{
    internal class Program
    {
        /// <summary>
        /// Returns whether the app is terminating.
        /// </summary>
        static bool isTerminating = false;

        /// <summary>
        /// Entry point of the application; 
        /// loads main components and configurations.
        /// </summary>
        static void Main()
        {
            Platform.Initialize();
            Configuration.LoadAll();

            InitializeProcessEvents();

            StaticInterface.Instance.Initialize();
            ApiInterface.Instance.Initialize();

            ApiAtlas.Initialize("Root", "/",

                                "Authenticate", "/global/auth",
                                "SchoolInfo", "/global/school/info",
                                "About", "/global/about",
                                "Quotation", "/global/quotation",
                                "Configuration", "/global/client/config",

                                "Profile", "/user/profile",
                                "Preferences", "/user/preferences",
                                "Timetable", "/user/timetable",
                                "LogOut", "/user/session/delete",
                                "TeamProfile", "/user/team/profile",
                                "Password", "/user/password/modify");

            ReNote.Server.Instance.Initialize();

            StaticInterface.Instance.Start();
            ApiInterface.Instance.Start();
        }

        /// <summary>
        /// Register Application Domain events.
        /// </summary>
        static void InitializeProcessEvents()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                if (e.Exception is SocketException || e.Exception is IOException)
                    return;

                Platform.Log($"Caused by {e.Exception.Source} | {e.Exception.GetType()} | {e.Exception.Message}\n{e.Exception.StackTrace}\n", LogLevel.ERROR);
            };

            AppDomain.CurrentDomain.ProcessExit += (sender, e) => EndService();
            Console.CancelKeyPress += (sender, e) => EndService();
        }

        /// <summary>
        /// Saves data and shut down the server.
        /// </summary>
        static void EndService()
        {
            if (isTerminating)
                return;

            isTerminating = true;

            Platform.Log("Shutting down", LogLevel.INFO);

            SessionManager.Clean(false);
            ReNote.Server.Database.Save();

            Environment.Exit(0);
        }
    }
}