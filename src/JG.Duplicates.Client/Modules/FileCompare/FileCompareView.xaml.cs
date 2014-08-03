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

namespace JG.Duplicates.Client.Modules
{
    /// <summary>
    /// Interaction logic for FileCompareView.xaml
    /// </summary>
    public partial class FileCompareView : UserControl
    {
        private readonly IFileCompareViewModel viewModel;

        public FileCompareView()
        {
            InitializeComponent();
        }

        public FileCompareView(IFileCompareViewModel viewModel) : this()
        {
            this.viewModel = viewModel;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this.viewModel;
        }
    }
}
