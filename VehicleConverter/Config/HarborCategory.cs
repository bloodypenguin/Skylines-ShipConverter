using System;

namespace FerryConverter.Config
{
    [Flags]
    public enum HarborCategory
    {
        None = 0,
        PassengerShipHarbor = 1,
        All = PassengerShipHarbor
    }
}
