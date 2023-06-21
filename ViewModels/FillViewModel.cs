using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using Documently.Models;

namespace Documently.ViewModels;

public class FillViewModel : ViewModelBase
{
    private ITemplateProcessor templateProcessor;
    private ObservableCollection<Field> fields;
    public string result;
    MemoryStream mem;
    public FillViewModel () { }
    public FillViewModel (ITemplateProcessor tp, MemoryStream name)
    {
        templateProcessor = tp;
        mem = name;
        templateProcessor.Setup(name, "C:/Users/User/Desktop//Interface", "Test");
        fields = templateProcessor.GetFields();
    }

    public ObservableCollection<Field> Fields
    {
        get => fields;
        set => this.RaiseAndSetIfChanged(ref fields, value);
    }
    public void GetTemplate()
    {
        templateProcessor.Setup(mem, Path.GetDirectoryName(result), Path.GetFileName(result));
        templateProcessor.Fill(fields);
    }
}
