using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace wpf_git
{
	public class MetadataViewerViewModel : IDisposable
	{
		public MetadataViewerViewModel()
		{
			PopulateTestMetadata();

			VideoMetadataCollection = new ObservableCollection<VideoMetadata>();
			_videoMetadataCollectionView = CollectionViewSource.GetDefaultView(VideoMetadataCollection);

			foreach (var item in _metadata2)
			{
				var metadata = new VideoMetadata() { Key = item.Key, Value = item.Value };
				VideoMetadataCollection.Add(metadata);
			}

			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(150);
			_timer.Tick += _timer_Tick;
		}

		#region Local

		private int _metadataCounter = 0;
		private string _searchTerm = null;
		private DispatcherTimer _timer = null;
		private List<KeyValuePair<string, string>> _metadata1 = null;
		private List<KeyValuePair<string, string>> _metadata2 = null;
		private List<KeyValuePair<string, string>> _metadata3 = null;
		private List<KeyValuePair<string, string>> _metadata4 = null;
		private MetadataViewerWindow _metadataViewerWindow = null;
		private ObservableCollection<VideoMetadata> _videoMetadataCollection = null;
		private ICollectionView _videoMetadataCollectionView = null;

		#endregion

		#region Properties

		public string SearchTerm
		{
			get { return _searchTerm; }
			set
			{
				_searchTerm = value;
				OnPropertyChanged("SearchTerm");
				SearchVideoMetadata(SearchTerm);
			}
		}

		public ObservableCollection<VideoMetadata> VideoMetadataCollection
		{
			get { return _videoMetadataCollection; }
			set
			{
				_videoMetadataCollection = value;
				OnPropertyChanged("VideoMetadataCollection");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion

		#region Methods

		private void CloseMetadataViewerWindow()
		{
			if(_metadataViewerWindow != null)
			{
				_metadataViewerWindow.Close();
			}
		}

		private Predicate<object> GenerateVideoMetadataSearchPredicate(string searchTerm)
		{
			var filter = new Predicate<object>(item =>
			{
				if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrWhiteSpace(searchTerm))
				{
					return true;
				}
				else
				{
					if (((VideoMetadata)item).Key.ToUpper().Contains(searchTerm.ToUpper()) ||
							((VideoMetadata)item).Value.ToUpper().Contains(searchTerm.ToUpper()))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			});
			return filter;
		}
		
		private void PopulateTestMetadata()
		{
			_metadata1 = new List<KeyValuePair<string, string>>();
			_metadata1.Add(new KeyValuePair<string, string>("Checksum", "12345"));
			_metadata1.Add(new KeyValuePair<string, string>("Precision Time Stamp", "Sep. 19 2012, 01:15:32"));
			_metadata1.Add(new KeyValuePair<string, string>("Density Altitude", "36,000"));
			_metadata1.Add(new KeyValuePair<string, string>("Outside Air Temp", "-120"));
			_metadata1.Add(new KeyValuePair<string, string>("Wind Speed", "150"));
			_metadata1.Add(new KeyValuePair<string, string>("Icing Detected", "1"));
			_metadata1.Add(new KeyValuePair<string, string>("Platform Pitch Angle", "14"));
			_metadata2 = new List<KeyValuePair<string, string>>();
			_metadata2.Add(new KeyValuePair<string, string>("Checksum", "56789"));
			_metadata2.Add(new KeyValuePair<string, string>("Precision Time Stamp", "Sep. 19 2012, 01:15:33"));
			_metadata2.Add(new KeyValuePair<string, string>("Density Altitude", "36,022"));
			_metadata2.Add(new KeyValuePair<string, string>("Outside Air Temp", "-121"));
			_metadata2.Add(new KeyValuePair<string, string>("Wind Speed", "156"));
			_metadata2.Add(new KeyValuePair<string, string>("Icing Detected", "1"));
			_metadata2.Add(new KeyValuePair<string, string>("Platform Pitch Angle", "15"));
			_metadata3 = new List<KeyValuePair<string, string>>();
			_metadata3.Add(new KeyValuePair<string, string>("Checksum", "101112"));
			_metadata3.Add(new KeyValuePair<string, string>("Precision Time Stamp", "Sep. 19 2012, 01:15:34"));
			_metadata3.Add(new KeyValuePair<string, string>("Density Altitude", "36,026"));
			_metadata3.Add(new KeyValuePair<string, string>("Outside Air Temp", "-122"));
			_metadata3.Add(new KeyValuePair<string, string>("Wind Speed", "153"));
			_metadata3.Add(new KeyValuePair<string, string>("Icing Detected", "1"));
			_metadata3.Add(new KeyValuePair<string, string>("Platform Pitch Angle", "16"));
			_metadata4 = new List<KeyValuePair<string, string>>();
			_metadata4.Add(new KeyValuePair<string, string>("Checksum", "23443"));
			_metadata4.Add(new KeyValuePair<string, string>("Precision Time Stamp", "Sep. 19 2012, 01:15:35"));
			_metadata4.Add(new KeyValuePair<string, string>("Density Altitude", "36,030"));
			_metadata4.Add(new KeyValuePair<string, string>("Outside Air Temp", "-121"));
			_metadata4.Add(new KeyValuePair<string, string>("Wind Speed", "152"));
			_metadata4.Add(new KeyValuePair<string, string>("Icing Detected", "1"));
			_metadata4.Add(new KeyValuePair<string, string>("Platform Pitch Angle", "12"));
		}

		private void RunModel(string searchTerm)
		{
			VideoMetadataCollection.Clear();

			List<KeyValuePair<string, string>> metadata = null;

			if (_metadataCounter == 0)
			{ metadata = _metadata1; }
			else if (_metadataCounter == 1)
			{ metadata = _metadata2; }
			else if (_metadataCounter == 2)
			{ metadata = _metadata3; }
			else if (_metadataCounter == 3)
			{ metadata = _metadata4; }
			else if (_metadataCounter == 4)
			{ metadata = _metadata1; }

			foreach (var item in metadata)
			{
				var data = new VideoMetadata() { Key = item.Key, Value = item.Value };
				VideoMetadataCollection.Add(data);
			}

			SearchVideoMetadata(searchTerm);

			_metadataCounter++;
			if (_metadataCounter > 4)
			{
				_metadataCounter = 1;
			}
		}

		private void SearchVideoMetadata(string searchTerm)
		{
			_videoMetadataCollectionView.Filter = GenerateVideoMetadataSearchPredicate(searchTerm);
		}

		private void ToggleTimer()
		{
			if (_timer.IsEnabled)
			{
				_timer.Stop();
			}
			else
			{
				_timer.Start();
			}
		}

		// Metadata Viewer
		public void TryOpenMetadataViewer()
		{
			if (_metadataViewerWindow == null)
			{
				_metadataViewerWindow = new MetadataViewerWindow();
				_metadataViewerWindow.DataContext = this;
				_metadataViewerWindow.Height = 550;
				_metadataViewerWindow.Width = 350;
			}

			//_metadataViewerWindow.ShowActivated = true;
			_metadataViewerWindow.Show();
			_metadataViewerWindow.Activate();
		}

		#endregion

		#region Events

		private void _timer_Tick(object sender, EventArgs e)
		{
			RunModel(SearchTerm);
		}

		#endregion

		#region Commands

		private ICommand _closeMetadataViewerWindowCommand;

		public ICommand CloseMetadataViewerWindowCommand
		{
			get
			{
				if (_closeMetadataViewerWindowCommand == null)
				{
					_closeMetadataViewerWindowCommand = new RelayCommand(
							param => CloseMetadataViewerWindow(),
							param => CanCloseMetadataViewerWindow()
					);
				}
				return _closeMetadataViewerWindowCommand;
			}
		}

		private bool CanCloseMetadataViewerWindow()
		{			
			return true;
		}

		private ICommand _startStopMetadataViewerCommand;

		public ICommand StartStopMetadataViewerCommand
		{
			get
			{
				if (_startStopMetadataViewerCommand == null)
				{
					_startStopMetadataViewerCommand = new RelayCommand(
							param => ToggleTimer(),
							param => CanStartStopMetadataViewer()
					);
				}
				return _startStopMetadataViewerCommand;
			}
		}

		private bool CanStartStopMetadataViewer()
		{			
			return true;
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

				_timer.Stop();
				_timer.Tick -= _timer_Tick;
				_timer = null;

				//_metadataViewerWindow.Close();
				//_metadataViewerWindow = null;

				//TryDispose(obj);
			}

			// Free any unmanaged objects here. 
			disposed = true;
		}

		~MetadataViewerViewModel()
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
