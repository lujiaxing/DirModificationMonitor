using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM.Common
{
    /// <summary>
    /// 操作结果
    /// </summary>
    public enum OperationResult: byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 文件已被覆盖
        /// </summary>
        FileConvered = 1,

        /// <summary>
        /// 目标拒绝访问
        /// </summary>
        AccessForbidden = 2,

        /// <summary>
        /// IO错误
        /// </summary>
        IOError = 3,

        /// <summary>
        /// 文件被锁定, 不能写入.
        /// </summary>
        FileLocked = 4,

        /// <summary>
        /// 读取文件错误
        /// </summary>
        FailedToOpen = 5,

        /// <summary>
        /// 参数错误
        /// </summary>
        InvalidArgument = 6,

        /// <summary>
        /// 不予处理
        /// </summary>
        DoNothing = 7,

        /// <summary>
        /// 其它错误
        /// </summary>
        Other = 8

    }
}
