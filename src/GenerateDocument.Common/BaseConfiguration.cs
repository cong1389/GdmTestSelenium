using System;
using System.Configuration;
using System.IO;
using Convert = System.Convert;

namespace GenerateDocument.Common
{
    public static class BaseConfiguration
    {
        public static double LongTimeout
        {
            get
            {
                return Convert.ToDouble(ConfigurationManager.AppSettings["longTimeout"]);

            }
        }

        public static double MiddleTimeout
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["middleTimeout"]); }
        }

        public static double ShortTimeout
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["shortTimeout"]); }
        }

        public static string DownloadFolder
        {
            get
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["downloadFolderName"]);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
    }
}
