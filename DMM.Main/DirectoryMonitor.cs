using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DMM.Common;

namespace DMM
{
    /// <summary>
    /// 目录监视者
    /// </summary>
    public class DirectoryMonitor: IDisposable
    {
        private FileSystemSafeWatcher watcher;

        /// <summary>
        /// 需要监视的目录
        /// </summary>
        public String DirectoryToMonitor
        {
            get;
            set;
        }

        /// <summary>
        /// 获取文件侦听器对象
        /// </summary>
        public FileSystemSafeWatcher Watcher
        {
            get
            {
                return watcher;
            }
        }

        /// <summary>
        /// 仅监视当前目录
        /// </summary>
        public Boolean CurrentLevelOnly
        {
            get;
            set;
        }

        /// <summary>
        /// 可用的操作集合
        /// </summary>
        public BehaviorsWapper[] Behaviors
        {
            get;
            set;
        }

        /// <summary>
        /// 初始化监听器
        /// </summary>
        public void Initialize()
        {
            watcher = new FileSystemSafeWatcher();

            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Path = DirectoryToMonitor;
            watcher.IncludeSubdirectories = !CurrentLevelOnly;

            watcher.Changed += (sender, e) =>
            {
                Logger.Instance.WriteInfoLog(String.Format("检测到文件夹修改. Type = {0}, Dir = {1}, FullPath = {2}", e.ChangeType.ToString(), DirectoryToMonitor, e.FullPath));

                for (byte i = 0; i < Behaviors.Length; i++)
                {
                    var behavior = Behaviors[i];

                    if (!behavior.Enable)
                        continue;

                    Dictionary<String, String> copiedArgCollection = new Dictionary<string, string>();

                    foreach (var pair in behavior.Parameters)
                        copiedArgCollection[pair.Key] = behavior.Parameters[pair.Key]
                            .Replace("$TARGET_FILE$", e.FullPath)
                            .Replace("&lt;", "<")
                            .Replace("&gt;", ">")
                            .Replace("&quot;", "\"")
                            .Replace("&nbsp;", " ");

                    try
                    {
                        if (behavior.IsAsync)
                        {
                            IAsyncBehavior s = (IAsyncBehavior)behavior.Behavior;

                            s.BeginDoAction(DirectoryToMonitor, copiedArgCollection, e, AsyncCallBack);

                            Logger.Instance.WriteInfoLog(String.Format("异步执行 {0}", behavior.Behavior));
                        }
                        else
                        {
                            var rs = behavior.Behavior.DoAction(DirectoryToMonitor, copiedArgCollection, e);

                            Logger.Instance.WriteInfoLog(String.Format("执行动作 {0} 的结果: {1}", behavior.Behavior, rs.ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.WriteErrorLog(String.Format("执行针对 {0} 的 {1} 执行响应动作 {2} 时发生错误: {3}.\n调用堆栈:\n{4}",
                            DirectoryToMonitor, e.ChangeType.ToString(), behavior.Behavior, ex.Message, ex.StackTrace));

                        continue;
                    }
                }
            };

            watcher.EnableRaisingEvents = true;

            
        }

        private void AsyncCallBack(IAsyncResult rs)
        {
            IAsyncBehavior b = (IAsyncBehavior)rs.AsyncState;

            var d = b.EndDoAction(rs);

            Logger.Instance.WriteInfoLog(String.Format("异步执行动作 {0} 的结果: {1}", b, d.ToString()));
        }




        public void Dispose()
        {
            if (watcher != null)
                watcher.Dispose();
        }
    }
}
