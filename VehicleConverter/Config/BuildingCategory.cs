using System;

namespace FerryConverter.Config
{
    [Flags]
    public enum BuildingCategory
    {
        None = 0,
        PassengerShipHarbor = 1,
        ShipBuilding = 2,
        All = PassengerShipHarbor | ShipBuilding
    }
}
