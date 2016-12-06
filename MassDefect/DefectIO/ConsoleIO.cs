namespace MassDefect.DefectIO
{
    using System;
    using Contracts;

    public class ConsoleIO : IReadeableWriteable
    {
        public string Read()
        {
            return Console.ReadLine();
        }

        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
