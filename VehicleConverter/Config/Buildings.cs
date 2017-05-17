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