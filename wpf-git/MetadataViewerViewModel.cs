using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace wpf_git
{
	public class MetadataViewerViewModel
	{
		public MetadataViewerViewModel()
		{
			_videoMetadataTable = new DataTable("VideoMetadataTable");
			_videoMetadataTable.Columns.Add("Key", typeof(string));
			_videoMetadataTable.Columns.Add("Value", typeof(string));
			VideoMetadataView = _videoMetadataTable.DefaultView;
			_videoMetadataTableBindingListView = _videoMetadataTable.DefaultView;

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

			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(150);
			_timer.Tick += _timer_Tick;
			_timer.Start();
		}

		#region Local

		private int _metadataCounter = 0;
		private string _searchTerm = null;
		private DispatcherTimer _timer = null;
		private DataTable _videoMetadataTable = null;
		private DataView _videoMetadataView = null;
		private IBindingListView _videoMetadataTableBindingListView = null;
		private List<KeyValuePair<string, string>> _metadata1 = null;
		private List<KeyValuePair<string, string>> _metadata2 = null;
		private List<KeyValuePair<string, string>> _metadata3 = null;

		#endregion

		#region Commands

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

		public DataView VideoMetadataView
		{
			get { return _videoMetadataView; }
			set
			{
				_videoMetadataView = value;
				OnPropertyChanged("VideoMetadataView");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion

		#region Methods

		private void RunModelLight()
		{
			VideoMetadataView.Table.Clear();

			foreach (var item in _metadata1)
			{
				DataRow row = _videoMetadataTable.NewRow();
				row.SetField(0, item.Key);
				row.SetField(1, item.Value);
				_videoMetadataTable.Rows.Add(row);
			}
			VideoMetadataView = _videoMetadataTable.DefaultView;

			VideoMetadataView.Table.Clear();

			foreach (var item in _metadata2)
			{
				DataRow row = _videoMetadataTable.NewRow();
				row.SetField(0, item.Key);
				row.SetField(1, item.Value);
				_videoMetadataTable.Rows.Add(row);
			}
			VideoMetadataView = _videoMetadataTable.DefaultView;

			VideoMetadataView.Table.Clear();

			foreach (var item in _metadata3)
			{
				DataRow row = _videoMetadataTable.NewRow();
				row.SetField(0, item.Key);
				row.SetField(1, item.Value);
				_videoMetadataTable.Rows.Add(row);
			}
			VideoMetadataView = _videoMetadataTable.DefaultView;
		}

		private void RunModel()
		{
			_videoMetadataTable.Clear();

			if (VideoMetadataView != null && VideoMetadataView.Table != null)
			{
				VideoMetadataView.Table.Clear();
			}

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
				DataRow row = _videoMetadataTable.NewRow();
				row.SetField(0, item.Key);
				row.SetField(1, item.Value);
				_videoMetadataTable.Rows.Add(row);
			}

			VideoMetadataView = _videoMetadataTable.DefaultView;

			// Google: IBindingListView regex
			// http://stackoverflow.com/questions/819526/net-bindingsource-filter-with-regular-expressions
			//var source = myDataTable.AsEnumerable();
			//var results = from matchingItem in source
			//							where Regex.IsMatch(matchingItem.Field<string>("Name"), "<put Regex here>")
			//							select matchingItem;
			////If you need them as a list when you are done (to bind to or something)
			//var list = results.ToList();

			if (string.IsNullOrEmpty(SearchTerm) == false && string.IsNullOrWhiteSpace(SearchTerm) == false)
			{
				_videoMetadataTableBindingListView.Filter = "Key = '" + SearchTerm + "'";
			}
			else
			{
				_videoMetadataTableBindingListView.Filter = null;
			}

			_metadataCounter++;
			if (_metadataCounter > 3)
			{
				_metadataCounter = 1;
			}
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
