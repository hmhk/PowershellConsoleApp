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
            this.work = new Process();
            work.StartInfo.FileName = cmdHelper(null, args);// "cmd.exe";
            //work.StartInfo.Arguments = " - " + cmdHelper(null, args);
            //work.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            //work.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            //work.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            //work.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            //work.StartInfo.CreateNoWindow = true;//不显示程序窗口

            work.Start();

            //work.StandardInput.WriteLine(cmdHelper(null, args) + "");
            //work.StandardInput.AutoFlush = true;

            //string output = work.StandardOutput.ReadToEnd();

            //work.WaitForExit();//等待程序执行完退出进程
            //work.Close();
            //Log StandardOutput
            //Console.WriteLine(output);
        } 
        #endregion

        #region " cmdHelper "
        private string cmdHelper(string cmdFileName, string text)
        {
            _cmdFileName = cmdFileName;
            if (string.IsNullOrEmpty(_cmdFileName))
            {
                _cmdFileName = "test.cmd";
            }

            File.WriteAllText(_cmdFileName, text);
            return _cmdFileName;
        } 
        #endregion

        #region " Property"

        private Process work;
        private string _cmdFileName; 
        #endregion
    }
}
