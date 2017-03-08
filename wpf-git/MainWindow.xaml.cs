﻿using System;
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
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			DataContext = new CalculatorViewModel();
		}

		private void Video_MouseEnter(object sender, MouseEventArgs e)
		{
			((CalculatorViewModel)DataContext).TryOpenQuickLaunchCtrl();
			((CalculatorViewModel)DataContext).TryOpenDvrCtrl();

			//if(((CalculatorViewModel)DataContext).IsQuickLaunchCtrlPinned)
			//{
			//	QuickLaunchCtrlBorder.SetValue(Grid.RowProperty, 1);
			//	DvrCtrlBorder.SetValue(Grid.RowProperty, 2);
			//	SlowMotionCtrlGrid.SetValue(Grid.RowProperty, 3);
			//}
			//else
			//{
			//	QuickLaunchCtrlBorder.SetValue(Grid.RowProperty, 0);
			//	DvrCtrlBorder.SetValue(Grid.RowProperty, 1);
			//	SlowMotionCtrlGrid.SetValue(Grid.RowProperty, 2);
			//}
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
				val = ActualHeight - 35;
			}
			else
			{
				val = ActualHeight + 35;
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

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Width = 450;
			Height = 457;
			_isWindowInitialized = true;
		}

		private void DoubleAnimation_Completed(object sender, EventArgs e)
		{
			if(((CalculatorViewModel)DataContext).ToggleQuickLaunchCommand.CanExecute(null))
			{
				((CalculatorViewModel)DataContext).ToggleQuickLaunchCommand.Execute(null);
			}
		}
	}
}
