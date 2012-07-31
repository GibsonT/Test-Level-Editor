using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace levelEditor2
{
    /// <summary>
    /// Interaction logic for AddTile.xaml
    /// </summary>
    public partial class AddTileWindow : Window
    {
        public AddTileWindow()
        {
            InitializeComponent();

            AddTileWindowViewModel vm = new AddTileWindowViewModel();
            vm.RequestClose += (s, e) => this.Close();
            this.DataContext = vm;
        }
    }
}
