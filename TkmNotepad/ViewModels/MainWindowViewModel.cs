using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace TkmNotepad.ViewModels
{
  public class MainWindowViewModel : BindableBase
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

							// 暫定読み込み
							this.LoadFileCommand.Execute(@"C:\Temp\DevTest.txt");
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

								using (var stream = new StreamReader(path, true))
								{
									this.CurrentInputText = stream.ReadToEnd();
								}

								this.CurrentFilePath = Path.GetFullPath(path);
							}
							catch (Exception e)
							{
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

		#region Method
		#endregion
	}
}
