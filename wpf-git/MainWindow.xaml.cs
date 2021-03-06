﻿using System.Windows;
using System.Windows.Controls;

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
			//((VideoPlayerViewModel)DataContext).MetadataViewerViewModel.TryOpenMetadataViewer();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			((VideoPlayerViewModel)DataContext).Dispose();
		}

    private void TestBtn_Click(object sender, RoutedEventArgs e)
    {
      MessageBox.Show(myVisual3D.Content.Bounds.X.ToString() + ", " + myVisual3D.Content.Bounds.Y.ToString() + ", " + myVisual3D.Content.Bounds.Z.ToString());
    }
  }
}
