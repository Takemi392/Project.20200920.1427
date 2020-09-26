using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;

namespace TkmNotepad.Models
{
  public class WindowClosingBehavior : Behavior<Window>
  {
    protected override void OnAttached()
    {
      base.OnAttached();

      this.AssociatedObject.Closing += Window_Closing;
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();

      this.AssociatedObject.Closing -= Window_Closing;
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      var window = sender as Window;

      if (window.DataContext is IClosing)
        e.Cancel = (window.DataContext as IClosing).OnClosing();
    }
  }
}
