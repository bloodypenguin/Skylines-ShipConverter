using System.Linq;
using System.Xml.Serialization;
using FerryConverter.OptionsFramework.Attibutes;

namespace FerryConverter.Config
{
    [Options("FerryConverter-Config")]
    public class Config
    {
        public Config()
        {
            PassengerShips = new ShipItems(Ships.GetItems(ShipCategory.PassengerShip).OrderBy(i => i.WorkshopId).ToList());
            PassengerShipHarbors = new HarborItems(Harbors.GetItems(HarborCategory.PassengerShipHarbor).OrderBy(i => i.WorkshopId).ToList());
        }

        [XmlElement("version")]
        public int Version { get; set; }
        [XmlElement("passenger-ships-to-ferries")]
        public ShipItems PassengerShips { get; private set; }
        [XmlElement("passenger-ship-harborss-to-ferry-stops")]
        public HarborItems PassengerShipHarbors { get; private set; }
    }
}
