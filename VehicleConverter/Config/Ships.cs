using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using FerryConverter.OptionsFramework;

namespace FerryConverter.Config
{
    public static class Ships
    {
        private static readonly Dictionary<ShipCategory, ShipItem[]> _ids = new Dictionary<ShipCategory, ShipItem[]>
        {
            {
                ShipCategory.PassengerShip, new[]
                {
                    new ShipItem(456366973, "Catamaran Ferry (Passenger Ship)"), 
                    new ShipItem(476945352, "Ferry Ship"),
                    new ShipItem(919952804, "StarFerry (Remake Version)"),
                    new ShipItem(773591637, "Passenger ferry - M/S Pernille - Sundbusserne"), 
                    new ShipItem(667891104, "Small Ship"), 
                    new ShipItem(467325398, "Steam Ship"), 
                    new ShipItem(463237025, "River Ship"), 
                }
            },
        };

        private static bool _configIsOverriden;

        public static IEnumerable<ShipItem> GetItems(ShipCategory shipCategory = ShipCategory.All)
        {
            var list = new List<ShipItem>();
            _ids.Where(kvp => (kvp.Key & shipCategory) != 0).Select(kvp => kvp.Value).ForEach(a => list.AddRange(a));
            return list;
        }

        private static Dictionary<ShipCategory, ShipItem[]> Ids  {
            get
            {
                if (_configIsOverriden)
                {
                    return _ids;
                }
                _ids[ShipCategory.PassengerShip] = OptionsWrapper<Config>.Options.PassengerShips.Items.ToArray();
                _configIsOverriden = true;
                return _ids;
            }
        }

        public static long[] GetConvertedIds(ShipCategory shipCategory = ShipCategory.All)
        {
            var list = new List<long>();
            Ids.Where(kvp => IsCategoryEnabled(kvp.Key) && (kvp.Key & shipCategory) != 0).Select(kvp => kvp.Value).ForEach(l => l.ForEach(t =>
            {
                if (!t.Exclude)
                {
                    list.Add(t.WorkshopId);
                }
            }));
            return list.ToArray();
        }

        private static bool IsCategoryEnabled(ShipCategory shipCategory)
        {
            switch (shipCategory)
            {
                case ShipCategory.PassengerShip:
                    return OptionsWrapper<Options>.Options.ConvertPassengerShipsToFerries && Util.DLC(SteamHelper.kMotionDLCAppID) ;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shipCategory), shipCategory, null);
            }
        }
    }
}