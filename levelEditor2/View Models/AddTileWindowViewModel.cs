using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;

namespace levelEditor2
{
    class AddTileWindowViewModel : IRequestCloseViewModel
    {
        public Tile NewTile { get; set; }
        public bool CreationSuccess { get; set; }
        public event EventHandler RequestClose;

        private DelegateCommand _browseForImageCommand;
        public ICommand BrowseForImageCommand
        {
            get
            {
                if (_browseForImageCommand == null)
                {
                    _browseForImageCommand = new DelegateCommand(BrowseForImage);
                }

                return _browseForImageCommand;
            }
        }
        private void BrowseForImage(object obj)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                NewTile.Image = new BitmapImage(new Uri(dialog.FileName, UriKind.Absolute));
                for (int i = NewTile.RelativeImagePath.Length - 1; i >= 0; i--)
                {
                    if (NewTile.RelativeImagePath[i].Equals('/') || i == 0)
                    {
                        NewTile.RelativeImagePath = NewTile.RelativeImagePath.Remove(i + 1);
                        NewTile.RelativeImagePath += dialog.SafeFileName;
                        return;
                    }
                }
            }
        }

        private DelegateCommand _confirmCreationCommand;
        public ICommand ConfirmCreationCommand
        {
            get
            {
                if (_confirmCreationCommand == null)
                {
                    _confirmCreationCommand = new DelegateCommand(ConfirmCreation);
                }

                return _confirmCreationCommand;
            }
        }
        private void ConfirmCreation(object obj)
        {
            CreationSuccess = true;

            if (RequestClose != null)
            { 
                RequestClose(this, new EventArgs());
            }
        }

        private DelegateCommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new DelegateCommand(Cancel);
                }

                return _cancelCommand;
            }
        }
        private void Cancel(object obj)
        {
            if (RequestClose != null)
            {
                RequestClose(this, new EventArgs());
            }
        }

        public AddTileWindowViewModel()
        {
            NewTile = new Tile();
        }
    }
}
