using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DMM.Common.Configuration
{
    public class MonitorationConfigurationElement : ConfigurationElement
    {
       
        [ConfigurationProperty("path", IsRequired = true)]
        public String Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }

        [ConfigurationProperty("currentLevelOnly", IsRequired = false, DefaultValue = false)]
        public Boolean CurrentLevelOnly
        {
            get
            {
                return (Boolean)this["currentLevelOnly"];
            }
            set
            {
                this["currentLevelOnly"] = value;
            }
        }

      

        [ConfigurationProperty("behaviors", IsRequired = false)]
        public BehaviorSectionsCollection Behaviors
        {
            get { return this["behaviors"] as BehaviorSectionsCollection; }
            set { this["behaviors"] = value; }
        }

        

    }


}
