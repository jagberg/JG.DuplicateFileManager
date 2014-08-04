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
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JG.Duplicates.Client.Modules
{
    /// <summary>
    /// Interaction logic for DuplicateFilesView.xaml
    /// </summary>
    public partial class DuplicateFilesView : UserControl
    {
        private readonly IDuplicateFileViewModel viewModel;

        public DuplicateFilesView()
        {
            InitializeComponent();
        }

        public DuplicateFilesView(IDuplicateFileViewModel viewModel) : this()
        {
            this.viewModel = viewModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this.viewModel;
        }
    }


    public class TreeViewSelectedItemBehavior : Behavior<TreeView>
    {
        //dependency property
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object),
            typeof(TreeViewSelectedItemBehavior),
            new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true });

        //property wrapper
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (this.AssociatedObject != null)
                this.AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
        }

        private void OnTreeViewSelectedItemChanged(object sender,
            RoutedPropertyChangedEventArgs<object> e)
        {
            this.SelectedItem = e.NewValue;
        }
    }
}
