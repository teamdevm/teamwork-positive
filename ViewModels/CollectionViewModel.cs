using System.Collections.ObjectModel;
using ReactiveUI;
using System.IO;
namespace Documently.ViewModels;

public class CollectionViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";
    private Node selected;
    public ObservableCollection<Node> Items { get; }
    public ObservableCollection<Node> SelectedItems { get; }
    public string strFolder { get; set; }
    public Node Selected 
    {
        get => selected; 
        set
        { 
            selected = value;
            Nodes = new ObservableCollection<string>(Directory.GetFiles(selected.strFullPath));
        } 
    }
    public ObservableCollection<string> nodes; 
    public ObservableCollection<string> Nodes
    {
        get => nodes;
        set => this.RaiseAndSetIfChanged(ref nodes, value);
    }
    public CollectionViewModel()
    {
        strFolder = "Documents"; // EDIT THIS FOR AN EXISTING FOLDER

        Items = new ObservableCollection<Node>();

        Node rootNode = new Node(strFolder);
        rootNode.Subfolders = GetSubfolders(strFolder);

        Items.Add(rootNode);
    }

    public ObservableCollection<Node> GetSubfolders(string strPath)
    {
        ObservableCollection<Node> subfolders = new ObservableCollection<Node>();
        string[] subdirs = Directory.GetDirectories(strPath, "*", SearchOption.TopDirectoryOnly);

        foreach (string dir in subdirs)
        {
            Node thisnode = new Node(dir);

            if (Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly).Length > 0)
            {
                thisnode.Subfolders = new ObservableCollection<Node>();

                thisnode.Subfolders = GetSubfolders(dir);
            }

            subfolders.Add(thisnode);
        }

        return subfolders;
    }

    public class Node
    {
        public ObservableCollection<Node> Subfolders { get; set; }

        public string strNodeText { get; }
        public string strFullPath { get; }

        public Node(string _strFullPath)
        {
            strFullPath = _strFullPath;
            strNodeText = Path.GetFileName(_strFullPath);
        }
    }

}
