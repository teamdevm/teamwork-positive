using Avalonia.Controls;
using Documently.ViewModels;

namespace Documently.Views
{
    public partial class FillView : UserControl
    {
        public FillView()
        {
            InitializeComponent();
            DataContextChanged += Handle;
        }
        private void Handle (object? sender, System.EventArgs args)
        {
            if (DataContext is FillViewModel m)
            {
                var key = new Avalonia.Input.KeyBinding();
                key.Command = m.UpdatePreview;
                key.Gesture = new Avalonia.Input.KeyGesture(Avalonia.Input.Key.Enter);
                KeyBindings.Add(key);
            }
        }
    }
}
