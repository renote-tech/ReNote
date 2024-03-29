﻿using System;
using System.Threading.Tasks;
using Server.Common;

namespace Server
{
    public class Platform
    {
        /// <summary>
        /// True if the <see cref="Platform"/> class is initialized; otherwise false.
        /// </summary>
        private static bool s_Initialized;

        /// <summary>
        /// Initializes the <see cref="Platform"/> class.
        /// </summary>
        public static void Initialize()
        {
            if (s_Initialized)
                return;

            Console.Title = "ReNote \u03a9 2023";
            string watermark = @"      ____       _   __      __      " + "\n" +
                               @"     / __ \___  / | / /___  / /____  " + "\n" +
                               @"    / /_/ / _ \/  |/ / __ \/ __/ _ \ " + "\n" +
                               @"   / _, _/  __/ /|  / /_/ / /_/  __/ " + "\n" +
                               @"  /_/ |_|\___/_/ |_/\____/\__/\___/  ";

            Console.WriteLine($"{watermark} Version {ServerInfo.Version} ({ServerInfo.Configuration})\n");

            Log("Thanks for choosing ReNote!", LogLevel.INFO);
            Log($"Running on {ServerInfo.GetPlatformName()}", LogLevel.INFO);

            s_Initialized = true;
        }

        /// <summary>
        /// Outputs a message to the console.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        /// <param name="level">The <see cref="LogLevel"/> of the message.</param>
        public static void Log(string message, LogLevel level = LogLevel.DEBUG)
        {
            if (!ServerInfo.IsDebug && level == LogLevel.DEBUG)
                return;

            switch (level)
            {
                case LogLevel.DEBUG:
                    ServerConsole.Log(message, level, "Green");
                    break;
                case LogLevel.INFO:
                    ServerConsole.Log(message, level, "DarkGreen");
                    break;
                case LogLevel.WARN:
                    ServerConsole.Log(message, level, "Yellow");
                    break;
                case LogLevel.ERROR:
                    ServerConsole.Log(message, level, "Red");
                    break;
                case LogLevel.FATAL:
                    ServerConsole.Log(message, level, "DarkRed");
                    break;
            }
        }
    }
}