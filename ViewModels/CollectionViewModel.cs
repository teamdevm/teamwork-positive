using System.Collections.ObjectModel;
using ReactiveUI;
using System.IO;
using Documently.Models;
namespace Documently.ViewModels;

public class CollectionViewModel : ViewModelBase
{
    private ObservableCollection<string> nodes;
    private Node selected;
    private string selectedTemplate;
    public ObservableCollection<Node> Items { get; }
    public ObservableCollection<Node> SelectedItems { get; }
    public string strFolder { get; set; }
    public Node Selected 
    {
        get => selected; 
        set
        { 
            selected = value;
            Nodes = new ObservableCollection<string>();

            foreach (string path in Directory.GetFiles(selected.strFullPath))
            {
                Nodes.Add(Path.GetFileName(path));
            }
        } 
    }
    public ObservableCollection<string> Nodes
    {
        get => nodes;
        set => this.RaiseAndSetIfChanged(ref nodes, value);
    }
    public string SelectedTemplate
    {
        get => selectedTemplate;
        set => this.RaiseAndSetIfChanged(ref selectedTemplate, value);
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
    public void FillSelected ()
    {
        FillViewModel fillViewModel = new FillViewModel(new Backend(), SelectedTemplate);
    }
}
