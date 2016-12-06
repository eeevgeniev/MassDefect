namespace MassDefect.DefectIO.Contracts
{
    using System.IO;

    public interface IFileWriteable
    {
        void Write(string path, string data);

        StreamWriter GetWriter(string path);
    }
}
