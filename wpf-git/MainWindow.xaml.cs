using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf_git
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private bool _isWindowInitialized = false;

		public MainWindow()
		{
			InitializeComponent();
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
			DataContext = new CalculatorViewModel();
			_isWindowInitialized = true;
		}

		private void Video_MouseEnter(object sender, MouseEventArgs e)
		{
			((CalculatorViewModel)DataContext).TryOpenQuickLaunchCtrl();
			((CalculatorViewModel)DataContext).TryOpenDvrCtrl();
		}

		private void QuickLaunch_MouseEnter(object sender, MouseEventArgs e)
		{
			((CalculatorViewModel)DataContext).QuickLaunchCtrlMouseEnter();
		}

		private void QuickLaunch_MouseLeave(object sender, MouseEventArgs e)
		{
			((CalculatorViewModel)DataContext).QuickLaunchCtrlMouseLeave();
		}

		private void Dvr_MouseEnter(object sender, MouseEventArgs e)
		{
			((CalculatorViewModel)DataContext).DvrCtrlMouseEnter();
		}

		private void Dvr_MouseLeave(object sender, MouseEventArgs e)
		{
			((CalculatorViewModel)DataContext).DvrCtrlMouseLeave();
		}

		private void dvrVisibility_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!_isWindowInitialized) return;
			double val;			
			if(((Border)sender).Visibility == Visibility.Collapsed)
			{
				val = ActualHeight - 33;
			}
			else
			{
				val = ActualHeight + 33;
			}
			if (val < 0) return;
			Height = val;
		}

		private void quickLaunchVisibility_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!_isWindowInitialized) return;
			double val;
			if (((Border)sender).Visibility == Visibility.Collapsed)
			{
				val = ActualHeight - 35;
			}
			else
			{
				val = ActualHeight + 35;
			}
			if (val < 0) return;
			Height = val;
		}
	}
}
