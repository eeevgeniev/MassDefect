namespace MassDefect.DefectIO
{
    using System;
    using System.IO;
    using Contracts;

    public class FileIO : IFileReadableWriteable
    {
        public StreamWriter GetWriter(string path)
        {
            return new StreamWriter(path);
        }

        public string Read(string path)
        {
            string data = string.Empty;

            using (StreamReader reader = new StreamReader(path))
            {
                data = reader.ReadToEnd();
            }

            return data;
        }

        public void Write(string path, string data)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(data);
            }
        }
    }
}
