using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_git
{
	public class VideoMetadata : INotifyPropertyChanged
	{
		private string _key = null;
		private string _value = null;

		public string Key
		{
			get { return _key; }
			set
			{
				_key = value;
				OnPropertyChanged("Key");
			}
		}

		public string Value
		{
			get { return _value; }
			set
			{
				_value = value;
				OnPropertyChanged("Value");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
