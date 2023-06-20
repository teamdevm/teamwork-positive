using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Documently.Models;

namespace Documently.ViewModels;

public class FillViewModel : ViewModelBase
{
    private ITemplateProcessor templateProcessor;
    private ObservableCollection<Field> fields;

    
    public FillViewModel () { }

    

    public FillViewModel (ITemplateProcessor tp, string name)
    {
        templateProcessor = tp;
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
        templateProcessor.Fill(fields);
    }
}
