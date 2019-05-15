using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

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

        public static string ExtractTextFromPdf(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("File path must be not null");
            }

            var sbTexts = new StringBuilder();

            using (PdfReader reader = new PdfReader(filePath))
            {

                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    var text = PdfTextExtractor.GetTextFromPage(reader, page, strategy);
                    sbTexts.Append(text);
                }
            }

            return sbTexts.ToString();
        }


    }
}
