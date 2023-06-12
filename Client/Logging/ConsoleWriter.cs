namespace Client.Logging;

using System;
using System.IO;
using System.Text;

internal class ConsoleWriter : TextWriter
{
    public override Encoding Encoding => Encoding.Default;

    public TextWriter TextWriter { get; set; }
    public FileStream FileStream { get; set; }
    public StreamWriter StreamWriter { get; set; }

    private static ConsoleWriter s_ConsoleWriter;

    public static void Initialize(string fileName)
    {
        s_ConsoleWriter = new ConsoleWriter
        {
            FileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write),
            StreamWriter = new StreamWriter(s_ConsoleWriter.FileStream)
        };

        s_ConsoleWriter.StreamWriter.AutoFlush = true;

        Console.SetOut(s_ConsoleWriter);
    }

    public override void Write(string value)
    {
        TextWriter?.Write(value);
        StreamWriter?.Write(value);
    }

    public override void Close()
    {
        base.Close();

        FileStream.Close();
        StreamWriter.Close();
    }
}