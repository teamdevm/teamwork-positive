using System.Data;
using System.Reflection.Metadata;

namespace Converter
{
    class Program
    {
        public static void Main(string[] args)
        {
            string pathPattern; // путь к шаблону
            string pathFolder; // путь к месту сохранения
            string fileName; // имя файла
            string startupPath = System.IO.Directory.GetCurrentDirectory();

            Console.WriteLine("Tests." +
                "\n1.\tDocx to Xml" +
                "\n2.\tXml to Docx" +
                "\n3.\tDoc to Xml" +
                "\n4.\tXml to Doc" +
                "\n5.\tRtf to Xml" +
                "\n6.\tHtml to Xml" +
                "\n7.\tTxt to Xml" +
                "\n8.\tXml to Odt" +
                "\n9.\tXml to Rtf" +
                "\n10.\tXml to Html" +
                "\n11.\tXml to Txt" +
                "\n12.\tXml to Pdf" +
                "\n13.\tXml to Md" +
                "\n14\tXml to OOXml");
            string choiceTest = "";

            while (choiceTest != "0")
            {
                choiceTest = Console.ReadLine();
                switch (choiceTest)
                {
                    case "1":
                        pathPattern = $"{startupPath}\\Tests\\договор аренды квартиры.docx";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "DocToXml Test";
                        Converter.DocxToXml(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "2":
                        pathPattern = $"{startupPath}\\Tests\\WordToXml Test.xml";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "XmlToDocx Test";
                        Converter.XmlToDocx(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "3":
                        pathPattern = $"{startupPath}\\Tests\\Договор дарения.doc";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "DocToXml Test";
                        Converter.DocToXml(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "4":
                        pathPattern = $"{startupPath}\\Tests\\OldWordToXml Test.xml";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "XmlToDoc Test";
                        Converter.XmlToDoc(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "5":
                        pathPattern = $"{startupPath}\\Tests\\договор-аренды-квартиры.rtf";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "RtfToXml Test";
                        Converter.RtfToXml(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "6":
                        pathPattern = $"{startupPath}\\Tests\\договор аренды квартиры.html";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "HtmlToXml Test";
                        Converter.HtmlToXml(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "7":
                        pathPattern = $"{startupPath}\\Tests\\договор аренды квартиры.txt";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "TxtToXml Test";
                        Converter.TxtToXml(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "8":
                        pathPattern = $"{startupPath}\\Tests\\WordToXml Test.xml";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "XmlToOdt Test";
                        Converter.XmlToOdt(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "9":
                        pathPattern = $"{startupPath}\\Tests\\RtfToXml Test.xml";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "XmlToRtf Test";
                        Converter.XmlToRtf(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "10":
                        pathPattern = $"{startupPath}\\Tests\\HtmlToXml Test.xml";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "XmlToHtml Test";
                        Converter.XmlToHtml(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "11":
                        pathPattern = $"{startupPath}\\Tests\\TxtToXml Test.xml";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "XmlToTxt Test";
                        Converter.XmlToTxt(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "12":
                        pathPattern = $"{startupPath}\\Tests\\WordToXml Test.xml";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "XmlToPdf Test";
                        Converter.XmlToPdf(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "13":
                        pathPattern = $"{startupPath}\\Tests\\WordToXml Test.xml";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "XmlToMd Test";
                        Converter.XmlToMd(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "14":
                        pathPattern = $"{startupPath}\\Tests\\WordToXml Test.xml";
                        pathFolder = $"{startupPath}\\Tests";
                        fileName = "XmlToOOXml Test";
                        Converter.XmlToOOXml(pathPattern, pathFolder, fileName);
                        Console.WriteLine("Готово, барин");
                        break;
                    case "0":
                        choiceTest = "0";
                        break;
                }

            }
        }
    }
}
    