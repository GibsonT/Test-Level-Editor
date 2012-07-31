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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace levelEditor2
{
    /// <summary>
    /// Interaction logic for TileGrid.xaml
    /// </summary>
    public partial class TileGrid : UserControl, INotifyPropertyChanged
    {
        #region Dependency Properties
        public DependencyProperty NumberOfRowsProperty;
        public DependencyProperty NumberOfColumnsProperty;
        private ObservableCollection<ObservableCollection<Image>> imageList { get; set; }

        public int NumberOfRows
        {
            get
            {
                return (int)GetValue(NumberOfRowsProperty);
            }
            set
            {
                SetValue(NumberOfRowsProperty, value);
            }
        }
        public int NumberOfColumns
        {
            get
            {
                return (int)GetValue(NumberOfColumnsProperty);
            }
            set
            {
                SetValue(NumberOfColumnsProperty, value);
            }
        }
        #endregion

        public TileGrid()
        {
            InitializeComponent();

            NumberOfRowsProperty = DependencyProperty.Register(
            "NumberOfRows", typeof(int), typeof(TileGrid), new PropertyMetadata(
                new PropertyChangedCallback(OnNumberOfRowsChanged)));

            NumberOfColumnsProperty = DependencyProperty.Register(
            "NumberOfColumns", typeof(int), typeof(TileGrid), new PropertyMetadata(
                new PropertyChangedCallback(OnNumberOfColumnsChanged)));

            imageList = new ObservableCollection<ObservableCollection<Image>>();

            NumberOfRows = 5;
            NumberOfColumns = 5;
            FillEmptyImages();
        }

        private void RemoveRows(int numberOfRowsToRemove)
        {
            int rowDefCount = editorGrid.RowDefinitions.Count;
            for (int i = rowDefCount - 1; i > (rowDefCount - 1) - numberOfRowsToRemove; i--)
            {
                editorGrid.RowDefinitions.RemoveAt(i);
                imageList.RemoveAt(imageList.Count - 1);
                TileManager.TileGrid.RemoveAt(TileManager.TileGrid.Count - 1);
            }
        }

        private void AddRows(int numberOfRowsToAdd)
        {
            for (int i = 0; i < numberOfRowsToAdd; i++)
            {
                editorGrid.RowDefinitions.Add(new RowDefinition() { MaxHeight = 32 });
                imageList.Add(new ObservableCollection<Image>());
                TileManager.TileGrid.Add(new ObservableCollection<Tile>());
                for (int j = 0; j < NumberOfColumns; j++)
                {
                    imageList[imageList.Count - 1].Add(new Image());
                    TileManager.TileGrid[TileManager.TileGrid.Count - 1].Add(new Tile());
                }
            }
        }

        private void RemoveColumns(int numberOfColumnsToRemove)
        {
            int colDefCount = editorGrid.ColumnDefinitions.Count;
            for (int i = colDefCount - 1; i > (colDefCount - 1) - numberOfColumnsToRemove; i--)
            {
                editorGrid.ColumnDefinitions.RemoveAt(i);
                for (int j = 0; j < imageList.Count; j++)
                {
                    imageList[j].RemoveAt(imageList[j].Count - 1);
                    TileManager.TileGrid[j].RemoveAt(TileManager.TileGrid[j].Count - 1);
                }
            }
        }

        private void AddColumns(int numberOfColumnsToAdd)
        {
            for (int i = 0; i < numberOfColumnsToAdd; i++)
            {
                editorGrid.ColumnDefinitions.Add(new ColumnDefinition() { MaxWidth = 32 });
                for (int j = 0; j < imageList.Count; j++)
                {
                    imageList[j].Add(new Image());
                    TileManager.TileGrid[j].Add(new Tile());
                }
            }
        }

        private void FillEmptyImages()
        {
            for (int i = 0; i < editorGrid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < editorGrid.ColumnDefinitions.Count; j++)
                {
                    if (imageList[i][j].Source == null)
                    {
                        //imageList[i][j] = new Image() { Source = new BitmapImage(new Uri("C:/empty.png", UriKind.Absolute)) };
                        imageList[i][j].Source = TileManager.TileGrid[i][j].Image;
                        imageList[i][j].MouseLeftButtonDown += new MouseButtonEventHandler(Image_MouseLeftButtonDown);
                        imageList[i][j].MouseRightButtonDown += new MouseButtonEventHandler(Image_MouseRightButtonDown);
                        imageList[i][j].SetValue(Grid.RowProperty, i);
                        imageList[i][j].SetValue(Grid.ColumnProperty, j);
                        editorGrid.Children.Add(imageList[i][j]);
                    }
                }
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image i = (Image)sender;
            i.Width = 32;
            i.Height = 32;
            TileManager.TileGrid[Grid.GetRow(i)][Grid.GetColumn(i)] = TileManager.SelectedTile;
            i.Source = TileManager.SelectedTile.Image;
        }

        private void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image i = sender as Image;
            TileManager.TileGrid[Grid.GetRow(i)][Grid.GetColumn(i)] = new Tile();
            imageList[Grid.GetRow(i)][Grid.GetColumn(i)].Source = TileManager.TileGrid[Grid.GetRow(i)][Grid.GetColumn(i)].Image;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void OnNumberOfRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TileGrid g = (TileGrid)d;
            int newValue = (int)e.NewValue;
            int oldValue = (int)e.OldValue;
            if (newValue < oldValue)
            {
                if (MessageBox.Show(
                    "This may result in a loss of data. Continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    g.RemoveRows(oldValue - newValue);
                }
                else
                {
                    return;
                }
            }
            else if (newValue > oldValue)
            {
                g.AddRows(newValue - oldValue);
                FillEmptyImages();
            }
        }

        public void OnNumberOfColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TileGrid g = (TileGrid)d;
            int newValue = (int)e.NewValue;
            int oldValue = (int)e.OldValue;
            if (newValue < oldValue)
            {
                if (MessageBox.Show(
                    "This may result in a loss of data. Continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    {
                        g.RemoveColumns(oldValue - newValue);
                    }
                }
                else
                {
                    return;
                }
            }
            else if (newValue > oldValue)
            {
                g.AddColumns(newValue - oldValue);
                FillEmptyImages();
            }
        }
    }
}
