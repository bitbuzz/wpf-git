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
		public CalculatorViewModel()
		{
			Value1 = "500";
			Value2 = "600";
			QuickLaunchHeight = 31;
		}

		private string _value1;
		private string _value2;
		private double _quickLaunchHeight = 31;

		#region Properties 

		public string Value1
		{
			get { return _value1; }
			set
			{
				_value1 = value;
				OnPropertyChanged("Value1");
			}
		}

		public string Value2
		{
			get { return _value2; }
			set
			{
				_value2 = value;
				OnPropertyChanged("Value2");
			}
		}

		public double QuickLaunchHeight
		{
			get { return _quickLaunchHeight; }
			set
			{
				_quickLaunchHeight = value;
				OnPropertyChanged("QuickLaunchHeight");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
		
		#endregion

		#region Commands
		
		private ICommand _calculateCommand;

		public ICommand CalculateCommand
		{
			get
			{
				if (_calculateCommand == null)
				{
					_calculateCommand = new RelayCommand(
							param => Calculate(),
							param => CanCalculate()
					);
				}
				return _calculateCommand;
			}
		}

		private bool CanCalculate()
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
							param => ToggleQuickLaunch(),
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

		#endregion

		#region Methods

		private void Calculate()
		{
			// Change the values here.
			var list = new List<int>();
			list.Add(Convert.ToInt32(_value1));
			list.Add(Convert.ToInt32(_value2));
			MessageBox.Show(Calculator.Add(list).ToString());
		}

		public void CloseQuickLaunch()
		{
			QuickLaunchHeight = 31;
		}

		public void OpenQuickLaunch()
		{
			QuickLaunchHeight = 31;
		}

		private void ToggleQuickLaunch()
		{
			if(QuickLaunchHeight == 0)
			{
				QuickLaunchHeight = 31;
			}
			else
			{
				QuickLaunchHeight = 0;
			}
		}

		#endregion
	}
}
