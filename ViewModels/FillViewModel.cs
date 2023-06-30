using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Documently.Models;
using Aspose.Words;

namespace Documently.ViewModels;

public class FillViewModel : ViewModelBase
{
    private ITemplateProcessor templateProcessor;
    private Dictionary<string, ObservableCollection<Field>> curFields;
    private Dictionary<string, ObservableCollection<Field>>[] fields;
    private bool _isFirst;
    private bool _isLast;
    private Document _doc;
    public string result;
    private int count;
    private int index;
    MemoryStream mem;
    public FillViewModel () { }
    public FillViewModel (ITemplateProcessor tp, MemoryStream name, int count)
    {
        mem = name;
        this.count = count;
        index = 1;
        templateProcessor = tp;
        Doc = templateProcessor.Setup(name, "", "");
        fields = new Dictionary<string, ObservableCollection<Field>>[count];
        for (int i = 0; i < count; i++)
        {
            fields[i] = templateProcessor.GetFields();
        }
        curFields = fields[index - 1];
        _isFirst = index == 1;
        _isLast = index == count;
        UpdatePreview = ReactiveCommand.Create(_UpdatePreview);
    }
    public Dictionary<string, ObservableCollection<Field>> CurFields
    {
        get => curFields;
        set => this.RaiseAndSetIfChanged(ref curFields, value);
    }
    public Document Doc
    {
        get => _doc;
        set => this.RaiseAndSetIfChanged(ref _doc, value);
    }
    public ReactiveCommand<Unit, Unit> UpdatePreview { get; }
    private void _UpdatePreview ()
    {
        Doc = templateProcessor.Fill(CurFields);
    }
    public void GetTemplate()
    {
        for (int i = 0; i < count; i++)
        {
            templateProcessor.Setup(mem, 
                Path.GetDirectoryName(result), 
                Path.GetFileNameWithoutExtension(result) + $" ({i+1})" + Path.GetExtension(result));
            Document doc = templateProcessor.Fill(fields[i]); // переделал тут, чтобы протестить, потом, как надо сделаете
            templateProcessor.Save(doc, Path.GetExtension(result));
        }
    }
    public void Next()
    {
        if (!IsLast)
        {
            CurFields = fields[++Index - 1];
            IsFirst = index == 1;
            IsLast = index == count;
        }
    }
    public void Previous()
    {
        if (!IsFirst)
        {
            CurFields = fields[--Index - 1];
            IsFirst = index == 1;
            IsLast = index == count;
        }
    }
    public bool IsFirst
    {
        get => _isFirst;
        set => this.RaiseAndSetIfChanged(ref _isFirst, value);
    }
    public bool IsLast
    {
        get => _isLast;
        set => this.RaiseAndSetIfChanged(ref _isLast, value);
    }
    public int Index
    {
        get => index;
        set => this.RaiseAndSetIfChanged(ref index, value);
    }
}
