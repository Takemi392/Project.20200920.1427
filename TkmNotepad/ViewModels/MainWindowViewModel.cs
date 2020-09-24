﻿using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using TkmNotepad.Models;

namespace TkmNotepad.ViewModels
{
  public class MainWindowViewModel : BindableBase, GongSolutions.Wpf.DragDrop.IDropTarget
  {
    #region Field / Property
    private string _title = String.Empty;
    public string Title
    {
      get { return _title; }
      set { SetProperty(ref _title, value); }
    }

    private Brush _background;
    public Brush Background
    {
      get { return _background; }
      set { SetProperty(ref _background, value); }
    }

    private Brush _fontColor;
    public Brush FontColor
    {
      get { return _fontColor; }
      set { SetProperty(ref _fontColor, value); }
    }

    private int _fontSize;
    public int FontSize
    {
      get { return _fontSize; }
      set { SetProperty(ref _fontSize, value); }
    }

    private FileInfoViewModel _currentFileInfoViewModel = new FileInfoViewModel();
    public FileInfoViewModel CurrentFileInfoViewModel
    {
      get { return _currentFileInfoViewModel; }
      set { SetProperty(ref _currentFileInfoViewModel, value); }
    }

    private Window WindowObject { get; } = null;
    private DesignSettingsYamlObject DesignSettingsYamlObject { get; set; } = null;
    #endregion

    #region Constructor
    public MainWindowViewModel(Window window)
    {
      this.WindowObject = window;
    }
    #endregion

    #region Command
    private DelegateCommand _loadedCommand;
    public DelegateCommand LoadedCommand
    {
      get
      {
        return _loadedCommand ?? (
          _loadedCommand = new DelegateCommand(
            () =>
            {
              try
              {
                this.Title = String.Format(
                  "{0} Ver.{1}",
                  System.IO.Path.GetFileNameWithoutExtension(this.GetType().Assembly.Location),
                  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
                );

                if (this.LoadDesignSettingsCommand.CanExecute())
                  this.LoadDesignSettingsCommand.Execute();
              }
              catch (Exception e)
              {
                var msg = String.Format(
                  "Failed to loaded command, Exception={0}, InnerException={1}",
                  e.Message, e.InnerException?.Message ?? "null"
                );

                System.Windows.MessageBox.Show(this.WindowObject, msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
              }
            }
          )
        );
      }
    }

    private DelegateCommand _closedCommand;
    public DelegateCommand ClosedCommand
    {
      get
      {
        return _closedCommand ?? (
          _closedCommand = new DelegateCommand(
            () =>
            {
              try
              {
              }
              catch (Exception e)
              {
                var msg = String.Format(
                  "Failed to closed command, Exception={0}, InnerException={1}",
                  e.Message, e.InnerException?.Message ?? "null"
                );

                System.Windows.MessageBox.Show(this.WindowObject, msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
              }
            }
          )
        );
      }
    }

    private DelegateCommand _closingCommand;
    public DelegateCommand ClosingCommand
    {
      get
      {
        return _closingCommand ?? (
          _closingCommand = new DelegateCommand(
            () =>
            {
              try
              {
              }
              catch (Exception e)
              {
                var msg = String.Format(
                  "Failed to closing command, Exception={0}, InnerException={1}",
                  e.Message, e.InnerException?.Message ?? "null"
                );

                System.Windows.MessageBox.Show(this.WindowObject, msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
              }
            }
          )
        );
      }
    }

    private DelegateCommand _createNewFileCommand;
    public DelegateCommand CreateNewFileCommand
    {
      get
      {
        return _createNewFileCommand ?? (
          _createNewFileCommand = new DelegateCommand(
            () =>
            {
              try
              {
                if (!this.ExistingTextEndProcess())
                  return;

                if (this.CurrentFileInfoViewModel.ClearCommand.CanExecute())
                  this.CurrentFileInfoViewModel.ClearCommand.Execute();
              }
              catch (Exception e)
              {
                var msg = String.Format(
                  "Failed to create new file command, Exception={0}, InnerException={1}",
                  e.Message, e.InnerException?.Message ?? "null"
                );

                System.Windows.MessageBox.Show(this.WindowObject, msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
              }
            }
          )
        );
      }
    }

    private DelegateCommand _createNewFileWithWindowCommand;
    public DelegateCommand CreateNewFileWithWindowCommand
    {
      get
      {
        return _createNewFileWithWindowCommand ?? (
          _createNewFileWithWindowCommand = new DelegateCommand(
            () =>
            {
              try
              {
                // 新しいウインドウで開く
              }
              catch (Exception e)
              {
                var msg = String.Format(
                  "Failed to create new file with window command, Exception={0}, InnerException={1}",
                  e.Message, e.InnerException?.Message ?? "null"
                );

                System.Windows.MessageBox.Show(this.WindowObject, msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
              }
            }
          )
        );
      }
    }

    private DelegateCommand _openFileCommand;
    public DelegateCommand OpenFileCommand
    {
      get
      {
        return _openFileCommand ?? (
          _openFileCommand = new DelegateCommand(
            () =>
            {
              try
              {
                if (!this.ExistingTextEndProcess())
                  return;

                var dialog = new OpenFileDialog()
                {
                  Filter = "テキストファイル (*.txt)|*.txt|全てのファイル (*.*)|*.*",
                  FileName = String.Empty,
                };

                if (dialog.ShowDialog() == true)
                {
                  var path = dialog.FileName;
                  if (System.IO.File.Exists(path))
                  {
                    if (this.CurrentFileInfoViewModel.LoadCommand.CanExecute(path))
                      this.CurrentFileInfoViewModel.LoadCommand.Execute(path);
                  }
                }
              }
              catch (Exception e)
              {
                var msg = String.Format(
                  "Failed to open file command, Exception={0}, InnerException={1}",
                  e.Message, e.InnerException?.Message ?? "null"
                );

                System.Windows.MessageBox.Show(this.WindowObject, msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
              }
            }
          )
        );
      }
    }

    private DelegateCommand _overwriteFileCommand;
    public DelegateCommand OverwriteFileCommand
    {
      get
      {
        return _overwriteFileCommand ?? (
          _overwriteFileCommand = new DelegateCommand(
            () =>
            {
              try
              {
                // 新規作成
                if (String.IsNullOrEmpty(this.CurrentFileInfoViewModel.FilePath))
                {
                  if (this.SaveFileCommand.CanExecute())
                    this.SaveFileCommand.Execute();
                }
                // 上書き
                else
                {
                  if (this.CurrentFileInfoViewModel.OverwriteCommand.CanExecute())
                    this.CurrentFileInfoViewModel.OverwriteCommand.Execute();
                }
              }
              catch (Exception e)
              {
                var msg = String.Format(
                  "Failed to overwritefile command, Exception={0}, InnerException={1}",
                  e.Message, e.InnerException?.Message ?? "null"
                );

                System.Windows.MessageBox.Show(this.WindowObject, msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
              }
            }
          )
        );
      }
    }

    private DelegateCommand _saveFileCommand;
    public DelegateCommand SaveFileCommand
    {
      get
      {
        return _saveFileCommand ?? (
          _saveFileCommand = new DelegateCommand(
            () =>
            {
              try
              {
                var dialog = new SaveFileDialog()
                {
                  Filter = "テキストファイル (*.txt)|*.txt|全てのファイル (*.*)|*.*",
                  FileName = String.Empty,
                };

                if (dialog.ShowDialog() == true)
                {
                  var path = dialog.FileName;
                  if (this.CurrentFileInfoViewModel.SaveCommand.CanExecute(path))
                    this.CurrentFileInfoViewModel.SaveCommand.Execute(path);
                }
              }
              catch (Exception e)
              {
                var msg = String.Format(
                  "Failed to save file command, Exception={0}, InnerException={1}",
                  e.Message, e.InnerException?.Message ?? "null"
                );

                System.Windows.MessageBox.Show(this.WindowObject, msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
              }
            }
          )
        );
      }
    }

    private DelegateCommand _applicationEndCommand;
    public DelegateCommand ApplicationEndCommand
    {
      get
      {
        return _applicationEndCommand ?? (
          _applicationEndCommand = new DelegateCommand(
            () =>
            {
              this.WindowObject.Close();
            }
          )
        );
      }
    }

    private DelegateCommand _loadDesignSettingsCommand;
    public DelegateCommand LoadDesignSettingsCommand
    {
      get
      {
        return _loadDesignSettingsCommand ?? (
          _loadDesignSettingsCommand = new DelegateCommand(
            () =>
            {
              try
              {
                var filePath = this.GetDesignSettingsFilePath();
                using (var stream = new System.IO.StreamReader(filePath, Encoding.UTF8))
                {
                  var deserializer = new YamlDotNet.Serialization.DeserializerBuilder().Build();
                  this.DesignSettingsYamlObject = deserializer.Deserialize<DesignSettingsYamlObject>(stream.ReadToEnd());
                }

                if (this.DesignSettingsYamlObject == null)
                  throw new Exception("DesignSettingsYamlObject == null");

                this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(this.DesignSettingsYamlObject.Background));
                this.FontColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(this.DesignSettingsYamlObject.FontColor));
                this.FontSize = this.DesignSettingsYamlObject.FontSize;
              }
              catch (Exception e)
              {
                var msg = String.Format(
                  "Failed to load design settings command, Exception={0}, InnerException={1}",
                  e.Message, e.InnerException?.Message ?? "null"
                );

                System.Windows.MessageBox.Show(this.WindowObject, msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
              }
            }
          )
        );
      }
    }
    #endregion

    #region IDropTarget
    public void DragOver(GongSolutions.Wpf.DragDrop.IDropInfo dropInfo)
    {
      var files = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
      dropInfo.Effects = files.Any(name => System.IO.File.Exists(name)) ? DragDropEffects.Copy : DragDropEffects.None;
    }

    public void Drop(GongSolutions.Wpf.DragDrop.IDropInfo dropInfo)
    {
      try
      {
        var files = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>().Where(name => System.IO.File.Exists(name)).ToList();
        if (files.Count == 0)
          return;

        if (!this.ExistingTextEndProcess())
          return;

        var file = files[0];
        if (this.CurrentFileInfoViewModel.LoadCommand.CanExecute(file))
          this.CurrentFileInfoViewModel.LoadCommand.Execute(file);
      }
      catch (Exception e)
      {
        var msg = String.Format(
          "Failed to load file, Exception={0}, InnerException={1}",
          e.Message, e.InnerException?.Message ?? "null"
        );

        System.Windows.MessageBox.Show(this.WindowObject, msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        System.Environment.Exit(1);
      }
    }
    #endregion

    #region Method
    private string GetDesignSettingsFilePath()
    {
      var directoryPath = System.IO.Path.Combine(
        System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
        "Settings"
      );

      var filePath = System.IO.Path.Combine(
        directoryPath,
        "DesignSettings.yaml"
      );

      System.IO.Directory.CreateDirectory(directoryPath);
      if (!System.IO.File.Exists(filePath))
      {
        var serializer = new YamlDotNet.Serialization.SerializerBuilder().Build();
        var yamlString = serializer.Serialize(new DesignSettingsYamlObject());

        using (var stream = new StreamWriter(filePath, false, Encoding.UTF8))
        {
          stream.Write(yamlString);
        }
      }

      return filePath;
    }

    private bool ExistingTextEndProcess()
    {
      if (this.CurrentFileInfoViewModel.IsTextChanged)
      {
        var r = System.Windows.MessageBox.Show(
          this.WindowObject,
          $"{this.CurrentFileInfoViewModel.FilePath}への変更内容を保存しますか？",
          "確認",
          MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes
        );

        switch (r)
        {
          // 保存
          case MessageBoxResult.Yes:
            {
              // 新規作成
              if (String.IsNullOrEmpty(this.CurrentFileInfoViewModel.FilePath))
              {
                var dialog = new SaveFileDialog()
                {
                  Filter = "テキストファイル (*.txt)|*.txt|全てのファイル (*.*)|*.*",
                  FileName = String.Empty,
                };

                if (dialog.ShowDialog() == false)
                  return false;

                var path = dialog.FileName;
                if (this.CurrentFileInfoViewModel.SaveCommand.CanExecute(path))
                  this.CurrentFileInfoViewModel.SaveCommand.Execute(path);

                return true;
              }
              // 上書き
              else
              {
                if (this.CurrentFileInfoViewModel.OverwriteCommand.CanExecute())
                  this.CurrentFileInfoViewModel.OverwriteCommand.Execute();

                return true;
              }
            }

          // 無視
          case MessageBoxResult.No:
            {
              return true;
            }

          // キャンセル
          case MessageBoxResult.Cancel:
          default:
            {
              return false;
            }
        }
      }

      return true;
    }
    #endregion
  }
}
