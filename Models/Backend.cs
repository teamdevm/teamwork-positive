using System;
using System.Collections.Generic;
using Aspose.Words;
using Aspose.Words.Replacing;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Documently.Models;

class Backend : ITemplateProcessor
{
    private string pathPattern;
    private string pathToFolder;
    private string fileName;

    public Backend()
    {
        pathPattern = string.Empty;
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

    public bool CheckFileName(ObservableCollection<Field> table)
    {
        string copyName = fileName;

        for (int i = 0; i < table.Count; i++)
            copyName = copyName.Replace(table[i].Name, "Переменная");

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

    public void Setup(string name, string path, string pattern)
    {
        pathPattern = name;
        pathToFolder = path;
        fileName = pattern;
    }

    public ObservableCollection<Field> GetFields()
    {
        ObservableCollection<Field> table = new ObservableCollection<Field>();
        Document doc = new Document(pathPattern);
        Field f;
        foreach (Paragraph p in doc.GetChildNodes(NodeType.Paragraph, true))
        {
            int left = p.ToString(SaveFormat.Text).IndexOf("<"), right = p.ToString(SaveFormat.Text).IndexOf(">");
            string varStr, nameCategory, pStr = p.ToString(SaveFormat.Text);

            while (left >= 0 && right >= 0)
            {
                varStr = pStr.Substring(left + 1, right - left - 1);

                int placeCategory = varStr.IndexOf(":");
                int counter = 0;
                if (placeCategory >= 0)
                    counter = varStr.Count(x => (x == ':'));

                if (counter < 2)
                {
                    if (placeCategory >= 0)
                    {
                        nameCategory = varStr.Substring(placeCategory + 1, varStr.Length - placeCategory - 1);
                        varStr = varStr.Remove(placeCategory, varStr.Length - placeCategory);
                    }
                    else
                        nameCategory = "NoCategory";
                    f = new TextField(varStr, nameCategory);
                    if (!table.Contains(f))
                        table.Add(f);
                }
                else Console.WriteLine("У переменной может быть только одна категория!");

                pStr = pStr.Remove(0, right + 1);
                left = pStr.IndexOf("<");
                right = pStr.IndexOf(">");

            }
        }
        return table;
    }

    public void Fill(ObservableCollection<Field> record)
    {
        Document doc = new Document(pathPattern);

        for (int j = 0; j < record.Count; j++)
        {
            FindReplaceOptions options = new FindReplaceOptions();
            options.MatchCase = false;
            options.FindWholeWordsOnly = false;
            options.Direction = FindReplaceDirection.Forward;

            if (record[j].Category == "NoCategory")
                doc.Range.Replace("<" + record[j].Name + ">", record[j].Value, options);
            else
                doc.Range.Replace("<" + record[j].Name + ":" + record[j].Category + ">", record[j].Value, options);
        }

        string name = fileName;

        for (int j = 0; j < record.Count; j++)
            name = name.Replace(record[j].Name, record[j].Value);

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
