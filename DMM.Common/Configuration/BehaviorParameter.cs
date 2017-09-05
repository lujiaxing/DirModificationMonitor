using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DMM.Common.Configuration
{
    /// <summary>
    /// ���������
    /// </summary>
    public class BehaviorParameter : ConfigurationElement
    {

        /// <summary>
        /// �����������
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// <summary>
        /// ���������ֵ
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

    }
}
