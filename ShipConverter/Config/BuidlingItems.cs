using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FerryConverter.Config
{
    [Serializable]
    public class BuidlingItems
    {
        public BuidlingItems()
        {
            this.Items = new List<BuildingItem>();
        }

        public BuidlingItems(List<BuildingItem> items)
        {
            this.Items = items;
        }

        [XmlElement("items")]
        public List<BuildingItem> Items { get; private set; }
    }
}
