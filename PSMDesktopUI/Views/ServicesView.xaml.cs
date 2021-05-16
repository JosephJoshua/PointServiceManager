using DevExpress.Xpf.Grid;
using PSMDesktopUI.ViewModels;
using System.Windows.Controls;

namespace PSMDesktopUI.Views
{
    public partial class ServicesView : UserControl
    {
        public ServicesView()
        {
            InitializeComponent();
        }

        private void OnRefresh()
        {
            TableView tableView = ServicesGrid.View as TableView;
            tableView.BestFitColumns();

            foreach (GridColumn column in ServicesGrid.Columns)
            {
                column.Width = column.Width.Value + 20;
            }
        }

        private void SetGridWidth()
        {
            double lcWidth = MainLayoutControl.ActualWidth;

            GridLayoutGroup.Width = lcWidth * 0.7d;
        }

        private void View_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ServicesViewModel vm = (ServicesViewModel)DataContext;
            vm.OnRefresh += OnRefresh;

            SetGridWidth();
        }
    }
}
