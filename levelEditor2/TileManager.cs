using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace levelEditor2
{
    class TileManager
    {
        public static ObservableCollection<Tile> StructuralItems { get; set; }
        public static ObservableCollection<ObservableCollection<Tile>> TileGrid { get; set; }
        public static Tile SelectedTile { get; set; }

        public TileManager()
        {
            StructuralItems = new ObservableCollection<Tile>();
            TileGrid = new ObservableCollection<ObservableCollection<Tile>>();
            SelectedTile = new Tile();
        }
    }
}
