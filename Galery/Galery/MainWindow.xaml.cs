using System;
using WinForm = System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using WinForms = System.Windows.Forms;

namespace Galery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageFilesFilter imageFilesFilter;
        private DirectoryInfo folder;

        public MainWindow()
        {
            InitializeComponent();

            imageFilesFilter = new ImageFilesFilter();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var openFolder = new WinForms.FolderBrowserDialog();
            var result = openFolder.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                folder = new DirectoryInfo(openFolder.SelectedPath);
                if (folder.Exists)
                {
                    foreach (var path in folder.GetFiles())
                    {
                        if (imageFilesFilter.CheckImages(path.ToString()))
                        {
                            WinForms.MessageBox.Show(path.ToString());
                        }
                    }
                }
            }
        }
    }
}
