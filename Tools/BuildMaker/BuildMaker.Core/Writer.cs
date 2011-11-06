using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BuildMaker.Core
{
    public class Writer
    {
        StringBuilder sb = new StringBuilder();

        public Writer()
        {
        }

        public void Write(string message)
        {
            sb.Append(message);
            Console.Write(message);            
        }

        public void WriteLine()
        {
            sb.Append(System.Environment.NewLine);
            Console.WriteLine();
        }
        public void WriteLine(string message)
        {
            sb.Append(message);
            sb.Append(System.Environment.NewLine);
            Console.WriteLine(message);
        }

        public void Clear()
        {
            sb.Clear();
        }

        public bool WriteToDiskAndClear(string fileName)
        {
            if (!WriteToDisk(fileName)) return false;            
            Clear();
            return true;            
        }
        public bool WriteToDisk(string fileName)
        {
            try
            {
                File.WriteAllText(fileName, sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}
