using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

namespace wpf_git
{
	public class CalculatorViewModel : INotifyPropertyChanged
	{
		// Comments from 100
		public CalculatorViewModel()
		{
			// More comments from local master.
			DvrCtrlVisibility = Visibility.Visible;
			QuickLaunchCtrlVisibility = Visibility.Visible;
			IsDvrCtrlPinned = true;
			IsQuickLaunchCtrlPinned = true;

			_dvrTimer = new System.Timers.Timer(_ctrlVisibilityTimer);
			_dvrTimer.AutoReset = false;
			_dvrTimer.Elapsed += _dvrTimer_Elapsed;

			_quickLaunchTimer = new System.Timers.Timer(_ctrlVisibilityTimer);
			_quickLaunchTimer.AutoReset = false;
			_quickLaunchTimer.Elapsed += _quickLaunchTimer_Elapsed;
		}

		#region Local

		private bool _isDvrCtrlPinned = true;
		private System.Timers.Timer _dvrTimer;
		private readonly object _dvrTimerlock = new object();
		private string _dvrLaunchPinImage = _dvrPinImagePinned;
		private const string _dvrPinImagePinned = @"~\..\Images\pin_pink_pinned.png";
		private const string _dvrPinImageUnpinned = @"~\..\Images\pin_pink_unpinned.png";

		private bool _isQuickLaunchCtrlPinned = true;
		private System.Timers.Timer _quickLaunchTimer;
		private readonly object _quickLaunchTimerlock = new object();
		private string _quickLaunchPinImage = _quickLaunchPinImagePinned;
		private const string _quickLaunchPinImagePinned = @"~\..\Images\pin_pink_pinned.png";
		private const string _quickLaunchPinImageUnpinned = @"~\..\Images\pin_pink_unpinned.png";

		private double _ctrlVisibilityTimer = 2000;

		private Visibility _dvrCtrlVisibility = Visibility.Visible;
		private Visibility _quickLaunchCtrlVisibility = Visibility.Visible;

		#endregion

		#region Commands

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
			// Return true
			return true;
		}

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
			// Return true
			return true;
		}
		
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
			// Return true
			return true;
		}

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
			// Return true
			return true;
		}

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
			// Return true
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

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion

		#region Methods

		private void TryOpenDvrAndQuickLaunchCtrls()
		{
			TryOpenQuickLaunchCtrl();
			TryOpenDvrCtrl();
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
				_dvrTimer.Interval = 750;
				_dvrTimer.Start();
			}

			if (IsQuickLaunchCtrlPinned == false)
			{
				QuickLaunchTimerStop();
				_quickLaunchTimer.Interval = 750;
				_quickLaunchTimer.Start();
			}
		}

		private void ToggleDvrCtrl()
		{
			if (_isDvrCtrlPinned == false)
			{
				DvrCtrlVisibility = Visibility.Visible;
				_isDvrCtrlPinned = true;
				DvrPinImage = _dvrPinImagePinned;
			}
			else
			{
				DvrCtrlVisibility = Visibility.Collapsed;
				_isDvrCtrlPinned = false;
				DvrPinImage = _dvrPinImagePinned;
			}
		}

		public void TryCloseDvrCtrl()
		{
			if (DvrCtrlVisibility == Visibility.Visible)
			{
				DvrCtrlVisibility = Visibility.Collapsed;
			}
		}

		public void TryOpenDvrCtrl()
		{
			if (DvrCtrlVisibility == Visibility.Collapsed)
			{
				DvrCtrlVisibility = Visibility.Visible;

				_isDvrCtrlPinned = false;
				DvrPinImage = _quickLaunchPinImageUnpinned;
				DvrTimerStop();
				_dvrTimer.Interval = _ctrlVisibilityTimer;
				_dvrTimer.Start();
			}
		}

		// Quick Launch Toolbar
		void QuickLaunchTimerStop()
		{
			lock (_quickLaunchTimerlock)
			{
				_quickLaunchTimer.Enabled = false; // equivalent to calling Stop()
			}
		}

		public void QuickLaunchCtrlMouseEnter()
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

		public void QuickLaunchCtrlMouseLeave()
		{
			if (_isQuickLaunchCtrlPinned == false)
			{
				QuickLaunchTimerStop();
				_quickLaunchTimer.Interval = 750;
				_quickLaunchTimer.Start();
			}
			if (_isDvrCtrlPinned == false)
			{
				DvrTimerStop();
				_dvrTimer.Interval = 750;
				_dvrTimer.Start();
			}
		}

		private void ToggleQuickLaunchCtrl()
		{
			if (_isQuickLaunchCtrlPinned == false)
			{
				QuickLaunchCtrlVisibility = Visibility.Visible;
				_isQuickLaunchCtrlPinned = true;
				QuickLaunchPinImage = _quickLaunchPinImagePinned;
			}
			else
			{
				QuickLaunchCtrlVisibility = Visibility.Collapsed;
				_isQuickLaunchCtrlPinned = false;
				QuickLaunchPinImage = _quickLaunchPinImageUnpinned;
			}
		}

		public void TryCloseQuickLaunchCtr()
		{
			if (QuickLaunchCtrlVisibility == Visibility.Visible)
			{
				QuickLaunchCtrlVisibility = Visibility.Collapsed;
			}
		}

		public void TryOpenQuickLaunchCtrl()
		{
			if (QuickLaunchCtrlVisibility == Visibility.Collapsed)
			{
				QuickLaunchCtrlVisibility = Visibility.Visible;
				_isQuickLaunchCtrlPinned = false;
				QuickLaunchPinImage = _quickLaunchPinImageUnpinned;
				_quickLaunchTimer.Stop();
				_quickLaunchTimer.Interval = _ctrlVisibilityTimer;
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
					DvrPinImage = _dvrPinImageUnpinned;
				}
				else
				{
					DvrCtrlVisibility = Visibility.Visible;
					DvrPinImage = _dvrPinImagePinned;
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
					QuickLaunchPinImage = _quickLaunchPinImageUnpinned;
				}
				else
				{
					QuickLaunchCtrlVisibility = Visibility.Visible;
					QuickLaunchPinImage = _quickLaunchPinImagePinned;
				}

				if (_quickLaunchTimer.Enabled == false)
				{
					return;
				}
			}
		}

		#endregion
	}
}
