﻿using System.Windows;

namespace TkmNotepad.Views
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      this.InitializeComponent();
      this.DataContext = new TkmNotepad.ViewModels.MainWindowViewModel(this);
    }
  }
}
