using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FerryConverter.Config
{
    [Serializable]
    public class HarborItems
    {
        public HarborItems()
        {
            this.Items = new List<HarborItem>();
        }

        public HarborItems(List<HarborItem> items)
        {
            this.Items = items;
        }

        [XmlElement("items")]
        public List<HarborItem> Items { get; private set; }
    }
}
