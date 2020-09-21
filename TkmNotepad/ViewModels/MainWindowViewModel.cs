using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
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

		private FileInfoViewModel _currentFileInfoViewModel = new FileInfoViewModel();
		public FileInfoViewModel CurrentFileInfoViewModel
		{
			get { return _currentFileInfoViewModel; }
			set { SetProperty(ref _currentFileInfoViewModel, value); }
		}

		public Window Owner { get; } = null;
		#endregion

		#region Constructor
		public MainWindowViewModel(Window owner)
		{
			this.Owner = owner;
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
			try
			{
				var files = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>().Where(name => name.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)).ToList();

				if (files.Count == 0)
					return;

				// 変更確認
				if (this.CurrentFileInfoViewModel.IsTextChanged)
				{
					var r = MessageBox.Show(
						this.Owner,
						$"{this.CurrentFileInfoViewModel.FilePath}への変更内容を保存しますか？",
						"確認",
						MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes
					);

					switch (r)
					{
						case MessageBoxResult.Yes:
							{
								// 新規作成
								if (String.IsNullOrEmpty(this.CurrentFileInfoViewModel.FilePath))
								{
									//
									//@ ダイアログ
									//
									//if (this.CurrentFileInfoViewModel.SaveCommand.CanExecute())
									//	this.CurrentFileInfoViewModel.SaveCommand.Execute();
								}
								// 上書き
								else
								{
									if (this.CurrentFileInfoViewModel.OverwriteCommand.CanExecute())
										this.CurrentFileInfoViewModel.OverwriteCommand.Execute();
								}

								break;
							}

						case MessageBoxResult.No:
							{
								// 無視
								break;
							}

						default:
							{
								// キャンセル
								return;
							}
					}
				}

				// 読み込み
				var file = files[0];
				if (this.CurrentFileInfoViewModel.LoadCommand.CanExecute(file))
					this.CurrentFileInfoViewModel.LoadCommand.Execute(file);
			}
			catch (Exception e)
			{
				MessageBox.Show(
					$"Message={e.Message}",
					"Error", MessageBoxButton.OK, MessageBoxImage.Error
				);

				Environment.Exit(1);
			}
		}
		#endregion

		#region Method
		#endregion
	}
}
