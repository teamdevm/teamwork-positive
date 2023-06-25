using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Words;
using Aspose.Words.Replacing;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

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

    //хз насколько оптимально это решение, особо не разбирался в вариациях
    // public static DataTable CreateTableFromWord(string pathPattern, string namePattern)
    // {
    //     DataTable table = new DataTable(namePattern);
    //     Document doc;
    //     try
    //     {
    //         doc = new Document(pathPattern); // проверку бы на открытие дока замутить
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.Message);
    //         return null;
    //     }

    //     foreach (Paragraph p in doc.GetChildNodes(NodeType.Paragraph, true))
    //     {
    //         int right = p.ToString(SaveFormat.Text).IndexOf("<"), left = p.ToString(SaveFormat.Text).IndexOf(">");
    //         string varStr, pStr = p.ToString(SaveFormat.Text);
    //         while (right >= 0 && left >= 0)
    //         {
    //             varStr = pStr.Substring(right, left - right + 1);
    //             if (!table.Contains(varStr))
    //                 table.Head.Add(varStr);
    //             pStr = pStr.Remove(0, left + 1);
    //             right = pStr.IndexOf("<");
    //             left = pStr.IndexOf(">");
    //         }
    //     }

    //     return table;
    // }

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
    // public static void Execute()
    // {
    //     DataTable table;
    //     string pathToPattern = "C:\\Users\\дом\\Desktop\\ФИТ\\3 Курс\\9 триместр\\Групповая работа\\Шаблоны\\Договоры\\договор аренды квартиры.docx";
    //     table = CreateTableFromWord(pathToPattern, "Договор аренды квартиры");

    //     FillTable(table);//заполняем табличку 

    //     string pathToFolder = GetPathToFolder(); //куда сохраняем, надо будет вызывать какой-нибудь файл-диалог

    //     string fileName = GetFileName(table);

    //     Console.WriteLine("Начать создание документов?(Да/Нет)");
    //     string readyOrNot = Console.ReadLine();
    //     if (readyOrNot == "Да")
    //     {
    //         Document doc;
    //         for (int i = 0; i < table.Body.Count; i++)
    //         {
    //             doc = new Document(pathToPattern);
    //             for (int j = 0; j < table.Head.Count; j++)
    //             {
    //                 FindReplaceOptions options = new FindReplaceOptions();
    //                 options.MatchCase = false;
    //                 options.FindWholeWordsOnly = false;
    //                 options.Direction = FindReplaceDirection.Forward;

    //                 doc.Range.Replace(table.Head[j], table.Body[i][j], options);
    //             }
    //             string name = fileName;
    //             for (int j = 0; j < table.Head.Count; j++)
    //                 name = name.Replace(table.Head[j], table.Body[i][j]);
    //             int counter = 0;
    //             string counterStr = "";
    //             while (File.Exists(pathToFolder + "\\" + name + " " + counterStr + ".docx"))
    //             {
    //                 counter++;
    //                 counterStr = "(" + counter + ")";
    //             }
    //             //if (counter > 0)
    //             //    counterStr = counter.ToString();
    //             doc.Save(pathToFolder + "\\" + name + " " + counterStr + ".docx");
    //             Console.WriteLine($"Документ {pathToFolder}\\{name} {counterStr}.docx создан");
    //         }
    //         Console.WriteLine("Все документы созданы");
    //     }
    // }

    public void Setup(MemoryStream name, string path, string pattern)
    {
        pathPattern = name;
        pathToFolder = path;
        fileName = pattern;
    }

    public Dictionary<string, ObservableCollection<Field>> GetFields()
    {
        ObservableCollection<Field> table;
        Document doc = new Document(pathPattern);
        Dictionary<string, ObservableCollection<Field>> dicCategory = new Dictionary<string, ObservableCollection<Field>>();
        Field f;
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

                f = new TextField(varStr, nameCategory);
                if (!dicCategory.ContainsKey(nameCategory))
                {
                    table = new ObservableCollection<Field>{ f };
                    dicCategory.Add(nameCategory, table);
                }
                else
                {
                    table = new ObservableCollection<Field>();
                    table = dicCategory[nameCategory];
                    if (!table.Contains(f))
                    {
                        table.Add(f);
                        dicCategory[nameCategory] = table;
                    }
                }

                pStr = pStr.Remove(0, right + 1);
                left = pStr.IndexOf("<");
                right = pStr.IndexOf(">");
            }
        }

        //table = new ObservableCollection<Field>();
        //if (dicCategory.ContainsKey("Общие данные"))
        //{
        //    table = dicCategory["Общие данные"];
        //    dicCategory.Remove("Общие данные");
        //    dicCategory.Add("Общие данные", table);
        //}

            return dicCategory;
    }

    public void Fill(Dictionary<string, ObservableCollection<Field>> record)
    {
        Document doc = new Document(pathPattern);

        foreach (var value in record)
            for (int j = 0; j < value.Value.Count; j++)
            {
                FindReplaceOptions options = new FindReplaceOptions();
                options.MatchCase = false;
                options.FindWholeWordsOnly = false;
                options.Direction = FindReplaceDirection.Forward;
                if (value.Value[j].Category == "Общие данные")
                    doc.Range.Replace("<" + value.Value[j].Name + ">", value.Value[j].Value, options);
                else
                    doc.Range.Replace("<" + value.Value[j].Name + ":" + value.Value[j].Category + ">", value.Value[j].Value, options);
            }

        string name = fileName;

        foreach (var value in record)
            for (int i = 0; i < value.Value.Count; i++)
                name = name.Replace(value.Value[i].Name, value.Value[i].Value);

        int counter = 0;
        string counterStr = "";

        while (File.Exists(Path.Join(pathToFolder, name + counterStr + ".docx")))
        {
            counter++;
            counterStr = " (" + counter + ")";
        }

        doc.Save(Path.Join(pathToFolder, name + counterStr + ".docx"));
        Console.WriteLine($"Документ {pathToFolder}\\{name}{counterStr}.docx создан");
    }

    public void Dispose()
    {
    }
}
