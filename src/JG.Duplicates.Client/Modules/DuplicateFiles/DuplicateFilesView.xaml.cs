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
}
