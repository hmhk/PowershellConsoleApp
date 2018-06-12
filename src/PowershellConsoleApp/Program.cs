using System;
using System.Management.Automation;
using System.Collections.ObjectModel;

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
            foreach (PSObject outputItem in PSOutput)
            {
                // if null object was dumped to the pipeline during the script then a null
                // object may be present here. check for null to prevent potential NRE.
                if (outputItem != null)
                {
                    //TODO: do something with the output item 
                    //Console.WriteLine(outputItem.BaseObject.GetType().FullName);
                    Console.WriteLine(outputItem.BaseObject.ToString() + "\n");
                }
            }
            Console.ReadLine();
        }
    }
}
