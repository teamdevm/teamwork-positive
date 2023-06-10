using System.Collections.Generic;
using Aspose.Words.MailMerging;

namespace Documently.Models;

class DataTable : IMailMergeDataSource
{
    public DataTable ()
    {
        Head = new List<string>();
        Body = new List<string[]>();
        TableName = string.Empty;
        RecordId = 0;
    }

    public DataTable (string name)
    {
        Head = new List<string>();
        Body = new List<string[]>();
        TableName = name;
        RecordId = 0;
    }

    private int RecordId;
    public string TableName { get; }
    public List<string> Head { get; }
    public List<string[]> Body { get; }

    public IMailMergeDataSource GetChildDataSource (string tableName)
    {
        throw new System.NotImplementedException();
    }

    public bool GetValue(string fieldName, out object fieldValue)
    {
        if (RecordId >= Body.Count)
        {
            fieldValue = null;
            return false;
        }

        string[] row = Body[RecordId];
        int idx = Head.IndexOf(fieldName);

        if (idx == -1)
        {
            throw new System.ArgumentException("Specified field was not found in the data table");
        }

        if (idx >= row.Length)
        {
            fieldValue = null;
            return false;
        }

        fieldValue = row[idx];
        return true;
    }

    public bool MoveNext()
    {
        if (RecordId >= Body.Count - 1) return false;
        ++RecordId;
        return true;
    }

    public bool Contains (string fieldName)
    {
        return Head.Contains(fieldName);
    }
}