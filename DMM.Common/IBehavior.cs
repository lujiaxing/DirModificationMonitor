using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM.Common
{

    /// <summary>
    /// 指示一个具体的行为.
    /// </summary>
    public interface IBehavior
    {
        /// <summary>
        /// 执行实际操作
        /// </summary>
        /// <param name="path">受影响的目录</param>
        /// <param name="parameters">参数</param>
        /// <param name="modificationArg">原始事件参数</param>
        /// <returns>操作结果</returns>
        OperationResult DoAction(String path, Dictionary<String, String> parameters, FileSystemEventArgs modificationArg);
    }
}
