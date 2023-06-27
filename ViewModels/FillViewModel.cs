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
    private ITemplateProcessor curTemplate;
    private Dictionary<string, ObservableCollection<Field>> curFields;

    private ITemplateProcessor[] templateProcessor;
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
        templateProcessor = new ITemplateProcessor[count];
        fields = new Dictionary<string, ObservableCollection<Field>>[count];
        for (int i = 0; i < count; i++)
        {
            templateProcessor[i] = tp;
            templateProcessor[i].Setup(name, "", "");
            fields[i] = templateProcessor[i].GetFields();
        }
        curTemplate = templateProcessor[index];
        curFields = fields[index];
    }
    public ITemplateProcessor CurTemplate
    {
        get => curTemplate;
        set => this.RaiseAndSetIfChanged(ref curTemplate, value);
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
            templateProcessor[i].Setup(mem, 
                Path.GetDirectoryName(result), 
                Path.GetFileNameWithoutExtension(result) + $" ({i+1})" + Path.GetExtension(result));
            string[] ext1 = result.Split('.');
            string ext2 = ext1[ext1.Length - 1];
            templateProcessor[i].Fill(fields[i], ext2);
        }
    }
    public void Next()
    {
        if (index != count - 1)
        {
            index++; 
            CurFields = fields[index];
            CurTemplate = templateProcessor[index];
        }
    }
    public void Previous()
    {
        if (index != 0)
        {
            index--;
            CurFields = fields[index];
            CurTemplate = templateProcessor[index];
        }
    }
}
