using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

namespace wpf_git
{
  public class VideoPlayerViewModel : INotifyPropertyChanged, IDisposable
	{
		public VideoPlayerViewModel()
		{
			_quickLaunchPinImage = QuickLaunchPinImagePinnedPath;
			_dvrLaunchPinImage = DvrPinImagePinnedPath;
			_playIconImage = PlayIconImagePath;

			DvrCtrlVisibility = Visibility.Visible;
			QuickLaunchCtrlVisibility = Visibility.Visible;
			IsDvrCtrlPinned = true;
			IsQuickLaunchCtrlPinned = true;

			_dvrTimer = new System.Timers.Timer(_ctrlVisibilityInterval);
			_dvrTimer.AutoReset = false;
			_dvrTimer.Elapsed += _dvrTimer_Elapsed;

			_quickLaunchTimer = new System.Timers.Timer(_ctrlVisibilityInterval);
			_quickLaunchTimer.AutoReset = false;
			_quickLaunchTimer.Elapsed += _quickLaunchTimer_Elapsed;

			_metadataViewerViewModel = new MetadataViewerViewModel();
		}

		#region Local

		private bool _isDvrCtrlPinned = true;
		private System.Timers.Timer _dvrTimer;
		private readonly object _dvrTimerlock = new object();
		private string _dvrLaunchPinImage = null;
		private const string DvrPinImagePinnedPath = @"~\..\Images\PinIn16.png";
		private const string DvrPinImageUnpinnedPath = @"~\..\Images\PinOut16.png";

		private bool _isQuickLaunchCtrlPinned = true;
		private System.Timers.Timer _quickLaunchTimer;
		private readonly object _quickLaunchTimerlock = new object();
		private string _quickLaunchPinImage = null;
		private const string QuickLaunchPinImagePinnedPath = @"~\..\Images\PinIn16.png";
		private const string QuickLaunchPinImageUnpinnedPath = @"~\..\Images\PinOut16.png";

		private string _playIconImage = null;
		private const string PlayIconImagePath = @"~\..\Images\play_icon.png";

		private double _mouseActionInterval = 250;
		private double _ctrlVisibilityInterval = 250;

		private Visibility _dvrCtrlVisibility = Visibility.Visible;
		private Visibility _quickLaunchCtrlVisibility = Visibility.Visible;

		private MetadataViewerViewModel _metadataViewerViewModel = null;

		#endregion

		#region Commands

		// Dvr Mouse Enter
		private ICommand _dvrCtrlMouseEnterCommand;

		public ICommand DvrCtrlMouseEnterCommand
		{
			get
			{
				if (_dvrCtrlMouseEnterCommand == null)
				{
					_dvrCtrlMouseEnterCommand = new RelayCommand(
							param => DvrCtrlMouseEnter(),
							param => CanDvrCtrlMouseEnter()
					);
				}
				return _dvrCtrlMouseEnterCommand;
			}
		}

		private bool CanDvrCtrlMouseEnter()
		{
			return true;
		}

		// Quick Launch Mouse Leave
		private ICommand _dvrCtrlMouseLeaveCommand;

		public ICommand DvrCtrlMouseLeaveCommand
		{
			get
			{
				if (_dvrCtrlMouseLeaveCommand == null)
				{
					_dvrCtrlMouseLeaveCommand = new RelayCommand(
							param => DvrCtrlMouseLeave(),
							param => CanDvrCtrlMouseLeave()
					);
				}
				return _dvrCtrlMouseLeaveCommand;
			}
		}

		private bool CanDvrCtrlMouseLeave()
		{
			return true;
		}

		// General Ctrls Mouse Enter
		private ICommand _generalCtrlsMouseEnterCommand;

		public ICommand GeneralCtrlsMouseEnterCommand
		{
			get
			{
				if (_generalCtrlsMouseEnterCommand == null)
				{
					_generalCtrlsMouseEnterCommand = new RelayCommand(
							param => GeneralCtrlsMouseEnter(),
							param => CanGeneralCtrlsMouseEnter()
					);
				}
				return _generalCtrlsMouseEnterCommand;
			}
		}

		private bool CanGeneralCtrlsMouseEnter()
		{
			return true;
		}

		// General Ctrls Mouse Leave
		private ICommand _generalCtrlsMouseLeaveCommand;

		public ICommand GeneralCtrlsMouseLeaveCommand
		{
			get
			{
				if (_generalCtrlsMouseLeaveCommand == null)
				{
					_generalCtrlsMouseLeaveCommand = new RelayCommand(
							param => GeneralCtrlsMouseLeave(),
							param => CanGeneralCtrlsMouseLeave()
					);
				}
				return _generalCtrlsMouseLeaveCommand;
			}
		}

		private bool CanGeneralCtrlsMouseLeave()
		{
			return true;
		}

		// Quick Launch Mouse Enter
		private ICommand _quickLaunchCtrlMouseEnterCommand;

		public ICommand QuickLaunchCtrlMouseEnterCommand
		{
			get
			{
				if (_quickLaunchCtrlMouseEnterCommand == null)
				{
					_quickLaunchCtrlMouseEnterCommand = new RelayCommand(
							param => QuickLaunchCtrlMouseEnter(),
							param => CanQuickLaunchCtrlMouseEnter()
					);
				}
				return _quickLaunchCtrlMouseEnterCommand;
			}
		}

		private bool CanQuickLaunchCtrlMouseEnter()
		{
			return true;
		}

		// Quick Launch Mouse Leave
		private ICommand _quickLaunchCtrlMouseLeaveCommand;

		public ICommand QuickLaunchCtrlMouseLeaveCommand
		{
			get
			{
				if (_quickLaunchCtrlMouseLeaveCommand == null)
				{
					_quickLaunchCtrlMouseLeaveCommand = new RelayCommand(
							param => QuickLaunchCtrlMouseLeave(),
							param => CanQuickLaunchCtrlMouseLeave()
					);
				}
				return _quickLaunchCtrlMouseLeaveCommand;
			}
		}

		private bool CanQuickLaunchCtrlMouseLeave()
		{
			return true;
		}

		// Try Open Metadata Viewer
		private ICommand _tryOpenMetadataViewerCommand;

		public ICommand TryOpenMetadataViewerCommand
		{
			get
			{
				if (_tryOpenMetadataViewerCommand == null)
				{
					_tryOpenMetadataViewerCommand = new RelayCommand(
							param => _metadataViewerViewModel.TryOpenMetadataViewer(),
							param => CanTryOpenMetadataViewer()
					);
				}
				return _tryOpenMetadataViewerCommand;
			}
		}

		private bool CanTryOpenMetadataViewer()
		{
			return true;
		}

		// Toggle Dvr 
		private ICommand _toggleDvrCommand;

		public ICommand ToggleDvrCommand
		{
			get
			{
				if (_toggleDvrCommand == null)
				{
					_toggleDvrCommand = new RelayCommand(
							param => ToggleDvrCtrl(),
							param => CanToggleDvr()
					);
				}
				return _toggleDvrCommand;
			}
		}

		private bool CanToggleDvr()
		{
			return true;
		}

		// Toggle Quick Launch
		private ICommand _toggleQuickLaunchCommand;

		public ICommand ToggleQuickLaunchCommand
		{
			get
			{
				if (_toggleQuickLaunchCommand == null)
				{
					_toggleQuickLaunchCommand = new RelayCommand(
							param => ToggleQuickLaunchCtrl(),
							param => CanToggleQuickLaunch()
					);
				}
				return _toggleQuickLaunchCommand;
			}
		}

		private bool CanToggleQuickLaunch()
		{
			return true;
		}

		// Toggle Dvr and Quick Launch
		private ICommand _toggleDvrAndQuickLaunchCommand;

		public ICommand ToggleDvrAndQuickLaunchCommand
		{
			get
			{
				if (_toggleDvrAndQuickLaunchCommand == null)
				{
					_toggleDvrAndQuickLaunchCommand = new RelayCommand(
							param => ToggleDvrAndQuickLaunchCtrls(),
							param => CanToggleDvrAndQuickLaunch()
					);
				}
				return _toggleDvrAndQuickLaunchCommand;
			}
		}

		private bool CanToggleDvrAndQuickLaunch()
		{
			return true;
		}

		// Try Open Dvr
		private ICommand _tryOpenDvrCommand;

		public ICommand TryOpenDvrCommand
		{
			get
			{
				if (_tryOpenDvrCommand == null)
				{
					_tryOpenDvrCommand = new RelayCommand(
							param => TryOpenDvrCtrl(),
							param => CanTryOpenDvr()
					);
				}
				return _tryOpenDvrCommand;
			}
		}

		private bool CanTryOpenDvr()
		{
			return true;
		}

		// Try Open Quick Launch
		private ICommand _tryOpenQuickLaunchCommand;

		public ICommand TryOpenQuickLaunchCommand
		{
			get
			{
				if (_tryOpenQuickLaunchCommand == null)
				{
					_tryOpenQuickLaunchCommand = new RelayCommand(
							param => TryOpenQuickLaunchCtrl(),
							param => CanTryOpenQuickLaunch()
					);
				}
				return _tryOpenQuickLaunchCommand;
			}
		}

		private bool CanTryOpenQuickLaunch()
		{
			return true;
		}

		// Try Open Dvr and Quick Launch
		private ICommand _tryOpenDvrAndQuickLaunchCommand;

		public ICommand TryOpenDvrAndQuickLaunchCommand
		{
			get
			{
				if (_tryOpenDvrAndQuickLaunchCommand == null)
				{
					_tryOpenDvrAndQuickLaunchCommand = new RelayCommand(
							param => TryOpenDvrAndQuickLaunchCtrls(),
							param => CanTryOpenDvrAndQuickLaunch()
					);
				}
				return _tryOpenDvrAndQuickLaunchCommand;
			}
		}

		private bool CanTryOpenDvrAndQuickLaunch()
		{
			return true;
		}

		#endregion

		#region Properties 

		public Visibility DvrCtrlVisibility
		{
			get { return _dvrCtrlVisibility; }
			set
			{
				_dvrCtrlVisibility = value;
				OnPropertyChanged("DVRCtrlVisibility");
			}
		}

		public Visibility QuickLaunchCtrlVisibility
		{
			get { return _quickLaunchCtrlVisibility; }
			set
			{
				_quickLaunchCtrlVisibility = value;
				OnPropertyChanged("QuickLaunchCtrlVisibility");
			}
		}

		public bool IsDvrCtrlPinned
		{
			get { return _isDvrCtrlPinned; }
			set
			{
				_isDvrCtrlPinned = value;
				OnPropertyChanged("IsDvrCtrlPinned");
			}
		}

		public bool IsQuickLaunchCtrlPinned
		{
			get { return _isQuickLaunchCtrlPinned; }
			set
			{
				_isQuickLaunchCtrlPinned = value;
				OnPropertyChanged("IsQuickLaunchCtrlPinned");
			}
		}

		public string DvrPinImage
		{
			get { return _dvrLaunchPinImage; }
			set
			{
				_dvrLaunchPinImage = value;
				OnPropertyChanged("DvrPinImage");
			}
		}

		public string QuickLaunchPinImage
		{
			get { return _quickLaunchPinImage; }
			set
			{
				_quickLaunchPinImage = value;
				OnPropertyChanged("QuickLaunchPinImage");
			}
		}

		public string PlayIconImage
		{
			get { return PlayIconImage; }
			set
			{
				_playIconImage = value;
				OnPropertyChanged("PlayIconImage");
			}
		}

		public MetadataViewerViewModel MetadataViewerViewModel
		{
			get { return _metadataViewerViewModel; }
			set
			{
				_metadataViewerViewModel = value;
				OnPropertyChanged("MetadataViewerViewModel");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion

		#region Methods

		// DVR and Quick Launch
		public void GeneralCtrlsMouseEnter()
		{
			QuickLaunchCtrlMouseEnter();
			DvrCtrlMouseEnter();
		}

		public void GeneralCtrlsMouseLeave()
		{
			QuickLaunchCtrlMouseLeave();
			DvrCtrlMouseLeave();
		}

		private void ToggleDvrAndQuickLaunchCtrls()
		{
			if (DvrCtrlVisibility == Visibility.Collapsed || QuickLaunchCtrlVisibility == Visibility.Collapsed)
			{
				TryOpenDvrAndQuickLaunchCtrls();
			}
			else if (DvrCtrlVisibility == Visibility.Visible || QuickLaunchCtrlVisibility == Visibility.Visible)
			{
				TryCloseDvrAndQuickLaunchCtrls();
			}
		}

		private void TryOpenDvrAndQuickLaunchCtrls()
		{
			TryOpenQuickLaunchCtrl();
			TryOpenDvrCtrl();
		}

		private void TryCloseDvrAndQuickLaunchCtrls()
		{
			TryCloseQuickLaunchCtrl();
			TryCloseDvrCtrl();
		}

		// DVR
		void DvrTimerStop()
		{
			lock (_dvrTimerlock)
			{
				_dvrTimer.Enabled = false; // equivalent to calling Stop()
			}
		}

		public void DvrCtrlMouseEnter()
		{
			if (_dvrTimer.Enabled == true)
			{
				DvrTimerStop();
			}
			if (_quickLaunchTimer.Enabled == true)
			{
				QuickLaunchTimerStop();
			}
		}

		public void DvrCtrlMouseLeave()
		{
			if (IsDvrCtrlPinned == false)
			{
				DvrTimerStop();
				_dvrTimer.Interval = _mouseActionInterval;
				_dvrTimer.Start();
			}

			if (IsQuickLaunchCtrlPinned == false)
			{
				QuickLaunchTimerStop();
				_quickLaunchTimer.Interval = _mouseActionInterval;
				_quickLaunchTimer.Start();
			}
		}

		private void ToggleDvrCtrl()
		{
			if (_isDvrCtrlPinned == false)
			{
				DvrCtrlVisibility = Visibility.Visible;
				_isDvrCtrlPinned = true;
				DvrPinImage = DvrPinImagePinnedPath;
			}
			else
			{
				DvrCtrlVisibility = Visibility.Collapsed;
				_isDvrCtrlPinned = false;
				DvrPinImage = DvrPinImagePinnedPath;
			}
		}

		public void TryCloseDvrCtrl()
		{
			if (DvrCtrlVisibility == Visibility.Visible)
			{
				IsDvrCtrlPinned = false;
				DvrCtrlVisibility = Visibility.Collapsed;
			}
		}

		public void TryOpenDvrCtrl()
		{
			if (DvrCtrlVisibility == Visibility.Collapsed)
			{
				DvrCtrlVisibility = Visibility.Visible;

				_isDvrCtrlPinned = false;
				DvrPinImage = DvrPinImageUnpinnedPath;
				DvrTimerStop();
				_dvrTimer.Interval = _ctrlVisibilityInterval;
				_dvrTimer.Start();
			}
		}

		// Quick Launch Toolbar
		private void QuickLaunchTimerStop()
		{
			lock (_quickLaunchTimerlock)
			{
				_quickLaunchTimer.Enabled = false; // equivalent to calling Stop()
			}
		}

		private void QuickLaunchCtrlMouseEnter()
		{
			if (_quickLaunchTimer.Enabled == true)
			{
				QuickLaunchTimerStop();
			}
			if (_dvrTimer.Enabled == true)
			{
				DvrTimerStop();
			}
		}

		private void QuickLaunchCtrlMouseLeave()
		{
			if (_isQuickLaunchCtrlPinned == false)
			{
				QuickLaunchTimerStop();
				_quickLaunchTimer.Interval = _mouseActionInterval;
				_quickLaunchTimer.Start();
			}
			if (_isDvrCtrlPinned == false)
			{
				DvrTimerStop();
				_dvrTimer.Interval = _mouseActionInterval;
				_dvrTimer.Start();
			}
		}

		private void ToggleQuickLaunchCtrl()
		{
			if (_isQuickLaunchCtrlPinned == false)
			{
				QuickLaunchCtrlVisibility = Visibility.Visible;
				_isQuickLaunchCtrlPinned = true;
				QuickLaunchPinImage = QuickLaunchPinImagePinnedPath;
			}
			else
			{
				QuickLaunchCtrlVisibility = Visibility.Collapsed;
				_isQuickLaunchCtrlPinned = false;
				QuickLaunchPinImage = QuickLaunchPinImageUnpinnedPath;
			}
		}

		public void TryCloseQuickLaunchCtrl()
		{
			if (QuickLaunchCtrlVisibility == Visibility.Visible)
			{
				IsQuickLaunchCtrlPinned = false;
				QuickLaunchCtrlVisibility = Visibility.Collapsed;
			}
		}

		public void TryOpenQuickLaunchCtrl()
		{
			if (QuickLaunchCtrlVisibility == Visibility.Collapsed)
			{
				QuickLaunchCtrlVisibility = Visibility.Visible;
				_isQuickLaunchCtrlPinned = false;
				QuickLaunchPinImage = QuickLaunchPinImageUnpinnedPath;
				_quickLaunchTimer.Stop();
				_quickLaunchTimer.Interval = _ctrlVisibilityInterval;
				_quickLaunchTimer.Start();
			}
		}

		#endregion

		#region Events

		private void _dvrTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			lock (_dvrTimerlock)
			{
				if (_isDvrCtrlPinned == false)
				{
					DvrCtrlVisibility = Visibility.Collapsed;
					DvrPinImage = DvrPinImageUnpinnedPath;
				}
				else
				{
					DvrCtrlVisibility = Visibility.Visible;
					DvrPinImage = DvrPinImagePinnedPath;
				}

				if (_dvrTimer.Enabled == false)
				{
					return;
				}
			}
		}

		private void _quickLaunchTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			lock (_quickLaunchTimerlock)
			{
				if (_isQuickLaunchCtrlPinned == false)
				{
					QuickLaunchCtrlVisibility = Visibility.Collapsed;
					QuickLaunchPinImage = QuickLaunchPinImageUnpinnedPath;
				}
				else
				{
					QuickLaunchCtrlVisibility = Visibility.Visible;
					QuickLaunchPinImage = QuickLaunchPinImagePinnedPath;
				}

				if (_quickLaunchTimer.Enabled == false)
				{
					return;
				}
			}
		}

		#endregion

		#region IDisposable

		// TODO: Employ Weak Event Patterns in .NET 4.5
		// http://msdn.microsoft.com/en-us/library/aa970850(v=vs.110).aspx

		// Flag: Has Dispose already been called? 
		bool disposed = false;

		// Public implementation of Dispose pattern callable by consumers. 
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Protected implementation of Dispose pattern. 
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				// Free any other managed objects here.
				_dvrTimer.Stop();
				_dvrTimer.Elapsed -= _dvrTimer_Elapsed;
				_dvrTimer = null;

				_quickLaunchTimer.Stop();
				_quickLaunchTimer.Elapsed -= _quickLaunchTimer_Elapsed;
				_quickLaunchTimer = null;

				TryDispose(_metadataViewerViewModel);
			}

			// Free any unmanaged objects here. 
			disposed = true;
		}

		~VideoPlayerViewModel()
		{
			this.Dispose(false);
		}

		private static void TryDispose(object obj)
		{
			var disposableObj = obj as IDisposable;
			if (disposableObj != null)
				disposableObj.Dispose();
		}

		#endregion
	}
}
