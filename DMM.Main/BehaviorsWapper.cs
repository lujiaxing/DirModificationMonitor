using DMM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMM
{
    public class BehaviorsWapper
    {
        public Dictionary<String, String> Parameters
        {
            get;
            internal set;
        }

        public Boolean Enable
        {
            get;
            internal set;
        }

        public Boolean IsAsync
        {
            get;
            internal set;
        }

        public IBehavior Behavior
        {
            get;
            internal set;
        }
    }
}
