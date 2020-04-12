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
            PassengerShipHarbors = new BuidlingItems(Buildings.GetItems(BuildingCategory.PassengerShipHarbor).OrderBy(i => i.WorkshopId).ToList());
            FixShipBuildingsShaders = new BuidlingItems(Buildings.GetItems(BuildingCategory.ShipBuilding).OrderBy(i => i.WorkshopId).ToList());
        }

        [XmlElement("version")]
        public int Version { get; set; }
        [XmlElement("passenger-ships-to-ferries")]
        public ShipItems PassengerShips { get; private set; }
        [XmlElement("passenger-ship-harbors-to-ferry-stops")]
        public BuidlingItems PassengerShipHarbors { get; private set; }
        [XmlElement("fix-ship-buildings-shaders")]
        public BuidlingItems FixShipBuildingsShaders { get; private set; }
    }
}
