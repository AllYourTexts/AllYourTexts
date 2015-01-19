using AllYourTextsUi.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsUi
{
    public class GraphTypeListItem
    {
        public string Description { get; set; }
        public GraphType GraphType { get; set; }

        public GraphTypeListItem()
        {
            // Empty constructor needed to instantiate in XAML.
        }
        
        public GraphTypeListItem(string description, GraphType graphType)
        {
            this.Description = description;
            this.GraphType = graphType;
        }
    }
}
