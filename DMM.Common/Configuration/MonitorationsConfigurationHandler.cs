using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM.Common.Configuration
{

    /// <summary>
    /// �ļ��м���
    /// </summary>
    public class MonitorationsConfigurationHandler : ConfigurationSection
    {

        /// <summary>
        /// ������
        /// </summary>
        [ConfigurationProperty("monitorations")]
        public MonitorationSectionsCollection Monitorations
        {
            get { return this["monitorations"] as MonitorationSectionsCollection; }
            set { this["monitorations"] = value; }
        }

    }
}
