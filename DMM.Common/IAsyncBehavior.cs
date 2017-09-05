using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM.Common
{

    /// <summary>
    /// 指示一个异步执行的具体行为.
    /// </summary>
    public interface IAsyncBehavior: IBehavior
    {
        /// <summary>
        /// 开始执行动作
        /// </summary>
        /// <param name="path">受影响的目录</param>
        /// <param name="parameters">参数</param>
        /// <param name="modificationArg">原始事件参数</param>
        /// <param name="callBack">操作回调</param>
        /// <returns>异步状态</returns>
        IAsyncResult BeginDoAction(String path, Dictionary<String, String> parameters, FileSystemEventArgs modificationArg, AsyncCallback callBack);

        /// <summary>
        /// 停止执行动作
        /// </summary>
        /// <param name="rs">异步状态</param>
        /// <returns>操作结果</returns>
        OperationResult EndDoAction(IAsyncResult rs);
    }
}
