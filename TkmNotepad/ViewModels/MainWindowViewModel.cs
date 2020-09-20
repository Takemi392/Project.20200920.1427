using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;

namespace TkmNotepad.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		#region Field / Property
		private string _title = "TkmNotepad";
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		private string _filePath = String.Empty;
		public string FilePath
		{
			get { return _filePath; }
			set { SetProperty(ref _filePath, value); }
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
              this.Title = String.Format("{0} Ver.{1}",
                System.IO.Path.GetFileNameWithoutExtension(this.GetType().Assembly.Location),
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
              );

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
							}
							catch (Exception)
							{
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
						}
					)
				);
			}
		}
		#endregion

		#region Method
		#endregion
	}
}
