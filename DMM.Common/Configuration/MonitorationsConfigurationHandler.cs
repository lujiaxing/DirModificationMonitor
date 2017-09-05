using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM.Common.Configuration
{

    /// <summary>
    /// 文件夹监视
    /// </summary>
    public class MonitorationsConfigurationHandler : ConfigurationSection
    {

        /// <summary>
        /// 监视项
        /// </summary>
        [ConfigurationProperty("monitorations")]
        public MonitorationSectionsCollection Monitorations
        {
            get { return this["monitorations"] as MonitorationSectionsCollection; }
            set { this["monitorations"] = value; }
        }

    }
}
