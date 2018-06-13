using System;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.IO;

namespace PowershellConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // create a new instance of the PowerShell class
            PowerShell ps = PowerShell.Create();

            // use "AddScript" to add the contents of a script file to the end of the execution pipeline.
            // use "AddParameter" to add a single parameter to the last command/script on the pipeline.
            ps.AddScript("param($param1) get-childitem -Path $param1 -Attributes !Directory+!ReadOnly -exclude *.dll,*.pdb -recurse -force");            
            ps.AddParameter("param1", @""); 

            // invoke execution on the pipeline (collecting output)
            Collection<PSObject> PSOutput = ps.Invoke();
            string[] allLines = new string[PSOutput.Count];
            int i = 0;
            foreach (PSObject outputItem in PSOutput)
            {
                // if null object was dumped to the pipeline during the script then a null
                // object may be present here. check for null to prevent potential NRE.
                if (outputItem != null)
                {
                    //TODO: do something with the output item 
                    //Console.WriteLine(outputItem.BaseObject.GetType().FullName);
                    //Console.WriteLine(outputItem.BaseObject.ToString() + "\n");
                    allLines[i++] = outputItem.BaseObject.ToString();
                }
            }
            XCopy(allLines);
            Console.ReadLine();
        }

        #region " XCopy "
        private static void XCopy(string[] AllLines)
        {
            //string[] AllLines = new string[1];
            string sourceBasePath = @"D:\Project\ASP.NET\VS2010\AdampakErp\ErpWebUi\";
            string sourceBasePath_appSetting = "";// ConfigurationManager.AppSettings["sourceBasePath"];
            if (!string.IsNullOrWhiteSpace(sourceBasePath_appSetting))
            {
                sourceBasePath = sourceBasePath_appSetting;
            }
            string destBasePath = @"C:\Users\Administrator\Desktop\new_" + DateTime.Now.ToString("yyyyMMdd_mmss") ;// this._TempText;

            //string str="xcopy \"{0}\" \"{1}\" /E  \r\n"; //Copies all subdirectories, even if they are empty
            string str = "xcopy \"{0}\" \"{1}\" /S  \r\n"; //Copies directories and subdirectories, unless they are empty

            string dest = "";
            string re = "";

            foreach (var s in AllLines)
            {
                string temp = s.Replace(sourceBasePath, "");
                temp = temp.Substring(0, temp.LastIndexOf('\\') + 1);//保留"\"
                dest = Path.Combine(destBasePath, temp);
                re += string.Format(str, s.Trim(), dest);
            }
            XCopy xp = new XCopy(re);
        }

        #endregion
    }
}
