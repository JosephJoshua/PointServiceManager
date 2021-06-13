using DevExpress.Xpf.Grid;
using PSMDesktopUI.ViewModels;
using System.Windows.Controls;

namespace PSMDesktopUI.Views
{
    public partial class ServicesView : UserControl
    {
        private bool _isFirstLoad = true;

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

        private void SetInitialGridWidth()
        {
            double lcWidth = MainLayoutControl.ActualWidth;
            GridLayoutGroup.Width = lcWidth * 0.7d;
        }

        private void View_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ServicesViewModel vm = (ServicesViewModel)DataContext;
            vm.OnRefresh += OnRefresh;

            if (_isFirstLoad)
            {
                SetInitialGridWidth();
            }

            _isFirstLoad = true;
        }
    }
}
