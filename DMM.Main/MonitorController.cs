using DMM.Common;
using DMM.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DMM
{
    public static class MonitorController
    {
        static DirectoryMonitor[] monitiors;

        public static DirectoryMonitor[] Monitors
        {
            get
            {
                return monitiors;
            }
        }

        public static void Init()
        {
            UnloadMonitors();

            monitiors = null;

            monitiors = GetMonitorationSettings();

            InitMonitors();
        }

        private static void InitMonitors()
        {
            foreach(var m in monitiors)
                m.Initialize();
        }

        private static void UnloadMonitors()
        {
            if (monitiors == null)
                return;


            foreach (var m in monitiors)
                m.Dispose();
        }

        /// <summary>
        /// 获取配置文件中的监视设置
        /// </summary>
        /// <returns></returns>
        private static DirectoryMonitor[] GetMonitorationSettings()
        {
            //读取配置文件. 加载根部: dirMonitor
            MonitorationsConfigurationHandler s = (MonitorationsConfigurationHandler)ConfigurationManager.GetSection("dirMonitor");
            List<DirectoryMonitor> dms = new List<DirectoryMonitor>();

            //读取单个监视设置
            foreach(var m in s.Monitorations)
            {
                MonitorationConfigurationElement elementM = (MonitorationConfigurationElement)m;
                DirectoryMonitor dm = new DirectoryMonitor();

                dm.CurrentLevelOnly = elementM.CurrentLevelOnly;
                //监视目标
                dm.DirectoryToMonitor =
                    elementM.Path.EndsWith("\\") ? elementM.Path.Remove(elementM.Path.Length - 1, 1) : elementM.Path;

                //行为列表
                List<BehaviorsWapper> bws = new List<BehaviorsWapper>();

                //读取处理行为
                foreach (var b in elementM.Behaviors)
                {
                    BehaviorsWapper w = new BehaviorsWapper();

                    BehaviorConfigurationElement elementB = (BehaviorConfigurationElement)b;

                    //读取到处理行为类类型, 和所在DLL加载起来.
                    //首先尝试直接反射出一个实例. 若失败, 则
                    //尝试手动加载程序集后再创建实例.
                    String t = elementB.Type;


                    try
                    {
                        w.Behavior = (IBehavior)Activator.CreateInstance(Type.GetType(t, true, true));
                    }
                    catch (FileNotFoundException)
                    {
                        Assembly a = Assembly.LoadFrom(elementB.AssemblyFile);
                        w.Behavior = (IBehavior)a.CreateInstance(t);
                    }

                    
                    //--------------------------------------------

                    
                    w.Enable = elementB.Enable;
                    w.IsAsync = elementB.IsAsync;
                    
                    Dictionary<String, String> parameters = new Dictionary<string, string>();

                    //处理行为参数
                    foreach(var c in elementB.Parameters)
                    {

                        BehaviorParameter elementP = (BehaviorParameter)c;

                        parameters.Add(elementP.Key, elementP.Value);

                    }

                    //将参数填回
                    w.Parameters = parameters;

                    bws.Add(w);
                }

                dm.Behaviors = bws.ToArray();
                dms.Add(dm);
            }

            return dms.ToArray();

            
        }
    }
}
