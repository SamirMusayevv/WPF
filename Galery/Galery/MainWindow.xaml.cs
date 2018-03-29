using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System;

namespace Galery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageFilesFilter _imageFilesFilter;
        private DirectoryInfo _folder;
        private List<List<string>> _albums;
        private List<string> _albumImages;
        private DispatcherTimer _dispatcherTimer;
        private int _currentImageNumber;
        private int _oldImageNumber;
        private bool _timerBool;
        private bool _showAlbums;
        private int _currentAlbumNumber;
        private int _oldAlbumNumber;

        public MainWindow()
        {
            InitializeComponent();

            _imageFilesFilter = new ImageFilesFilter();
            _albums = new List<List<string>>();
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            _currentImageNumber = 0;
            _currentAlbumNumber = 0;
            _timerBool = false;
            _dispatcherTimer.IsEnabled = false;
            _showAlbums = true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var openFolder = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFolder.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _folder = new DirectoryInfo(openFolder.SelectedPath);
                if (_folder.Exists)
                {
                    _albumImages = new List<string>();
                    foreach (var path in _folder.GetFiles())
                    {
                        if (_imageFilesFilter.CheckImages(path.ToString()))
                        {
                            var fullPath = _folder.FullName + "\\" + path.ToString();
                            _albumImages.Add(fullPath);
                        }
                    }
                    _albums.Add(_albumImages);
                    _showAlbums = false;
                    if (_albums.Count != 0)
                    {
                        AlbumWrapPanel.Children.Clear();
                        for (int i = 0; i < _albums.Count; i++)
                        {
                            var btn = new Button
                            {
                                Height = 70,
                                Width = 150,
                                BorderBrush = Brushes.Red,
                                Content = $"Album{i + 1}"
                            };
                            btn.Template = FindResource("AlbumBtn") as ControlTemplate;
                            AlbumWrapPanel.Children.Add(btn);
                        }
                    }
                    _currentAlbumNumber = _albums.Count - 1;
                    CurrentAlbumNumber();
                    WrapPanelImagesAdd(_albums);
                    PortretImage(_albums);
                }
            }
        }

        private void WrapPanelImagesAdd(List<List<string>> albums)
        {
            if (albums != null)
            {
                WrapPanelImages.Children.Clear();
                for (int i = 0; i < albums.Count; i++)
                {
                    if (i == _currentAlbumNumber)
                    {
                        for (int j = 0; j < albums[i].Count; j++)
                        {
                            var btn = new Button
                            {
                                BorderBrush = Brushes.Red,
                                Content = albums[i][j],
                                Width = 150,
                                Height = 100
                            };
                            btn.Template = FindResource("ImgButton") as ControlTemplate;
                            WrapPanelImages.Children.Add(btn);
                        }
                    }
                }
            }
        }

        private void PortretImage(List<List<string>> albums)
        {
            if (albums != null)
            {
                for (int i = 0; i < albums.Count; i++)
                {
                    if (i == _currentAlbumNumber)
                    {
                        for (int j = 0; j < albums[i].Count; j++)
                        {
                            if (j == _currentImageNumber)
                            {
                                var portret = new Button
                                {
                                    Content = albums[i][j]
                                };
                                portret.Template = FindResource("ImgPortret") as ControlTemplate;
                                CurrentImageNumber();
                                DockPortret.Children.Clear();
                                DockPortret.Children.Add(portret);
                            }
                        }
                    }
                }
            }
        }

        private void ButtonFolder_Click(object sender, RoutedEventArgs e)
        {
            if (_showAlbums)
            {
                _showAlbums = false;
                AlbumWrapPanel.Children.Clear();
                if (_albums.Count != 0)
                {
                    for (int i = 0; i < _albums.Count; i++)
                    {
                        var lbl = new Button
                        {
                            Height = 70,
                            Width = 150,
                            BorderBrush = Brushes.Red,
                            Content = $"Album{i + 1}"
                        };
                        lbl.Template = FindResource("AlbumBtn") as ControlTemplate;
                        AlbumWrapPanel.Children.Add(lbl);
                    }
                    CurrentAlbumNumber();
                }
            }
            else
            {
                _showAlbums = true;
                AlbumWrapPanel.Children.Clear();
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (WrapPanelImages.Children.Count > 0)
            {
                if (_currentImageNumber != _albumImages.Count - 1)
                {
                    _oldImageNumber = _currentImageNumber;
                    _currentImageNumber++;
                    DockPortret.Children.Clear();
                    for (int i = 0; i < _albumImages.Count - 1; i++)
                    {
                        if (i == _oldImageNumber)
                            OldImageNumber();
                    }
                    PortretImage(_albums);
                }
            }
        }

        private void ButtonLast_Click(object sender, RoutedEventArgs e)
        {
            if (WrapPanelImages.Children.Count > 0)
            {
                if (_currentImageNumber != 0)
                {
                    _oldImageNumber = _currentImageNumber;
                    _currentImageNumber--;
                    DockPortret.Children.Clear();
                    for (int i = _albumImages.Count - 1; i > 0; i--)
                    {
                        if (i == _oldImageNumber)
                            OldImageNumber();
                    }
                    PortretImage(_albums);
                }
            }
        }

        private void AlbumWrapPanel_MouseDown(object sender, RoutedEventArgs e)
        {
            var btn = e.Source as Button;
            if (btn != null)
            {
                _oldAlbumNumber = _currentAlbumNumber;
                OldAlbumNumber();
                _currentAlbumNumber = AlbumWrapPanel.Children.IndexOf(btn);
                CurrentAlbumNumber();
                WrapPanelImagesAdd(_albums);
                PortretImage(_albums);
            }
        }

        private void WrapPanelChildren_MouseDown(object sender, RoutedEventArgs e)
        {
            var btn = e.Source as Button;
            if (btn != null)
            {
                _oldImageNumber = _currentImageNumber;
                OldImageNumber();
                _currentImageNumber = WrapPanelImages.Children.IndexOf(btn);
                CurrentImageNumber();
                DockPortret.Children.Clear();
                PortretImage(_albums);
            }
        }

        private void CurrentImageNumber()
        {
            var currentImage = WrapPanelImages.Children[_currentImageNumber] as Control;
            currentImage.BorderBrush = Brushes.Green;
        }

        private void OldImageNumber()
        {
            var oldImage = WrapPanelImages.Children[_oldImageNumber] as Control;
            oldImage.BorderBrush = Brushes.Red;
        }

        private void OldAlbumNumber()
        {
            var oldAlbum = AlbumWrapPanel.Children[_oldAlbumNumber] as Control;
            oldAlbum.BorderBrush = Brushes.Red;
        }

        private void CurrentAlbumNumber()
        {
            var currentAlbum = AlbumWrapPanel.Children[_currentAlbumNumber] as Control;
            currentAlbum.BorderBrush = Brushes.Green;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (WrapPanelImages.Children.Count > 1)
            {
                if (_timerBool)
                {
                    _dispatcherTimer.IsEnabled = false;

                    _timerBool = false;
                    var play = new Button
                    {
                        Content = @"C:\Users\samir\source\repos\WPF\Galery\Galery\Images\play.PNG",
                    };
                    play.Template = FindResource("btnPlay") as ControlTemplate;
                    dockPlay.Children.Clear();
                    dockPlay.Children.Add(play);
                }
                else
                {
                    _timerBool = true;
                    var pause = new Button
                    {
                        Content = @"C:\Users\samir\source\repos\WPF\Galery\Galery\Images\pause.PNG",
                    };
                    pause.Template = FindResource("btnPlay") as ControlTemplate;
                    dockPlay.Children.Clear();
                    dockPlay.Children.Add(pause);

                    _dispatcherTimer.Tick += DTTicker;
                    _dispatcherTimer.IsEnabled = true;
                }
            }
            else
                MessageBox.Show("No photos for slideshow!", "Informathion", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void DTTicker(object sender, EventArgs e)
        {
            if (_currentImageNumber < WrapPanelImages.Children.Count - 1)
            {
                _oldImageNumber = _currentImageNumber;
                _currentImageNumber++;
                OldImageNumber();
                PortretImage(_albums);
            }
        }
    }
}
