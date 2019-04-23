using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument.Test.DataDriven
{
    public static class TestData
    {
        public static IEnumerable Credentials
        {
            get { return DataDrivenHelper.ReadDataDriveFile(@"D:\RM\GdmTestSelenium\GenerateDocument.Test\DataDriven\DataDriven.xml"
                , "credential"
                , new[] { "user", "password" }
                , "AutoSave"); }
        }
    }
}
