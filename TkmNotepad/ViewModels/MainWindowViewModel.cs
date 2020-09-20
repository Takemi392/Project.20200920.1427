using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace TkmNotepad.ViewModels
{
  public class MainWindowViewModel : BindableBase, IDropTarget
	{
		#region Field / Property
		private string _title = String.Empty;
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		private string _currentFilePath = String.Empty;
		public string CurrentFilePath
		{
			get { return _currentFilePath; }
			set { SetProperty(ref _currentFilePath, value); }
		}

		private string _currentInputText = String.Empty;
		public string CurrentInputText
		{
			get { return _currentInputText; }
			set { SetProperty(ref _currentInputText, value); }
		}
		#endregion

		#region Constructor
		public MainWindowViewModel()
		{
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
							this.Title = $"{Path.GetFileNameWithoutExtension(this.GetType().Assembly.Location)} Ver.{Assembly.GetExecutingAssembly().GetName().Version}";
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
							// 仮表示
							var r = MessageBox.Show("閉じますか", "確認", MessageBoxButton.YesNo);
						}
					)
				);
			}
		}

		private DelegateCommand<string> _loadFileCommand;
		public DelegateCommand<string> LoadFileCommand
		{
			get
			{
				return _loadFileCommand ?? (
					_loadFileCommand = new DelegateCommand<string>(
						(path) =>
						{
							try
							{
								if (String.IsNullOrEmpty(path) || !File.Exists(path))
									return;

								//
								//@ 既存読み込み分の変更確認
								//

								// ファイル読み込み
								var fullPath = Path.GetFullPath(path);
								using (var stream = new StreamReader(fullPath, true))
								{
									this.CurrentInputText = stream.ReadToEnd();
								}

								this.CurrentFilePath = fullPath;
								this.Title = fullPath;
							}
							catch (Exception e)
							{
								MessageBox.Show(
									$"Message={e.Message}",
									"Error", MessageBoxButton.OK, MessageBoxImage.Error
								);

								Environment.Exit(1);
							}
						},
						(path) =>
						{
							return true;
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
						},
						() =>
						{
							return true;
						}
					)
					.ObservesProperty(() => !String.IsNullOrEmpty(this.CurrentFilePath))
				);
			}
		}
		#endregion

		#region IDropTarget
		public void DragOver(IDropInfo dropInfo)
		{
			var files = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
			dropInfo.Effects = files.Any(name => name.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)) ? DragDropEffects.Copy : DragDropEffects.None;
		}

		public void Drop(IDropInfo dropInfo)
		{
			var files = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>().Where(name => name.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)).ToList();

			if (files.Count == 0)
				return;

			var file = files[0]; // 1ファイルのみ有効
			if (this.LoadFileCommand.CanExecute(file))
				this.LoadFileCommand.Execute(file);
		}
		#endregion

		#region Method
		#endregion
	}
}
