using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMM.Common.Configuration
{
    [ConfigurationCollection(typeof(BehaviorConfigurationElement), AddItemName = "behavior")]
    public class BehaviorSectionsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BehaviorConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            BehaviorConfigurationElement section = (BehaviorConfigurationElement)element;

            return section.Type;
        }

    }
}
