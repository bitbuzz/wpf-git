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
using System.Windows.Media.Animation;
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
		private double _dvrToolbarHeight = 33;
		private double _quickLaunchToolbarHeight = 29;
		private bool _isWindowInitialized = false;

		public MainWindow()
		{
			InitializeComponent();
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			DataContext = new VideoPlayerViewModel();

			//ExpanderTester et = new ExpanderTester();
			//et.Show();
		}

		private void DvrCtrlBorder_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!_isWindowInitialized)
			{
				return;
			}

			double val;			
			if(((Border)sender).Visibility == Visibility.Collapsed)
			{
				val = ActualHeight - _dvrToolbarHeight;
			}
			else
			{
				val = ActualHeight + _dvrToolbarHeight;
			}

			if (val < 0)
			{
				return;
			}
			else
			{
				Height = val;
			}
		}

		private void QuickLaunchCtrlBorder_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!_isWindowInitialized) 
			{ 
				return; 
			}

			double val;
			if (((Border)sender).Visibility == Visibility.Collapsed)
			{
				val = ActualHeight - _quickLaunchToolbarHeight;
			}
			else
			{
				val = ActualHeight + _quickLaunchToolbarHeight;
			}

			if (val < 0)
			{
				return;
			}
			else
			{
				Height = val;
			}			
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Width = 580;
			Height = 547;
			_isWindowInitialized = true;
			((VideoPlayerViewModel)DataContext).MetadataViewerViewModel.TryOpenMetadataViewer();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			((VideoPlayerViewModel)DataContext).Dispose();
		}
	}
}
