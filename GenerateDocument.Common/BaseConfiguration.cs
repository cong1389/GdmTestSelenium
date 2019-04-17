using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static string NewAppTestDir
        {
            get { return ConfigurationManager.AppSettings["newAppTestDir"]; }
        }
    }
}
