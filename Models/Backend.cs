using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Words;
using Aspose.Words.Replacing;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Linq;

namespace Documently.Models;

class Backend : ITemplateProcessor
{
    private MemoryStream pathPattern;
    //private string pathPattern;
    private string pathToFolder;
    private string fileName;

    public Backend()
    {
        pathPattern = null!;
        //pathPattern = string.Empty;
        pathToFolder = string.Empty;
        fileName = string.Empty;
    }


    public bool CheckFileName(Dictionary<string, ObservableCollection<Field>> table)
    {
        string copyName = fileName;

        foreach (var value in table)
            for (int i = 0; i < value.Value.Count; i++)
                copyName = copyName.Replace(value.Value[i].Name, "Переменная");

        // Path.GetInvalidFileNameChars();

        if (copyName.IndexOf("\\") >= 0 || copyName.IndexOf("/") >= 0 || copyName.IndexOf(":") >= 0 ||
            copyName.IndexOf("*") >= 0 || copyName.IndexOf("?") >= 0 || copyName.IndexOf("|") >= 0 ||
            copyName.IndexOf("<") >= 0 || copyName.IndexOf(">") >= 0 || copyName == "")
        {
            throw new ArgumentException("Недопустимый символ в имени файла или пустой ввод.Запрещено использовать такие символы, как \\, /, :, *, ?, |, <, > \n" +
                "(Знаки <> можно использовать в том случае, если они используются для определения переменной в названии файла)\n" +
                "Попробуйте ввести имя еще раз");
        }

        return true;
    }


    public Document Setup(MemoryStream name, string path, string pattern)
    {
        pathPattern = name;
        pathToFolder = path;
        fileName = pattern;
        return new Document(pathPattern);
    }

    public Dictionary<string, ObservableCollection<Field>> GetFields()
    {
        ObservableCollection<Field> table;
        Document doc = new Document(pathPattern);
        Dictionary<string, ObservableCollection<Field>> dicCategory = new Dictionary<string, ObservableCollection<Field>>();
        Field f;
        CurrentDateField curDate;
        int pos = 0;
        foreach (Paragraph p in doc.GetChildNodes(NodeType.Paragraph, true))
        {
            int left = p.ToString(SaveFormat.Text).IndexOf("<"), right = p.ToString(SaveFormat.Text).IndexOf(">");
            string varStr, nameCategory, pStr = p.ToString(SaveFormat.Text);

            while (left >= 0 && right >= 0)
            {
                varStr = pStr.Substring(left + 1, right - left - 1);

                int placeCategory = varStr.IndexOf(':');
                int counter = 0;
                if (placeCategory >= 0)
                    counter = varStr.IndexOf(':', placeCategory + 1);

                if (counter > 0)
                    throw new ArgumentException("У переменной может быть только одна категория!");

                pos++;
                if (placeCategory >= 0)
                {
                    nameCategory = varStr.Substring(placeCategory + 1, varStr.Length - placeCategory - 1);
                    varStr = varStr.Remove(placeCategory, varStr.Length - placeCategory); 
                }
                else    
                    nameCategory = "Общие данные";

                // Display name
                string displayName = "";
                char prevChar = char.MinValue; 
                int index = 0;
                foreach (char c in varStr)
                {
                    index++;
                    if (displayName == "")
                        displayName += c;  
                    else if (char.IsUpper(prevChar) && char.IsUpper(c))
                    {
                        displayName = displayName.Substring(0, displayName.Count() - 1) + prevChar + c;
                    }
                    else if (char.IsUpper(c) && char.IsLower(prevChar))
                    {
                        displayName += " " + char.ToLower(c);
                    }
                    else
                        displayName += c;
                    prevChar = c;
                }

                curDate = new CurrentDateField(varStr, displayName, nameCategory);
                f = new TextField(varStr, displayName, nameCategory);

                if (!dicCategory.ContainsKey(nameCategory))
                {
                    if (nameCategory == "Дата")
                    {
                        table = new ObservableCollection<Field> { curDate };
                        dicCategory.Add(nameCategory, table);
                    }
                    else
                    {
                        table = new ObservableCollection<Field> { f };
                        dicCategory.Add(nameCategory, table);
                    }
                }
                else
                {
                    table = new ObservableCollection<Field> { };
                    table = dicCategory[nameCategory];
                    int check = dicCategory[nameCategory].Where(x => x.Name == varStr).Count();
                    if (check == 0)
                    {
                        if (nameCategory == "Дата")
                        {
                            dicCategory[nameCategory].Add(curDate);
                        }
                        else
                        {
                            dicCategory[nameCategory].Add(f);
                        }
                    }
                }

                pStr = pStr.Remove(0, right + 1);
                left = pStr.IndexOf("<");
                right = pStr.IndexOf(">");
            }
        }
            return dicCategory;
    }

    public Document Fill(Dictionary<string, ObservableCollection<Field>> record)
    {
        Document doc = new Document(pathPattern);

        foreach (var value in record)
            for (int j = 0; j < value.Value.Count; j++)
            {
                FindReplaceOptions options = new FindReplaceOptions();
                options.MatchCase = false;
                options.FindWholeWordsOnly = false;
                options.Direction = FindReplaceDirection.Forward;
                if (value.Value[j].Category == "Общие данные" && value.Value[j].Value != "")
                    doc.Range.Replace("<" + value.Value[j].Name + ">", value.Value[j].Value, options);
                else if (value.Value[j].Value != "")
                    doc.Range.Replace("<" + value.Value[j].Name + ":" + value.Value[j].Category + ">", value.Value[j].Value, options);
            }

        return doc;
    }

    public void Save(Document doc, string extension)
    {
        string name = fileName;
        int counter = 0;
        string counterStr = "";

        while (File.Exists(Path.Join(pathToFolder, name + counterStr + extension)))
        {
            counter++;
            counterStr = " (" + counter + ")";
        }

        switch (extension)
        {
            case ".docx":
                doc.Save(Path.Join(pathToFolder, name + counterStr), SaveFormat.Docx);
                break;
            case ".doc":
                doc.Save(Path.Join(pathToFolder, name + counterStr), SaveFormat.Doc);
                break;
            case ".rtf":
                doc.Save(Path.Join(pathToFolder, name + counterStr), SaveFormat.Rtf);
                break;
            case ".html":
                doc.Save(Path.Join(pathToFolder, name + counterStr), SaveFormat.Html);
                break;
            case ".txt":
                doc.Save(Path.Join(pathToFolder, name + counterStr), SaveFormat.Text);
                break;
            case ".odt":
                doc.Save(Path.Join(pathToFolder, name + counterStr), SaveFormat.Odt);
                break;
            case ".pdf":
                doc.Save(Path.Join(pathToFolder, name + counterStr), SaveFormat.Pdf);
                break;
            case ".md":
                doc.Save(Path.Join(pathToFolder, name + counterStr));
                break;
            case ".xml":
                doc.Save(Path.Join(pathToFolder, name + counterStr), SaveFormat.FlatOpc);
                break;
            default:
                throw new ArgumentException($"Не знаю, что такое .{extension}");
        }
    }

    public void Dispose()
    {
    }
}
