using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace Galery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageFilesFilter _imageFilesFilter;
        private DirectoryInfo _folder;
        private List<string> _albumImages;
        private int _currentImageNumber;
        private int _oldImageNumber;

        public MainWindow()
        {
            InitializeComponent();

            _imageFilesFilter = new ImageFilesFilter();
            _albumImages = new List<string>();
            _currentImageNumber = 0;
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
                        DockPortret.Children.Add(portret);
                        CurrentImageNumber();
                    }
                }
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentImageNumber != _albumImages.Count - 1)
            {
                _oldImageNumber = _currentImageNumber;
                _currentImageNumber++;
                DeleteDockChild();
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
                DeleteDockChild();
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
            DeleteDockChild();
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

        private void DeleteDockChild()
        {
            if (DockPortret.Children.Count != 0)
                DockPortret.Children.Remove(DockPortret.Children[0]);
        }
    }
}
