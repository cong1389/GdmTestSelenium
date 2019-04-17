﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Internal;

namespace GenerateDocument.Common.Helpers
{
    public static class FilesHelper
    {
        public static ICollection<FileInfo> GetAllFiles(string folder)
        {
            return new DirectoryInfo(folder).GetFiles().ToList();
        }
        public static FileInfo GetFileByName(string folder, string fileName)
        {
            return new DirectoryInfo(folder).GetFiles(fileName).First();
        }

        public static int CountFiles(string folder)
        {
            return GetAllFiles(folder).Count;
        }

        public static void DeleteAllFiles(string filePath)
        {
            var dir = new DirectoryInfo(filePath);
            foreach (var file in dir.GetFiles())
            {
                file.Delete();
            }
        }


    }
}
