using System;

namespace FerryConverter.Config
{
    [Flags]
    public enum ShipCategory
    {
        None = 0,
        PassengerShip = 1,
        Ships = PassengerShip,
        All = Ships
    }
}
