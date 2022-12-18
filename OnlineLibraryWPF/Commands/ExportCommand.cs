using Microsoft.Win32;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Stores;
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
    public class ExportCommand : AsyncCommandBase
    {
        private readonly MongoDBService _mongoDBService;
        private readonly MessageStore _messageStore;

        public ExportCommand(MongoDBService mongoDBService, MessageStore messageStore)
        {
            _mongoDBService = mongoDBService;
            _messageStore = messageStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            folderBrowser.FileName = "Folder Selection.";
            if ((bool)folderBrowser.ShowDialog())
            {
                string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                string newFolder = "export_" + DateTime.Now.ToString("yyyy-MM-dd_HHmm");
                string dir = folderPath + "\\" + newFolder + "\\";

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    await _mongoDBService.ExportJSONAsync(dir);
                    _messageStore.Message = "Succesfully exported into " + dir;
                    return;
                }
                _messageStore.Message = "Error while exporting";
            }
        }
    }
}
