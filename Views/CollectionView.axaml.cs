using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Documently.ViewModels;

namespace Documently.Views
{
    public partial class CollectionView : UserControl
    {
        public CollectionView()
        {
            InitializeComponent();
            TreeView t = this.FindControl<TreeView>("tree");
            t.DoubleTapped += (s, e) => t.ExpandSubTree(FindItem(e.Source));
        }
        private static TreeViewItem FindItem (IInteractive obj)
        {
            while (obj is not TreeViewItem)
            {
                obj = obj.InteractiveParent;
            }
            return obj as TreeViewItem;
        }
    }
}
