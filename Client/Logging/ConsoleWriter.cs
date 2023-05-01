using System;
using System.IO;
using System.Text;

namespace Client.Logging
{
    internal class ConsoleWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.Default;

        public TextWriter TextWriter { get; set; }
        public FileStream FileStream { get; set; }
        public StreamWriter StreamWriter { get; set; }

        private static ConsoleWriter s_ConsoleWriter;
        private static bool s_Initialized;

        public static void Initialize(string fileName)
        {
            if (s_Initialized)
                return;

            s_ConsoleWriter = new ConsoleWriter();
#if DEBUG
            s_ConsoleWriter.TextWriter = Console.Out;
#endif
            s_ConsoleWriter.FileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            s_ConsoleWriter.StreamWriter = new StreamWriter(s_ConsoleWriter.FileStream);

            s_ConsoleWriter.StreamWriter.AutoFlush = true;

            Console.SetOut(s_ConsoleWriter);

            s_Initialized = true;
        }

        public override void Write(string value)
        {
            if (TextWriter != null)
                TextWriter.Write(value);
            
            if (StreamWriter != null)
                StreamWriter.Write(value);
        }

        public override void Close()
        {
            base.Close();

            FileStream.Close();
            StreamWriter.Close();
        }
    }
}