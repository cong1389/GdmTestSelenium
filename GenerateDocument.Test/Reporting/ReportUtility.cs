using System.Diagnostics;

namespace GenerateDocument.Test.Reporting
{
    public class ReportUtility
    {
        private static ReportUtility instance;

        private ReportUtility()
        {
        }

        public static ReportUtility Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReportUtility();
                }

                return instance;
            }
        }

        public string GetMethodName(int level)
        {
            var stackTrace = new StackTrace();
            var methodBase = stackTrace.GetFrame(level).GetMethod();
            var Class = methodBase.ReflectedType;
            //var Namespace = Class.Namespace;
            return Class.Name + "." + methodBase.Name;
        }

        public string GetClassName(int level)
        {
            var stackTrace = new StackTrace();
            var methodBase = stackTrace.GetFrame(level).GetMethod();
            var Class = methodBase.ReflectedType;
            //var Namespace = Class.Namespace;
            return Class.Name;
        }
    }
}
