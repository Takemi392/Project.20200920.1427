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

		private DelegateCommand _closeingCommand;
		public DelegateCommand CloseingCommand
		{
			get
			{
				return _closeingCommand ?? (
					_closeingCommand = new DelegateCommand(
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
