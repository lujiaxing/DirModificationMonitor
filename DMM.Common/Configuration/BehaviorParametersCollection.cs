using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMM.Common.Configuration
{
    /// <summary>
    /// 行为参数集合
    /// </summary>
    [ConfigurationCollection(typeof(BehaviorParameter), AddItemName = "parameter")]
    public class BehaviorParametersCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BehaviorParameter();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            BehaviorParameter section = (BehaviorParameter)element;

            return section.Key;
        }

    }
}
