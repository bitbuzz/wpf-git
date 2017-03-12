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
	public class MetadataViewerViewModel
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
			_timer.Start();
		}

		#region Local

		private int _metadataCounter = 0;
		private string _searchTerm = null;
		private DispatcherTimer _timer = null;
		private List<KeyValuePair<string, string>> _metadata1 = null;
		private List<KeyValuePair<string, string>> _metadata2 = null;
		private List<KeyValuePair<string, string>> _metadata3 = null;
		private ObservableCollection<VideoMetadata> _videoMetadataCollection = null;
		private ICollectionView _videoMetadataCollectionView = null;

		#endregion

		#region Commands

		private ICommand _searchVideoMetadataCollectionCommand;

		public ICommand SearchVideoMetadataCollectionCommand
		{
			get
			{
				if (_searchVideoMetadataCollectionCommand == null)
				{
					_searchVideoMetadataCollectionCommand = new RelayCommand(
							param => SearchVideoMetadata(),
							param => CanSearchVideoMetadataCollection()
					);
				}
				return _searchVideoMetadataCollectionCommand;
			}
		}

		private bool CanSearchVideoMetadataCollection()
		{
			// Return true
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
			// Return true
			return true;
		}

		#endregion

		#region Properties

		public string SearchTerm
		{
			get { return _searchTerm; }
			set
			{
				_searchTerm = value;
				OnPropertyChanged("SearchTerm");
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

		private void PopulateTestMetadata()
		{
			_metadata1 = new List<KeyValuePair<string, string>>();
			_metadata1.Add(new KeyValuePair<string, string>("Checksum", "12345"));
			_metadata1.Add(new KeyValuePair<string, string>("Precision Time Stamp", "Sep. 19 2012, 01:15:32"));
			_metadata1.Add(new KeyValuePair<string, string>("Density Altitude", "36,000"));
			_metadata1.Add(new KeyValuePair<string, string>("Outside Air Temp", "72"));
			_metadata2 = new List<KeyValuePair<string, string>>();
			_metadata2.Add(new KeyValuePair<string, string>("Checksum", "56789"));
			_metadata2.Add(new KeyValuePair<string, string>("Precision Time Stamp", "Sep. 19 2012, 01:16:44"));
			_metadata2.Add(new KeyValuePair<string, string>("Density Altitude", "37,000"));
			_metadata2.Add(new KeyValuePair<string, string>("Outside Air Temp", "73"));
			_metadata3 = new List<KeyValuePair<string, string>>();
			_metadata3.Add(new KeyValuePair<string, string>("Checksum", "101112"));
			_metadata3.Add(new KeyValuePair<string, string>("Precision Time Stamp", "Sep. 19 2012, 01:17:55"));
			_metadata3.Add(new KeyValuePair<string, string>("Density Altitude", "38,000"));
			_metadata3.Add(new KeyValuePair<string, string>("Outside Air Temp", "74"));
		}

		private void RunModel()
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
			{ metadata = _metadata1; }

			foreach (var item in metadata)
			{
				var data = new VideoMetadata() { Key = item.Key, Value = item.Value };
				VideoMetadataCollection.Add(data);
			}

			_videoMetadataCollectionView.Filter = SearchVideoMetadata(SearchTerm);

			_metadataCounter++;
			if (_metadataCounter > 3)
			{
				_metadataCounter = 1;
			}
		}

		private void SearchVideoMetadata()
		{
			_videoMetadataCollectionView.Filter = SearchVideoMetadata(SearchTerm);
		}

		private Predicate<object> SearchVideoMetadata(string searchTerm)
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

		#endregion

		#region Events

		private void _timer_Tick(object sender, EventArgs e)
		{
			RunModel();
		}

		#endregion
	}
}
