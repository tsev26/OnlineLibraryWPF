using Microsoft.Win32;
using OnlineLibraryWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class SelectImgCommand : CommandBase
    {
        private BookAddEditViewModel _bookAddEditViewModel;

        public SelectImgCommand(BookAddEditViewModel bookAddEditViewModel)
        {
            _bookAddEditViewModel = bookAddEditViewModel;
        }

        public override void Execute(object? parameter)
        {
            OpenFileDialog openFD = new OpenFileDialog();
            openFD.Filter = "jpeg|*.jpg|BMP|*.bmp";

            if ((bool)openFD.ShowDialog())
            {
                Image img = Bitmap.FromFile(openFD.FileName);
                using (var ms = new MemoryStream())
                {
                    img.Save(ms, img.RawFormat);
                    _bookAddEditViewModel.Picture = ms.ToArray();
                }
            }
        }
    }
}
