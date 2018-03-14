using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Galery
{
    abstract class FilesFilter
    {
        protected string[] Filter { get; set; }

        public bool CheckImages(string filename)
        {
            foreach (var etension in Filter)
            {
                if (filename.EndsWith(etension) == true)
                    return true;
            }
            return false;
        }
    }

    class ImageFilesFilter : FilesFilter
    {
        public ImageFilesFilter()
        {
            Filter = new string[] { ".jpg", ".jpeg", ".bmp", ".gif", ".png", ".PNG" };
        }
    }
}
