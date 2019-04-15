using System.IO;
using System.Text;

namespace GenerateDocument.Test.Reporting
{
    public class TestResultUtility
    {
        public static StringBuilder testResultHtmlString;

        private static TestResultUtility instance;

        private TestResultUtility()
        {
          
        }

        public static TestResultUtility Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TestResultUtility();
                    testResultHtmlString = new StringBuilder();
                  
                }

                return instance;
            }
        }


        public void InitializeTestResultString(string TestSuiteName)
        {

            testResultHtmlString.Append("<html><header><title>").Append(TestSuiteName).Append("</title></header><body><table border=\"1\">");
            testResultHtmlString.Append("<tr style=\"background-color:#99CCFF\"><td><b> Test Case </b></td><td><b> Test Result </b></td></tr>");
        }

        public void AddTestPassToTestResultString(string TestMethod, string TestResult)
        {
            //add green color to the background if pass
            testResultHtmlString.Append("<tr style=\"background-color:#33CC33\"><td>").Append(TestMethod).Append("</td><td>").Append(TestResult).Append("</td></tr>");

        }
        public void AddTestFailToTestResultString(string TestMethod, string TestResult)
        {
            //add yellow color to the background if fail
            testResultHtmlString.Append("<tr style=\"background-color:#FFFF00\"><td>").Append(TestMethod).Append("</td><td>").Append(TestResult).Append("</td></tr>");

        }
        public void EndTestResultString()
        {
            testResultHtmlString.Append("</table></body></html>");
        }
        public void WriteToHtmlFile(string filename)
        {
            //Create a file stream
            FileStream file = new FileStream(filename, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(file);
            //writing to the file
            sw.WriteLine(testResultHtmlString.ToString());
            sw.Close();
        }
    }
}