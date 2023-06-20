using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Words;
using Aspose.Words.MailMerging;
using Aspose.Words.Replacing;
using System.Dynamic;
using System.Collections.ObjectModel;
using System.Linq;
using Aspose.Words.Drawing.Ole;
using Aspose.Words.Fields;
using Aspose.Words.Lists;
using System.Drawing;

namespace Documently.Models;

class Backend : ITemplateProcessor
{
    private string pathPattern;
    private string pathToFolder;
    private string fileName;

    public Backend ()
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

    public bool CheckFileName (ObservableCollection<Field> table)
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
        pathToFolder = "D:\\ФИТ 3 курс\\9 трим\\доки";
        fileName = pattern;
        CheckFileName(GetFields());
    }

    public ObservableCollection<Field> GetFields()
    {
        ObservableCollection<Field> table = new ObservableCollection<Field>();
        Document doc = new Document(pathPattern);
        Field f1, f2;
        int countGroups = 0;
        List<FieldsGroup> fieldsGroup = new List<FieldsGroup>();
        foreach (Paragraph p in doc.GetChildNodes(NodeType.Paragraph, true))
        {
            int left = p.ToString(SaveFormat.Text).IndexOf("<"), right = p.ToString(SaveFormat.Text).IndexOf(">");
            string varStr, pStr = p.ToString(SaveFormat.Text);
            while (left >= 0 && right >= 0)
            {
                varStr = pStr.Substring(left+1, right-left-1);
                string res = "", res2 = "", res3 = "";
                char prevChar = char.MinValue, prevCh = char.MinValue; //индексы предыдущих символов
                int ind = 0, index = 0;
                //Разделяем название тега на слова и отделяем категорию (если есть). 
                foreach (char c in varStr)
                {
                    ind++;
                    if (res == "")
                        res += c;
                    //Приводим название тега в нужные регистры, если тег составной
                    //else if (char.IsUpper(c) && char.IsLower(prevChar))
                        //res += " " + char.ToLower(c);
                    //Название категории (группы тега) расположено после двоеточия
                    else if (c == ':')
                    {
                        res2 = varStr.Substring(ind);
                        //Категорию также отделяем на слова, если это потребуется
                        foreach (char ch in res2)
                        {
                            index++;
                            if (res3 == "")
                                res3 += ch;
                           // else if (char.IsUpper(ch) && char.IsLower(prevChar))
                               // res3 += " " + char.ToLower(ch);
                            else
                                res3 += ch;
                            prevCh = ch;
                        }
                        break;
                    }
                    else
                        res += c;
                    prevChar = c;
                }
                if (countGroups == 0)//если еще не былв добавлена ни одна группа в список
                {
                    FieldsGroup group = new FieldsGroup();
                    countGroups++;
                    group.nameGroup = res3;
                    group.nameField.Add(res);
                    fieldsGroup.Add(group);
                }
                else
                {
                    bool bl = false;
                    foreach (FieldsGroup FG in fieldsGroup) //если имя категории уже существует
                    {
                        if (FG.nameGroup == res3)
                        {
                            FG.nameField.Add(res);
                            bl = true;
                            break;
                        }
                    }
                    if (bl == false) //если список не пустой и категории еще нет
                    {
                        FieldsGroup group = new FieldsGroup();
                        countGroups++;
                        group.nameGroup = res3;
                        group.nameField.Add(res);
                        fieldsGroup.Add(group);
                    }
                }

                pStr = pStr.Remove(0, right + 1);
                left = pStr.IndexOf("<");
                right = pStr.IndexOf(">");
            }
        }

        foreach (FieldsGroup fGr in fieldsGroup)
        {
            if (fGr.nameGroup != "")
            {  
                f1 = new Field(fGr.nameGroup);
                if (!table.Contains(f1))
                    table.Add(f1);
            }

            foreach (string s in fGr.nameField)
            {
                f2 = new Field(s);
                if (!table.Contains(f2))
                    table.Add(f2);
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

            doc.Range.Replace(record[j].Name, record[j].Value, options);
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
        Console.WriteLine($"Документ {pathToFolder}\\{name} {counterStr}.docx создан");
    }

    public void Dispose()
    {
    }
}
