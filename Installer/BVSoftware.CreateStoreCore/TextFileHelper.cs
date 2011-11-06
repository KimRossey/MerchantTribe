using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BVSoftware.CreateStoreCore
{
    public class TextFileHelper
    {
        public static void ReplaceInTextFile(string sourceFilePath,
                                           string wordToReplace,
                                           string replacement)
        {
            if (File.Exists(sourceFilePath))
            {
                StreamReader stream = new StreamReader(sourceFilePath);
                string fileText = stream.ReadToEnd();
                stream.Close();

                fileText = fileText.Replace(wordToReplace, replacement);

                StreamWriter writer = new StreamWriter(sourceFilePath, false);
                writer.Write(fileText);
                writer.Close();
            }

        }
    }
}
