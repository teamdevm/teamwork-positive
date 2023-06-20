using System;
using System.Data;

namespace Converter
{
    internal class Converter
    {
        public static void DocxToXml(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".docx")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".xml", Aspose.Words.SaveFormat.FlatOpc);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }
        public static void XmlToDocx(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".xml")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".docx", Aspose.Words.SaveFormat.Docx);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }

        public static void DocToXml(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".doc")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".xml", Aspose.Words.SaveFormat.FlatOpc);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }
        public static void XmlToDoc(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".xml")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".doc", Aspose.Words.SaveFormat.Doc);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }


        public static void RtfToXml(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".rtf")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".xml", Aspose.Words.SaveFormat.FlatOpc);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }
        public static void XmlToRtf(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".xml")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".rtf", Aspose.Words.SaveFormat.Rtf);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }


        public static void HtmlToXml(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".html")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".xml", Aspose.Words.SaveFormat.FlatOpc);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }
        public static void XmlToHtml(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".xml")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".html", Aspose.Words.SaveFormat.Html);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }


        public static void TxtToXml(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".txt")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".xml", Aspose.Words.SaveFormat.FlatOpc);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }
        public static void XmlToTxt(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".xml")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".txt", Aspose.Words.SaveFormat.Text);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }

        public static void XmlToOdt(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".xml")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".odt", Aspose.Words.SaveFormat.Odt);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }

        public static void XmlToPdf(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".xml")
            {
                var doc = new Aspose.Words.Document(namePattern);

                //doc.BindXml(namePattern); 

                doc.Save(pathFolder + "\\" + fileName + ".pdf", Aspose.Words.SaveFormat.Pdf);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }

        public static void XmlToMd(string namePattern, string pathFolder, string fileName)
        {
            if (Path.GetExtension(namePattern) == ".xml")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".md");
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        }

        public static void XmlToOOXml(string namePattern, string pathFolder, string fileName) 
        {
            if (Path.GetExtension(namePattern) == ".xml")
            {
                var doc = new Aspose.Words.Document(namePattern);

                doc.Save(pathFolder + "\\" + fileName + ".xml", Aspose.Words.SaveFormat.FlatOpc);
            }
            else
                Console.WriteLine($"Не понял, какой еще {Path.GetExtension(namePattern)}?!");
        } //це излишне
    }
}
