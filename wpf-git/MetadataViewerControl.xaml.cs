using System.Windows.Controls;

namespace wpf_git
{
	/// <summary>
	/// Interaction logic for MetadataViewerControl.xaml
	/// </summary>
	public partial class MetadataViewerControl : UserControl
	{
		public MetadataViewerControl()
		{
			InitializeComponent();
		}

		// This prevents an extra empty column from being created within the DataGrid.
		private void VideoMetadataDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
		}
	}
}
