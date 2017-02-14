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
			Value1 = "5";
			Value2 = "6";
		}

		private string _value1;
		private string _value2;

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

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

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
			return true;
		}

		private void Calculate()
		{
			var list = new List<int>();
			list.Add(Convert.ToInt32(_value1));
			list.Add(Convert.ToInt32(_value2));
			var result = Calculator.Add(list) + 100 - 200 + 1.75;
			MessageBox.Show(result.ToString());
		}
	}
}
