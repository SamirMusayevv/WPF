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
        private int _currentImageNumber;
        private int _oldImageNumber;
        private bool _timerBool;
        private DispatcherTimer _dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();

            _imageFilesFilter = new ImageFilesFilter();
            _albums = new List<List<string>>();
            _albumImages = new List<string>();
            _currentImageNumber = 0;
            _timerBool = false;
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            _dispatcherTimer.IsEnabled = false;
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
                    foreach (var path in _folder.GetFiles())
                    {
                        if (_imageFilesFilter.CheckImages(path.ToString()))
                        {
                            var fullPath = _folder.FullName + "\\" + path.ToString();
                            _albumImages.Add(fullPath);
                        }
                    }
                    _albums.Add(_albumImages);
                    WrapPanelChildren.Children.Clear();
                    WrapPanelChildrenAdd(_albumImages);
                    PortretImage(_albumImages);
                }
            }
        }

        private void WrapPanelChildrenAdd(List<string> paths)
        {
            if (paths != null)
            {
                foreach (var path in paths)
                {
                    var btn = new Button
                    {
                        BorderBrush = Brushes.Red,
                        Content = path,
                        Width = 150,
                        Height = 100
                    };
                    btn.Template = FindResource("ImgButton") as ControlTemplate;
                    WrapPanelChildren.Children.Add(btn);
                }
            }
        }

        private void PortretImage(List<string> paths)
        {
            if (paths != null)
            {
                for (int i = 0; i < paths.Count; i++)
                {
                    if (i == _currentImageNumber)
                    {
                        var portret = new Button
                        {
                            Content = paths[_currentImageNumber]
                        };
                        portret.Template = FindResource("ImgPortret") as ControlTemplate;
                        CurrentImageNumber();
                        DockPortret.Children.Clear();
                        DockPortret.Children.Add(portret);
                    }
                }
            }
        }

        private void ButtonFolder_Click(object sender, RoutedEventArgs e)
        {
            if (_albums.Count != 0)
            {
                for (int i = 0; i < _albums.Count; i++)
                {
                    var lbl = new Button
                    {
                        Height = 50,
                        Width = 100,
                        BorderBrush = Brushes.Green,
                        Content = $"Album{i}"
                    };
                    lbl.Template = FindResource("AlbumBtn") as ControlTemplate;
                    AlbumWrapPanel.Children.Add(lbl);
                }
                //foreach (var album in _albums)
                //{
                //    var lbl = new Button
                //    {
                //        Height = 50,
                //        Width = 50,
                //        BorderBrush = Brushes.Green,
                //        Content = @"Album{}"
                //    };
                //    lbl.Template = FindResource("AlbumBtn") as ControlTemplate;
                //    AlbumWrapPanel.Children.Add(lbl);
                //}
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
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
                PortretImage(_albumImages);
            }
        }

        private void ButtonLast_Click(object sender, RoutedEventArgs e)
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
                PortretImage(_albumImages);
            }
        }

        private void WrapPanelChildren_MouseDown(object sender, RoutedEventArgs e)
        {
            var btn = e.Source as Button;
            _oldImageNumber = _currentImageNumber;
            OldImageNumber();
            _currentImageNumber = WrapPanelChildren.Children.IndexOf(btn);
            CurrentImageNumber();
            DockPortret.Children.Clear();
            PortretImage(_albumImages);
        }

        private void CurrentImageNumber()
        {
            var currentImage = WrapPanelChildren.Children[_currentImageNumber] as Control;
            currentImage.BorderBrush = Brushes.Green;
        }

        private void OldImageNumber()
        {
            var oldImage = WrapPanelChildren.Children[_oldImageNumber] as Control;
            oldImage.BorderBrush = Brushes.Red;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (WrapPanelChildren.Children.Count > 1)
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
            if (_currentImageNumber < _albumImages.Count - 1)
            {
                _oldImageNumber = _currentImageNumber;
                _currentImageNumber++;
                PortretImage(_albumImages);
                OldImageNumber();
            }
        }
    }
}
