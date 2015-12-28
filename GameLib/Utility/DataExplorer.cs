using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Utility
{
    class ExplorableAttribute : Attribute { }

    public class DataExplorer
    {
        public DataExplorer(object root)
        {
            var matches = root.GetType().GetProperties();
                // world, questInstantiator, profile, cards, bag, 
            // Scan tree
            // Just use ToString()?
        }
    }
}
