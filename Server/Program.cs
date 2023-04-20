﻿using System;
using Server.Common;
using Server.ReNote;
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
            ApiRegisterer.Initialize();

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
                Platform.Log($"Caused by {e.Exception.Source} | {e.Exception.Message}\n{e.Exception.StackTrace}\n", LogLevel.ERROR);
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