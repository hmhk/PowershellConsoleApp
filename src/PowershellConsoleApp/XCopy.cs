using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace PowershellConsoleApp
{
    public class XCopy
    {
        #region " Construct "
        public XCopy(string args)
        {
            Process work = new Process();
            work.StartInfo.FileName = cmdHelper(null, args);
            
            work.Start();
        } 
        #endregion

        #region " cmdHelper "
        private string cmdHelper(string cmdFileName, string text)
        {
            if (string.IsNullOrEmpty(cmdFileName))
            {
                cmdFileName = "test.cmd";
            }

            File.WriteAllText(cmdFileName, text);
            return cmdFileName;
        } 
        #endregion

        #region " Property"

        #endregion
    }
}
