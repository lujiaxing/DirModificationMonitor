using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DMM.Common.Configuration
{
    /// <summary>
    /// 操作项
    /// </summary>
    public class BehaviorConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// 操作项指向的具体行为执行类的类型
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public String Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }

        /// <summary>
        /// 包含操作项指向的具体行为执行类的DLL
        /// </summary>
        [ConfigurationProperty("file", IsRequired = true)]
        public String AssemblyFile
        {
            get
            {
                return (string)this["file"];
            }
            set
            {
                this["file"] = value;
            }
        }


        /// <summary>
        /// 该操作项是否生效
        /// </summary>
        [ConfigurationProperty("enable", IsRequired = false, DefaultValue = true)]
        public bool Enable
        {
            get
            {
                return (bool)this["enable"];
            }
            set
            {
                this["enable"] = value;
            }
        }


        /// <summary>
        /// 指示该行为是否为异步
        /// </summary>
        [ConfigurationProperty("isAsync", IsRequired = false, DefaultValue = false)]
        public bool IsAsync
        {
            get
            {
                return (bool)this["isAsync"];
            }

            set
            {
                this["isAsync"] = value;

            }
        }

        /// <summary>
        /// 操作项参数
        /// </summary>
        [ConfigurationProperty("parameters", IsRequired = false)]
        public BehaviorParametersCollection Parameters
        {
            get { return this["parameters"] as BehaviorParametersCollection; }
            set { this["parameters"] = value; }
        }

        

    }


}
