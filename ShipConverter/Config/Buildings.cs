using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using FerryConverter.OptionsFramework;

namespace FerryConverter.Config
{
    public static class Buildings
    {
        private static readonly Dictionary<BuildingCategory, BuildingItem[]> _ids = new Dictionary<BuildingCategory, BuildingItem[]>
        {
            {
                BuildingCategory.PassengerShipHarbor, new[]
                {
                    new BuildingItem(625539070, "Small Ferry Terminal"),
                }
            },
            {
                BuildingCategory.ShipBuilding, new[]
                {
                    new BuildingItem(436874500, "Fishing Trawler Ship"),
                    new BuildingItem(438644475, "Aircraft Carrier"),
                    new BuildingItem(439334278, "Color Changing Cruiser - Solid"),
                    new BuildingItem(439326155, "Color Changing Cruiser -Gradient"),
                    new BuildingItem(437889905, "Hasuna WII Cargo Ship"),
                    new BuildingItem(437640993, "Oil Tanker Ship (Park)"),
                    new BuildingItem(437088425, "Tugboat"),
                    new BuildingItem(437037088, "Fishing Vessel"),
                    new BuildingItem(430845670, "50 meter Yacht"), //remove props?
                    new BuildingItem(439558291, "Luxury Yacht - Color Changing"),  //remove props?
                    new BuildingItem(436554307, "british frigate"), 
                    new BuildingItem(448355071, "HarborPack by --VIP-- (building №2 Ship )"), 
                    new BuildingItem(481423357, "Ocean Liner (stationary)"), 
                    new BuildingItem(478150328, "small Motor Boat"), 
                    new BuildingItem(478153907, "USS Missouri Battleship (BETA)"),  //remove props?
                    new BuildingItem(705265644, "The scenery for the port .The cargo ship"), 
                    new BuildingItem(661139666, "USS Missouri (BB-63) Battle Ship"), //remove props?
                    new BuildingItem(563183620, "Ploppable Maersk Line Cargo Ship"), 
                    new BuildingItem(682415206, "BATTLESHIP K.M.S. BISMARK (Beta)"), //remove props? 
                }
            }
        };

        private static bool _configIsOverriden;

        public static IEnumerable<BuildingItem> GetItems(BuildingCategory buildingCategory = BuildingCategory.All)
        {
            var list = new List<BuildingItem>();
            _ids.Where(kvp => (kvp.Key & buildingCategory) != 0).Select(kvp => kvp.Value).ForEach(a => list.AddRange(a));
            return list;
        }

        private static Dictionary<BuildingCategory, BuildingItem[]> Ids  {
            get
            {
                if (_configIsOverriden)
                {
                    return _ids;
                }
                _ids[BuildingCategory.PassengerShipHarbor] = OptionsWrapper<Config>.Options.PassengerShipHarbors.Items.ToArray();
                _ids[BuildingCategory.ShipBuilding] = OptionsWrapper<Config>.Options.FixShipBuildingsShaders.Items.ToArray();
                _configIsOverriden = true;
                return _ids;
            }
        }

        public static long[] GetConvertedIds(BuildingCategory buildingCategory = BuildingCategory.All)
        {
            var list = new List<long>();
            Ids.Where(kvp => IsCategoryEnabled(kvp.Key) && (kvp.Key & buildingCategory) != 0).Select(kvp => kvp.Value).ForEach(l => l.ForEach(t =>
            {
                if (!t.Exclude)
                {
                    list.Add(t.WorkshopId);
                }
            }));
            return list.ToArray();
        }

        private static bool IsCategoryEnabled(BuildingCategory buildingCategory)
        {
            switch (buildingCategory)
            {
                case BuildingCategory.ShipBuilding:
                    return OptionsWrapper<Options>.Options.PatchShipBuildinsShaders;
                case BuildingCategory.PassengerShipHarbor:
                    return OptionsWrapper<Options>.Options.ConvertPassengerHarborsToFerryStops && Util.DLC(SteamHelper.kMotionDLCAppID);
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildingCategory), buildingCategory, null);
            }
        }

        public static bool ToDecoration(long id)
        {
            var list = new List<long>();
            Ids.Select(kvp => kvp.Value).ForEach(l => l.ForEach(t =>
            {
                if (t.ToDecoration)
                {
                    list.Add(t.WorkshopId);
                }
            }));
            return list.Contains(id);
        }

        public static BuildingCategory GetCategory(long id)
        {
            return Ids.Keys.FirstOrDefault(cat => Ids[cat].Select(i => i.WorkshopId).Contains(id));
        }
    }
}