using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMM.Common.Configuration
{
    [ConfigurationCollection(typeof(MonitorationConfigurationElement), AddItemName = "monitoration")]
    public class MonitorationSectionsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MonitorationConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            MonitorationConfigurationElement section = (MonitorationConfigurationElement)element;

            return section.Path;
        }

    }
}
