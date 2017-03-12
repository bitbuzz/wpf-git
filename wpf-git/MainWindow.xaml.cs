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
		private double _dvrToolbarHeight = 33;
		private double _quickLaunchToolbarHeight = 29;
		private bool _isWindowInitialized = false;

		public MainWindow()
		{
			InitializeComponent();
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			DataContext = new VideoPlayerViewModel();
		}

		//private void Video_MouseEnter(object sender, MouseEventArgs e)
		//{
		//	((VideoPlayerViewModel)DataContext).TryOpenQuickLaunchCtrl();
		//	((VideoPlayerViewModel)DataContext).TryOpenDvrCtrl();
		//}

		private void QuickLaunch_MouseEnter(object sender, MouseEventArgs e)
		{
			((VideoPlayerViewModel)DataContext).QuickLaunchCtrlMouseEnter();
		}

		private void QuickLaunch_MouseLeave(object sender, MouseEventArgs e)
		{
			((VideoPlayerViewModel)DataContext).QuickLaunchCtrlMouseLeave();
		}

		private void Dvr_MouseEnter(object sender, MouseEventArgs e)
		{
			((VideoPlayerViewModel)DataContext).DvrCtrlMouseEnter();
		}

		private void Dvr_MouseLeave(object sender, MouseEventArgs e)
		{
			((VideoPlayerViewModel)DataContext).DvrCtrlMouseLeave();
		}

		private void GeneralCtrlsGrid_MouseEnter(object sender, MouseEventArgs e)
		{
			((VideoPlayerViewModel)DataContext).GeneralCtrlsMouseEnter();
		}

		private void GeneralCtrlsGrid_MouseLeave(object sender, MouseEventArgs e)
		{
			((VideoPlayerViewModel)DataContext).GeneralCtrlsMouseLeave();
		}

		private void dvrVisibility_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!_isWindowInitialized) return;
			double val;			
			if(((Border)sender).Visibility == Visibility.Collapsed)
			{
				val = ActualHeight - _dvrToolbarHeight;
			}
			else
			{
				val = ActualHeight + _dvrToolbarHeight;
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
				val = ActualHeight - _quickLaunchToolbarHeight;
			}
			else
			{
				val = ActualHeight + _quickLaunchToolbarHeight;
			}
			if (val < 0) return;
			Height = val;
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
