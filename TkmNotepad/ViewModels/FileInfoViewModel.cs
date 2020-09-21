using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.IO;

namespace TkmNotepad.ViewModels
{
  public class FileInfoViewModel : BindableBase
	{
		#region Field / Property
		private string _currentPath = String.Empty;
		public string CurrentPath
		{
			get { return _currentPath; }
			set { SetProperty(ref _currentPath, value); }
		}

		private string _currentText = String.Empty;
		public string CurrentText
		{
			get { return _currentText; }
			set { SetProperty(ref _currentText, value); }
		}

		private string _lastSavedText = String.Empty;
		public string LastSavedText
		{
			get { return _lastSavedText; }
			set { SetProperty(ref _lastSavedText, value); }
		}
		#endregion

		#region Constructor
		public FileInfoViewModel()
		{
		}
		#endregion

		#region Command
		private DelegateCommand<string> _loadCommand;
		public DelegateCommand<string> LoadCommand
		{
			get
			{
				return _loadCommand ?? (
					_loadCommand = new DelegateCommand<string>(
						(path) =>
						{
              var fullPath = Path.GetFullPath(path);
              using (var stream = new StreamReader(fullPath, true))
              {
								var data = stream.ReadToEnd();

								this.LastSavedText = data;
								this.CurrentText = data;
							}

              this.CurrentPath = fullPath;
						}
					)
				);
			}
		}

		private DelegateCommand<string> _saveCommand;
		public DelegateCommand<string> SaveCommand
		{
			get
			{
				return _saveCommand ?? (
					_saveCommand = new DelegateCommand<string>(
						(path) =>
						{
							var fullPath = Path.GetFullPath(path);

							//
							//@ 書き込み, 安全のため未実装
							//
							Debug.WriteLine("新規");
							this.LastSavedText = this.CurrentText;

							this.CurrentPath = fullPath;
						}
					)
				);
			}
		}

		private DelegateCommand _overwriteCommand;
		public DelegateCommand OverwriteCommand
		{
			get
			{
				return _overwriteCommand ?? (
					_overwriteCommand = new DelegateCommand(
						() =>
						{
							//
							//@ 書き込み, 安全のため未実装
							//
							Debug.WriteLine("上書き");
							this.LastSavedText = this.CurrentText;
						},
						() =>
						{
							return !String.IsNullOrEmpty(this.CurrentPath);
						}
					)
					.ObservesProperty(() => this.CurrentPath)
				);
			}
		}
		#endregion

		#region Method
		public bool IsTextChanged()
		{
			return this.CurrentText != this.LastSavedText;
		}
		#endregion
	}
}
