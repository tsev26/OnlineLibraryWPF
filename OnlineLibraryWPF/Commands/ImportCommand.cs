using Microsoft.Win32;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class ImportCommand : AsyncCommandBase
    {
        private readonly MongoDBService _mongoDBService;
        private readonly MessageStore _messageStore;

        public ImportCommand(MongoDBService mongoDBService, MessageStore messageStore)
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
            folderBrowser.FileName = "Select folder with JSON collection files to import";
            if ((bool)folderBrowser.ShowDialog())
            {
                string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                string dir = folderPath + "\\";

                if (Directory.Exists(dir))
                {
                    await _mongoDBService.ImportJSONAsync(dir);
                    _messageStore.Message = "Succesfully import collections from " + dir;
                    return;
                }
                _messageStore.Message = "Error while importing";
            }
        }
    }
}
