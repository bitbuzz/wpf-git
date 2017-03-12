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
using System.Windows.Shapes;

namespace wpf_git
{
	/// <summary>
	/// Interaction logic for ExpanderTester.xaml
	/// </summary>
	public partial class ExpanderTester : Window
	{
		public ExpanderTester()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Width = 580;
			Height = 550;
		}

		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{
			rotate.Angle += 1;
		}
	}
}
