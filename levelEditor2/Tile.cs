using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace levelEditor2
{
    public class Tile : INotifyPropertyChanged
    {
        public enum TileType
        { 
            Structural,
            Actor,
            Controller,
            NULL
        };
        
        public event PropertyChangedEventHandler PropertyChanged;
        private BitmapImage _image;
        private int _textureID;
        private TileType _thisTileType;
        private string _relativeImagePath;

        public BitmapImage Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged("Image");
                }
            }
        }
        public int TextureID
        {
            get
            {
                return _textureID;
            }
            set
            {
                if (_textureID != value)
                {
                    _textureID = value;
                    OnPropertyChanged("TextureKey");
                }
            }
        }
        public TileType ThisTileType
        {
            get
            {
                return _thisTileType;
            }
            set
            {
                if (_thisTileType != value)
                {
                    _thisTileType = value;
                    OnPropertyChanged("ThisTileType");
                }
            }
        }
        public string RelativeImagePath
        {
            get
            {
                return _relativeImagePath;
            }
            set
            {
                if (_relativeImagePath != value)
                {
                    _relativeImagePath = value;
                    OnPropertyChanged("RelativeImagePath");
                }
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Tile()
        { 
            this.Image = new BitmapImage(new Uri("C:/empty.png", UriKind.Absolute));
            this.TextureID = -1;
            this.ThisTileType = TileType.NULL;
            this.RelativeImagePath = "Images/empty.png";
        }
    }
}
