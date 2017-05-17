using System.Xml.Serialization;
using FerryConverter.OptionsFramework.Attibutes;

namespace FerryConverter
{
    [Options("FerryConverter-Options")]
    public class Options
    {
        private const string FERRIES = "Trains to trams - Require Snowfall DLC";
        private const string HARBORS = "Passenger Ship Harbors to Ferry Stops";

        public Options()
        {
            ConvertPassengerShipsToFerries = true;
            ConvertPassengerHarborsToFerryStops = true;
        }

        [XmlElement("convert-passenger-ships-to-ferries")]
        [Checkbox("Convert some passenger ships to Mass Transit ferries", null, null, FERRIES)]
        public bool ConvertPassengerShipsToFerries { set; get; }

        [XmlElement("convert-passenger-harbors-to-ferry-stops")]
        [Checkbox("Convert some passenger ship harbors to ferry stops", null, null, HARBORS)]
        public bool ConvertPassengerHarborsToFerryStops { set; get; }
    }
}