using DMM.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM
{
    public class CommandBehavior : IBehavior
    {
        public OperationResult DoAction(string path, Dictionary<string, string> parameters, System.IO.FileSystemEventArgs modificationArg)
        {
            if(!parameters.ContainsKey("command"))
                return OperationResult.InvalidArgument;

            Process pocess = new Process();
            pocess.StartInfo.FileName = "%windir%\\System32\\cmd.exe";
            pocess.StartInfo.Arguments = String.Format("/c \"{0}\"", parameters["command"].Replace("$TARGET_FILE$", modificationArg.FullPath));

            pocess.Start();

            return OperationResult.Success;
        }

    }
}
