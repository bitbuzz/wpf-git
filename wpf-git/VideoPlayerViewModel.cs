using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics.Tracing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media.Animation;

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

		#region Private

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

		private bool isDvrRecentlyUnpinned = false;
		private bool isQuickLaunchRecentlyUnpinned = false;
		private double _mouseActionInterval = 250;
		private double _ctrlVisibilityInterval = 250;
		private Visibility _dvrCtrlVisibility = Visibility.Visible;
		private Visibility _quickLaunchCtrlVisibility = Visibility.Visible;
		private ICommand _dvrCtrlMouseEnterCmd;
		private ICommand _dvrCtrlMouseLeaveCmd;
		private ICommand _generalCtrlsMouseEnterCmd;
		private ICommand _generalCtrlsMouseLeaveCmd;
		private ICommand _quickLaunchCtrlMouseEnterCmd;
		private ICommand _quickLaunchCtrlMouseLeaveCmd;
		private ICommand _tryOpenMetadataViewerCmd;
		private ICommand _toggleDvrCmd;
		private ICommand _toggleQuickLaunchCmd;
		private ICommand _toggleDvrAndQuickLaunchCmd;
		private ICommand _tryOpenDvrCmd;
		private ICommand _tryOpenQuickLaunchCmd;
		private ICommand _tryCloseDvrAndQuickLaunchCtrlsCmd;
		private ICommand _tryOpenDvrAndQuickLaunchCmd;
		private ICommand _videoCtrlMouseEnterCmd;
		private ICommand _videoCtrlMouseLeaveCmd;
		private MetadataViewerViewModel _metadataViewerViewModel = null;

		#endregion

		#region Commands

		// DVR
		public ICommand DvrCtrlMouseEnterCmd
		{
			get
			{
				if (_dvrCtrlMouseEnterCmd == null)
				{
					_dvrCtrlMouseEnterCmd = new RelayCommand(
							param => DvrCtrlMouseEnter(),
							(param) => true
					);
				}
				return _dvrCtrlMouseEnterCmd;
			}
		}

		public ICommand DvrCtrlMouseLeaveCmd
		{
			get
			{
				if (_dvrCtrlMouseLeaveCmd == null)
				{
					_dvrCtrlMouseLeaveCmd = new RelayCommand(
							param => DvrCtrlMouseLeave(),
							(param) => true
					);
				}
				return _dvrCtrlMouseLeaveCmd;
			}
		}

		public ICommand ToggleDvrCmd
		{
			get
			{
				if (_toggleDvrCmd == null)
				{
					_toggleDvrCmd = new RelayCommand(
							param => ToggleDvrCtrl(),
							(param) => true
					);
				}
				return _toggleDvrCmd;
			}
		}

		public ICommand OpenDvrCmd
		{
			get
			{
				if (_tryOpenDvrCmd == null)
				{
					_tryOpenDvrCmd = new RelayCommand(
							param => TryOpenDvrCtrl(),
							(param) => true
					);
				}
				return _tryOpenDvrCmd;
			}
		}

		// Quick Launch
		public ICommand QuickLaunchCtrlMouseEnterCmd
		{
			get
			{
				if (_quickLaunchCtrlMouseEnterCmd == null)
				{
					_quickLaunchCtrlMouseEnterCmd = new RelayCommand(
							param => QuickLaunchCtrlMouseEnter(),
							(param) => true
					);
				}
				return _quickLaunchCtrlMouseEnterCmd;
			}
		}

		public ICommand QuickLaunchCtrlMouseLeaveCmd
		{
			get
			{
				if (_quickLaunchCtrlMouseLeaveCmd == null)
				{
					_quickLaunchCtrlMouseLeaveCmd = new RelayCommand(
							param => QuickLaunchCtrlMouseLeave(),
							(param) => true
					);
				}
				return _quickLaunchCtrlMouseLeaveCmd;
			}
		}

		public ICommand ToggleQuickLaunchCmd
		{
			get
			{
				if (_toggleQuickLaunchCmd == null)
				{
					_toggleQuickLaunchCmd = new RelayCommand(
							param => ToggleQuickLaunchCtrl(),
							(param) => true
					);
				}
				return _toggleQuickLaunchCmd;
			}
		}

		public ICommand OpenQuickLaunchCmd
		{
			get
			{
				if (_tryOpenQuickLaunchCmd == null)
				{
					_tryOpenQuickLaunchCmd = new RelayCommand(
							param => TryOpenQuickLaunchCtrl(),
							(param) => true
					);
				}
				return _tryOpenQuickLaunchCmd;
			}
		}
		
		// Dvr and Quick Launch
		public ICommand ToggleDvrAndQuickLaunchCmd
		{
			get
			{
				if (_toggleDvrAndQuickLaunchCmd == null)
				{
					_toggleDvrAndQuickLaunchCmd = new RelayCommand(
							param => ToggleDvrAndQuickLaunchCtrls(),
							(param) => true
					);
				}
				return _toggleDvrAndQuickLaunchCmd;
			}
		}

		public ICommand TryCloseDvrAndQuickLaunchCtrlsCmd
		{
			get
			{
				if (_tryCloseDvrAndQuickLaunchCtrlsCmd == null)
				{
					_tryCloseDvrAndQuickLaunchCtrlsCmd = new RelayCommand(
							param => TryCloseDvrAndQuickLaunchCtrls(),
							(param) => true
					);
				}
				return _tryCloseDvrAndQuickLaunchCtrlsCmd;
			}
		}

		public ICommand TryOpenDvrAndQuickLaunchCmd
		{
			get
			{
				if (_tryOpenDvrAndQuickLaunchCmd == null)
				{
					_tryOpenDvrAndQuickLaunchCmd = new RelayCommand(
							param => TryOpenDvrAndQuickLaunchCtrls(),
							(param) => true
					);
				}
				return _tryOpenDvrAndQuickLaunchCmd;
			}
		}

		public ICommand TryOpenMetadataViewerCmd
		{
			get
			{
				if (_tryOpenMetadataViewerCmd == null)
				{
					_tryOpenMetadataViewerCmd = new RelayCommand(
							param => _metadataViewerViewModel.TryOpenMetadataViewer(),
							(param) => true
					);
				}
				return _tryOpenMetadataViewerCmd;
			}
		}

		// General Ctrls
		public ICommand GeneralCtrlsMouseEnterCmd
		{
			get
			{
				if (_generalCtrlsMouseEnterCmd == null)
				{
					_generalCtrlsMouseEnterCmd = new RelayCommand(
							param => GeneralCtrlsMouseEnter(),
							(param) => true
					);
				}
				return _generalCtrlsMouseEnterCmd;
			}
		}

		public ICommand GeneralCtrlsMouseLeaveCmd
		{
			get
			{
				if (_generalCtrlsMouseLeaveCmd == null)
				{
					_generalCtrlsMouseLeaveCmd = new RelayCommand(
							param => GeneralCtrlsMouseLeave(),
							(param) => true
					);
				}
				return _generalCtrlsMouseLeaveCmd;
			}
		}

		// Video
		public ICommand VideoCtrlMouseEnterCmd
		{
			get
			{
				if (_videoCtrlMouseEnterCmd == null)
				{
					_videoCtrlMouseEnterCmd = new RelayCommand(
							param => VideoCtrlMouseEnter(),
							(param) => true
					);
				}
				return _videoCtrlMouseEnterCmd;
			}
		}
		
		public ICommand VideoCtrlMouseLeaveCmd
		{
			get
			{
				if (_videoCtrlMouseLeaveCmd == null)
				{
					_videoCtrlMouseLeaveCmd = new RelayCommand(
							param => VideoCtrlMouseLeave(),
							(param) => true
					);
				}
				return _videoCtrlMouseLeaveCmd;
			}
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
				isDvrRecentlyUnpinned = true;
			}
		}

		public void TryCloseDvrCtrl()
		{
			if (DvrCtrlVisibility == Visibility.Visible && IsDvrCtrlPinned == false)
			{
				DvrCtrlVisibility = Visibility.Collapsed;
			}
		}

		public void TryOpenDvrCtrl()
		{
			if (isDvrRecentlyUnpinned)
			{
				return;
			}

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
				isQuickLaunchRecentlyUnpinned = true;
			}
		}

		public void TryCloseQuickLaunchCtrl()
		{
			if (QuickLaunchCtrlVisibility == Visibility.Visible && IsQuickLaunchCtrlPinned == false)
			{
				QuickLaunchCtrlVisibility = Visibility.Collapsed;
			}
		}

		public void TryOpenQuickLaunchCtrl()
		{
			if (isQuickLaunchRecentlyUnpinned == true)
			{
				return;
			}

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

		// General Ctrls
		private void GeneralCtrlsMouseEnter()
		{
			// TODO: Implement this?
		}

		private void GeneralCtrlsMouseLeave()
		{
			//isDvrRecentlyUnpinned = false;
			//isQuickLaunchRecentlyUnpinned = false;
		}

		// Video
		private void VideoCtrlMouseEnter()
		{
			//TryOpenDvrAndQuickLaunchCtrls();
			ToggleDvrAndQuickLaunchCtrls();

			DvrCtrlMouseEnter();
			QuickLaunchCtrlMouseEnter();

			isDvrRecentlyUnpinned = false;
			isQuickLaunchRecentlyUnpinned = false;
		}

		private void VideoCtrlMouseLeave()
		{
			DvrCtrlMouseLeave();
			QuickLaunchCtrlMouseLeave();
			isDvrRecentlyUnpinned = false;
			isQuickLaunchRecentlyUnpinned = false;
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
