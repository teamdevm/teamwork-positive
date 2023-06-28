using System;
using System.IO; 

namespace Documently.Models
{
    public class Converter
    {
        public static string ToXml(string namePattern)
        {

            if (Path.GetExtension(namePattern) == ".docx" ||
                Path.GetExtension(namePattern) == ".doc" ||
                Path.GetExtension(namePattern) == ".rtf" ||
                Path.GetExtension(namePattern) == ".html" ||
                Path.GetExtension(namePattern) == ".txt")
            {
                var doc = new Aspose.Words.Document(namePattern);

                string startupPath = System.IO.Directory.GetCurrentDirectory();

                int counter = 0;
                string counterStr = "";
                while (File.Exists(Path.Join(startupPath, Path.GetFileNameWithoutExtension(namePattern) + counterStr + ".xml")))
                {
                    counter++;
                    counterStr = " (" + counter + ")";
                }

                doc.Save(Path.Join(startupPath, Path.GetFileNameWithoutExtension(namePattern) + counterStr + ".xml"), Aspose.Words.SaveFormat.FlatOpc);

                return Path.Join(startupPath, Path.GetFileNameWithoutExtension(namePattern) + counterStr + ".xml");
            }
            else
               throw new ArgumentException ($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }
    }
}
