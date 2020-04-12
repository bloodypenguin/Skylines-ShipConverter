using System.Xml.Serialization;
using FerryConverter.OptionsFramework.Attibutes;

namespace FerryConverter
{
    [Options("FerryConverter-Options")]
    public class Options
    {
        private const string FERRIES = "Ships to ferries - Require Mass Transit DLC";
        private const string BUILDINGS = "Ship park building patch";

        public Options()
        {
            ConvertPassengerShipsToFerries = true;
            ConvertPassengerHarborsToFerryStops = true;
            PatchShipBuildinsShaders = true;
        }

        [XmlElement("convert-passenger-ships-to-ferries")]
        [Checkbox("Convert some passenger ships to Mass Transit ferries", null, null, FERRIES)]
        public bool ConvertPassengerShipsToFerries { set; get; }

        [XmlElement("convert-passenger-harbors-to-ferry-stops")]
        [Checkbox("Convert some passenger ship harbors to ferry stops (Not implemented)", null, null, FERRIES)]
        public bool ConvertPassengerHarborsToFerryStops { set; get; }

        [XmlElement("patch-ship-buildings-shaders")]
        [Checkbox("Patch shaders of some ship buildings so that they could actually float", null, null, BUILDINGS)]
        public bool PatchShipBuildinsShaders { set; get; }
    }
}