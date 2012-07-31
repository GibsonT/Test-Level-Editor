using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Xml;

namespace levelEditor2
{
    class LevelEditorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static int _tileCount = 0;

        private DelegateCommand _exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new DelegateCommand(Exit);
                }

                return _exitCommand;
            }
        }
        public void Exit(object obj)
        {
            Application.Current.Shutdown();
        }

        private DelegateCommand _exportCommand;
        public ICommand ExportCommand
        {
            get
            {
                if (_exportCommand == null)
                {
                    _exportCommand = new DelegateCommand(Export);
                }

                return _exportCommand;
            }
        }
        public void Export(object obj)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Save WCF File";
            dialog.AddExtension = true;
            dialog.Filter = "World Creation Files (*.wcf)|*.wcf";
            if (dialog.ShowDialog() == true)
            {
                XmlWriterSettings xmlSettings = new XmlWriterSettings();
                xmlSettings.Indent = true;
                xmlSettings.NewLineOnAttributes = false;
                xmlSettings.OmitXmlDeclaration =  true;
                XmlWriter wcfWriter = XmlWriter.Create(dialog.FileName, xmlSettings);
                XmlWriter rmfWriter = XmlWriter.Create(dialog.FileName + ".rmf", xmlSettings);

                #region WCF Generation
                wcfWriter.WriteStartDocument();                 //Start of file.
                wcfWriter.WriteStartElement("WCF");             //Start "WCF"
                wcfWriter.WriteStartElement("OBJECTS");         //Start "OBJECTS"
                for (int i = 0; i < TileManager.TileGrid.Count; i++)
                {
                    int structuralTileCount = 0;
                    for (int j = 0; j < TileManager.TileGrid[i].Count; j++)
                    {
                        Tile currentTile = TileManager.TileGrid[i][j];
                        if (currentTile.ThisTileType != Tile.TileType.NULL)
                        {
                            structuralTileCount++;

                            int xPos = j * 32;
                            int yPos = i * 32;

                            wcfWriter.WriteStartElement("STRUCTURAL");  //Start "STRUCTURAL" block

                            wcfWriter.WriteAttributeString("TexID", currentTile.TextureID.ToString());
                            wcfWriter.WriteAttributeString("X", xPos.ToString());
                            wcfWriter.WriteAttributeString("Y", yPos.ToString());

                            wcfWriter.WriteEndElement();                //End "STRUCTURAL" block
                        }
                        else if (structuralTileCount > 0)
                        {

                            int xPos = (j - structuralTileCount) * 32;
                            int yPos = i * 32;
                            int width = structuralTileCount * 32;
                            int height = 32;

                            wcfWriter.WriteStartElement("BOUNDS");

                            wcfWriter.WriteAttributeString("X", xPos.ToString());
                            wcfWriter.WriteAttributeString("Y", yPos.ToString());
                            wcfWriter.WriteAttributeString("Width", width.ToString());
                            wcfWriter.WriteAttributeString("Height", height.ToString());

                            wcfWriter.WriteEndElement();


                            structuralTileCount = 0;
                        }
                    }

                    if (structuralTileCount > 0)
                    {
                        int xPos = (TileManager.TileGrid[i].Count - structuralTileCount) * 32;
                        int yPos = i * 32;
                        int width = structuralTileCount * 32;
                        int height = 32;

                        wcfWriter.WriteStartElement("BOUNDS");

                        wcfWriter.WriteAttributeString("X", xPos.ToString());
                        wcfWriter.WriteAttributeString("Y", yPos.ToString());
                        wcfWriter.WriteAttributeString("Width", width.ToString());
                        wcfWriter.WriteAttributeString("Height", height.ToString());

                        wcfWriter.WriteEndElement();
                    }
                }

                wcfWriter.WriteEndElement();    //End "OBJECTS"
                wcfWriter.WriteEndElement();    //End "WCF"
                wcfWriter.WriteEndDocument();   //EOF
                wcfWriter.Flush();
                #endregion

                rmfWriter.WriteStartDocument();
                rmfWriter.WriteStartElement("RMF");
                foreach (Tile t in TileManager.StructuralItems)
                {
                    rmfWriter.WriteStartElement("TEXTURE");
                    rmfWriter.WriteAttributeString("TexID", t.TextureID.ToString());
                    rmfWriter.WriteAttributeString("Image", t.RelativeImagePath);
                    rmfWriter.WriteEndElement();
                }
                rmfWriter.WriteEndElement();
                rmfWriter.WriteEndDocument();
                rmfWriter.Flush();

                MessageBox.Show("DONE");
            }
        }

        private DelegateCommand _addStructuralItemCommand;
        public ICommand AddStructuralItemCommand
        {
            get
            {
                if (_addStructuralItemCommand == null)
                {
                    _addStructuralItemCommand = new DelegateCommand(AddStructuralItem);
                }

                return _addStructuralItemCommand;
            }
        }
        public void AddStructuralItem(object obj)
        {
            if (TileManager.StructuralItems == null)
            {
                TileManager.StructuralItems = new ObservableCollection<Tile>();
            }

            AddTileWindow win = new AddTileWindow();
            win.ShowDialog();
            AddTileWindowViewModel vm = (AddTileWindowViewModel)win.DataContext;

            if (vm.CreationSuccess)
            {
                TileManager.StructuralItems.Add(CreateNewTile(vm.NewTile, Tile.TileType.Structural));
            }
        }

        private DelegateCommand _selectedTileChangedCommand;
        public ICommand SelectedTileChangedCommand
        {
            get
            {
                if (_selectedTileChangedCommand == null)
                {
                    _selectedTileChangedCommand = new DelegateCommand(SelectedTileChanged);
                }

                return _selectedTileChangedCommand;
            }
        }
        public void SelectedTileChanged(object obj)
        {
            Tile t = (Tile)obj;
            TileManager.SelectedTile = t;
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static Tile CreateNewTile(Tile tileToCreate, Tile.TileType tileType)
        {
            _tileCount++;
            
            Tile tile = new Tile();
            tile.Image = tileToCreate.Image;
            tile.TextureID = _tileCount;
            tile.RelativeImagePath = tileToCreate.RelativeImagePath;
            tile.ThisTileType = tileType;
            return tile;
        }

        public LevelEditorViewModel()
        {
            TileManager.StructuralItems = new ObservableCollection<Tile>();
        }
    }
}
