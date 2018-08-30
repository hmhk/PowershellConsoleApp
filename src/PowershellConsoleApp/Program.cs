using System;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Configuration;

namespace PowershellConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            // create a new instance of the PowerShell class
            PowerShell ps = PowerShell.Create();

            // use "AddScript" to add the contents of a script file to the end of the execution pipeline.
            // use "AddParameter" to add a single parameter to the last command/script on the pipeline.
            ps.AddScript("param($param1) get-childitem -Path $param1 -Attributes !Directory+!ReadOnly -exclude *.dll,*.pdb -recurse -force | Select Name");            
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
                    allLines[i++] = outputItem.BaseObject.ToString();
                }
            }

            */
            //config
            sourceBasePath_appSetting = ConfigurationManager.AppSettings["sourceBasePath"];            
            destBasePath_appSetting = ConfigurationManager.AppSettings["destBasePath"];
            
            if (string.IsNullOrWhiteSpace(sourceBasePath_appSetting) || string.IsNullOrWhiteSpace(destBasePath_appSetting))
            {
                throw new ConfigurationException("app config is error!");
            }
            destBasePath_appSetting += "_new" + DateTime.Now.ToString("yyyyMMdd_hhmmss");

            Process p = new Process();            
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;            
            p.StartInfo.FileName = "cmd.exe";
            //p.StartInfo.Arguments = "/K dir /A:-D-R /s /b | findstr /v /i \"\\.exe$\" | findstr /v  /i \"\\.pdb\"";
            p.StartInfo.Arguments = "/C dir /A:-D-R /s /b | findstr /v /i \"\\.exe$\" | findstr /v  /i \"\\.pdb\"";
            p.Start();

            StringBuilder stdOutput = new StringBuilder();
            while (!p.StandardOutput.EndOfStream)
            {
                string line = p.StandardOutput.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    stdOutput.Append(XCopy(sourceBasePath_appSetting, destBasePath_appSetting,line) + "\n");
                }
            }
            p.Close();

            XCopy xp = new XCopy(stdOutput.ToString());
            Console.ReadLine();
        }

        #region " XCopy "
        
        private static string XCopy(string sourceBasePath_appSetting, string destBasePath_appSetting,string line)
        {
            //string str="xcopy \"{0}\" \"{1}\" /E  \r\n"; //Copies all subdirectories, even if they are empty
            string str = "xcopy \"{0}\" \"{1}\" /S  \r\n"; //Copies directories and subdirectories, unless they are empty

            string relativePath = line.Replace(sourceBasePath_appSetting, "");
            relativePath = relativePath.Substring(0, relativePath.LastIndexOf('\\') + 1);

            string destFolder = Path.Combine(destBasePath_appSetting, relativePath) + "\\";
            return string.Format(str, line.Trim(), destFolder);
        }

        #endregion

        public static string sourceBasePath_appSetting = null;
        public static string destBasePath_appSetting = null;
    }
}
