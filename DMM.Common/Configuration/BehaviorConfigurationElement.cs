using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DMM.Common.Configuration
{
    /// <summary>
    /// ������
    /// </summary>
    public class BehaviorConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// ������ָ��ľ�����Ϊִ���������
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
        /// ����������ָ��ľ�����Ϊִ�����DLL
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
        /// �ò������Ƿ���Ч
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
        /// ָʾ����Ϊ�Ƿ�Ϊ�첽
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
        /// ���������
        /// </summary>
        [ConfigurationProperty("parameters", IsRequired = false)]
        public BehaviorParametersCollection Parameters
        {
            get { return this["parameters"] as BehaviorParametersCollection; }
            set { this["parameters"] = value; }
        }

        

    }


}
