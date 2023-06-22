using System.Collections.ObjectModel;
using ReactiveUI;
using System.IO;
using Avalonia;
namespace Documently.ViewModels;

public class EditViewModel : ViewModelBase
{
    public EditViewModel()
    {
        Application.Current.Resources["LeftAlignMode"] = Application.Current.Resources["LeftAlign"];
        Application.Current.Resources["CenterAlignMode"] = Application.Current.Resources["CenterAlign"];
        Application.Current.Resources["RightAlignMode"] = Application.Current.Resources["RightAlign"];
        Application.Current.Resources["JustifyMode"] = Application.Current.Resources["Justify"];

        Application.Current.Resources["BoldMode"] = Application.Current.Resources["Bold"];
        Application.Current.Resources["ItalicMode"] = Application.Current.Resources["Italic"];
        Application.Current.Resources["UnderlineMode"] = Application.Current.Resources["Underline"];

        Application.Current.Resources["OrderlistMode"] = Application.Current.Resources["Orderlist"];
        Application.Current.Resources["UlitemMode"] = Application.Current.Resources["Ulitem"];

    }

}
