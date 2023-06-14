using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Words;
using Aspose.Words.MailMerging;
using Aspose.Words.Replacing;
using System.Dynamic;

namespace Documently.Models;

class Backend
{
    //хз насколько оптимально это решение, особо не разбирался в вариациях
    public static DataTable CreateTableFromWord(string pathPattern, string namePattern)
    {
        DataTable table = new DataTable(namePattern);
        Document doc;
        try
        {
            doc = new Document(pathPattern); // проверку бы на открытие дока замутить
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }

        foreach (Paragraph p in doc.GetChildNodes(NodeType.Paragraph, true))
        {
            int right = p.ToString(SaveFormat.Text).IndexOf("<"), left = p.ToString(SaveFormat.Text).IndexOf(">");
            string varStr, pStr = p.ToString(SaveFormat.Text);
            while (right >= 0 && left >= 0)
            {
                varStr = pStr.Substring(right, left - right + 1);
                if (!table.Contains(varStr))
                    table.Head.Add(varStr);
                pStr = pStr.Remove(0, left + 1);
                right = pStr.IndexOf("<");
                left = pStr.IndexOf(">");
            }
        }

        return table;
    }

    public static void FillTable(DataTable table)
    {
        string[] filling = new string[table.Head.Count];
        for (int i = 0; i < table.Head.Count; i++)
        {
            Console.WriteLine($"{table.Head[i]} = {i}");
            filling[i] = i.ToString();
        }
        table.Body.Add(filling);
    }

    public static string GetPathToFolder()
    {
        return "C:\\Users\\дом\\Desktop\\ФИТ\\3 Курс\\9 триместр\\Групповая работа\\Тесты";
    }

    public static string GetFileName(DataTable table)
    {
        Console.WriteLine("Имя для формируемых документов: ");
        string copyName;
        string name = copyName = Console.ReadLine();
        for (int i = 0; i < table.Head.Count; i++)
            copyName = copyName.Replace(table.Head[i], "Переменная");

        while (copyName.IndexOf("\\") >= 0 || copyName.IndexOf("/") >= 0 || copyName.IndexOf(":") >= 0 ||
            copyName.IndexOf("*") >= 0 || copyName.IndexOf("?") >= 0 || copyName.IndexOf("|") >= 0 ||
            copyName.IndexOf("<") >= 0 || copyName.IndexOf(">") >= 0 || copyName == "")
        {
            Console.WriteLine("Недопустимый символ в имени файла или пустой ввод.Запрещено использовать такие символы, как \\, /, :, *, ?, |, <, > \n" +
                "(Знаки <> можно использовать в том случае, если они используются для определения переменной в названии файла)\n" +
                "Попробуйте ввести имя еще раз");
            Console.WriteLine("Имя для формируемых документов: ");
            name = copyName = Console.ReadLine();
            for (int i = 0; i < table.Head.Count; i++)
                copyName = copyName.Replace(table.Head[i], "Переменная");
        }
        return name;
    }
    public static void Execute()
    {
        DataTable table;
        string pathToPattern = "C:\\Users\\дом\\Desktop\\ФИТ\\3 Курс\\9 триместр\\Групповая работа\\Шаблоны\\Договоры\\договор аренды квартиры.docx";
        table = CreateTableFromWord(pathToPattern, "Договор аренды квартиры");

        FillTable(table);//заполняем табличку 

        string pathToFolder = GetPathToFolder(); //куда сохраняем, надо будет вызывать какой-нибудь файл-диалог

        string fileName = GetFileName(table);

        Console.WriteLine("Начать создание документов?(Да/Нет)");
        string readyOrNot = Console.ReadLine();
        if (readyOrNot == "Да")
        {
            Document doc;
            for (int i = 0; i < table.Body.Count; i++)
            {
                doc = new Document(pathToPattern);
                for (int j = 0; j < table.Head.Count; j++)
                {
                    FindReplaceOptions options = new FindReplaceOptions();
                    options.MatchCase = false;
                    options.FindWholeWordsOnly = false;
                    options.Direction = FindReplaceDirection.Forward;

                    doc.Range.Replace(table.Head[j], table.Body[i][j], options);
                }
                string name = fileName;
                for (int j = 0; j < table.Head.Count; j++)
                    name = name.Replace(table.Head[j], table.Body[i][j]);
                int counter = 0;
                string counterStr = "";
                while (File.Exists(pathToFolder + "\\" + name + " " + counterStr + ".docx"))
                {
                    counter++;
                    counterStr = "(" + counter + ")";
                }
                //if (counter > 0)
                //    counterStr = counter.ToString();
                doc.Save(pathToFolder + "\\" + name + " " + counterStr + ".docx");
                Console.WriteLine($"Документ {pathToFolder}\\{name} {counterStr}.docx создан");
            }
            Console.WriteLine("Все документы созданы");
        }
    }

    private static Document CreateSourceDocExecuteDataTable()
    {
        Document doc = new Document();
        DocumentBuilder builder = new DocumentBuilder(doc);

        builder.InsertField(" MERGEFIELD CustomerName ");
        builder.InsertParagraph();
        builder.InsertField(" MERGEFIELD Address ");

        return doc;
    }
}
