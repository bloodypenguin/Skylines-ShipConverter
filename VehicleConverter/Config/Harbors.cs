using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using FerryConverter.OptionsFramework;

namespace FerryConverter.Config
{
    public static class Harbors
    {
        private static readonly Dictionary<HarborCategory, HarborItem[]> _ids = new Dictionary<HarborCategory, HarborItem[]>
        {
            {
                HarborCategory.PassengerShipHarbor, new[]
                {
                    new HarborItem(723742558, "Joak's 4-Track Train Station"),
                }
            },
        };

        private static bool _configIsOverriden;

        public static IEnumerable<HarborItem> GetItems(HarborCategory harborCategory = HarborCategory.All)
        {
            var list = new List<HarborItem>();
            _ids.Where(kvp => (kvp.Key & harborCategory) != 0).Select(kvp => kvp.Value).ForEach(a => list.AddRange(a));
            return list;
        }

        private static Dictionary<HarborCategory, HarborItem[]> Ids  {
            get
            {
                if (_configIsOverriden)
                {
                    return _ids;
                }
                _ids[HarborCategory.PassengerShipHarbor] = OptionsWrapper<Config>.Options.PassengerShipHarbors.Items.ToArray();
                _configIsOverriden = true;
                return _ids;
            }
        }

        public static long[] GetConvertedIds(HarborCategory harborCategory = HarborCategory.All)
        {
            var list = new List<long>();
            Ids.Where(kvp => IsCategoryEnabled(kvp.Key) && (kvp.Key & harborCategory) != 0).Select(kvp => kvp.Value).ForEach(l => l.ForEach(t =>
            {
                if (!t.Exclude)
                {
                    list.Add(t.WorkshopId);
                }
            }));
            return list.ToArray();
        }

        private static bool IsCategoryEnabled(HarborCategory harborCategory)
        {
            switch (harborCategory)
            {
                case HarborCategory.PassengerShipHarbor:
                    return OptionsWrapper<Options>.Options.ConvertPassengerHarborsToFerryStops && Util.DLC(SteamHelper.kMotionDLCAppID);
                default:
                    throw new ArgumentOutOfRangeException(nameof(harborCategory), harborCategory, null);
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

        public static HarborCategory GetCategory(long id)
        {
            return Ids.Keys.FirstOrDefault(cat => Ids[cat].Select(i => i.WorkshopId).Contains(id));
        }
    }
}