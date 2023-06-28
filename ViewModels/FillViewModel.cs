using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Documently.Models;

namespace Documently.ViewModels;

public class FillViewModel : ViewModelBase
{
    private ITemplateProcessor templateProcessor;
    private Dictionary<string, ObservableCollection<Field>> curFields;
    private Dictionary<string, ObservableCollection<Field>>[] fields;
    public string result;
    public int count;
    public int index;
    MemoryStream mem;
    public FillViewModel () { }
    public FillViewModel (ITemplateProcessor tp, MemoryStream name, int count)
    {
        mem = name;
        this.count = count;
        index = 0;
        templateProcessor = tp;
        fields = new Dictionary<string, ObservableCollection<Field>>[count];
        for (int i = 0; i < count; i++)
        {
            templateProcessor.Setup(name, "", "");
            fields[i] = templateProcessor.GetFields();
        }
        curFields = fields[index];
    }
    public Dictionary<string, ObservableCollection<Field>> CurFields
    {
        get => curFields;
        set => this.RaiseAndSetIfChanged(ref curFields, value);
    }
    public void GetTemplate()
    {
        for (int i = 0; i < count; i++)
        {
            templateProcessor.Setup(mem, 
                Path.GetDirectoryName(result), 
                Path.GetFileNameWithoutExtension(result) + $" ({i+1})" + Path.GetExtension(result));
            Aspose.Words.Document doc = templateProcessor.Fill(fields[i]); // переделал тут, чтобы протестить, потом, как надо сделаете
            templateProcessor.Save(doc, Path.GetExtension(result));
        }
    }
    public void Next()
    {
        if (index != count - 1)
        {
            index++; 
            CurFields = fields[index];
        }
    }
    public void Previous()
    {
        if (index != 0)
        {
            index--;
            CurFields = fields[index];
        }
    }
}
