﻿using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using VehicleConverter.OptionsFramework;

namespace VehicleConverter.Config
{
    public static class Stations
    {
        private static readonly Dictionary<StationCategory, StationItem[]> _ids = new Dictionary<StationCategory, StationItem[]>
        {
            {
                StationCategory.Modern, new[]
                {
                    new StationItem(723742558, "Joak's 4-Track Train Station"),
                    new StationItem(720837956, "EDSA MRT3 Philippines"),
                    new StationItem(433435357, "Modern Train Station"),
                    new StationItem(701048320, "train station Thorildsplan"),
                    new StationItem(618580477, "City Station"),
                    new StationItem(658612506, "2-Track Elevated Station With Pedestrian deck"),
                    new StationItem(564051384, "2-Track Elevated Station"),
                    new StationItem(532551655, "4-TracksElevatedStation Plain"),
                    new StationItem(532551461, "4-Tracks Elevated Station"),
                    new StationItem(529560650, "Urban Elevated Station NoAD"),
                    new StationItem(527697251, "Urban Elevated Station"),
                    new StationItem(524975362, "Medium Elevated Station"),
                    new StationItem(522406139, "Industrial Elevated Station"),
                    new StationItem(532099566, "Elevated Double Track Train Station"),
                    new StationItem(665292636, "Skytrain station"),
                    new StationItem(538157066, "Sunken Train Station (Concrete)"),
                    new StationItem(536893383, "Sunken Train Station (Brick)"),
                }
            },
            {
                StationCategory.Old, new[]
                {
                    new StationItem(550104018, "NYC_Elevated Stacked Train Station"),
                    new StationItem(552914350, "NYC Elevated Station Over Road 2"),
                    new StationItem(519519752, "Elevated Train Stop (Read Description pls)"),
                    new StationItem(665774022, "Single Brick Station with central platform"),
                    new StationItem(665788204, "Double Brick Station with central platforms"),
                    new StationItem(670379488, "Single Concrete Station with central platform"),
                    new StationItem(670381051, "Double Concrete Station with central platforms"),
                }
            },
            {
                StationCategory.Tram, new[]
                {
                    new StationItem(587989905, "[XIRI] Advanced Tram Station"),
                    new StationItem(541850201, "Tram Station"),
                    new StationItem(542504491, "Tram Station 2"),
                    new StationItem(542877830, "Antwerp Large Station 02"),
                    new StationItem(542877989, "[TRAM] Antwerp Large Station 01"),
                    new StationItem(571249327, "FancyTrack B"),
                    new StationItem(571248983, "FancyTrack A"),
                    new StationItem(542877628, "[TRAM] Antwerp Large track 01"),
                    new StationItem(542877135, "[TRAM] Antwerp Large track 04"),
                    new StationItem(542877338, "[TRAM] Antwerp Large track 03"),
                    new StationItem(542877508, "[TRAM] Antwerp Large track 02"),
                    new StationItem(544693280, "[XIRI] LTN Station [In-Avenue Station]"),
                }
            },
        };

        private static bool _configIsOverriden;

        public static IEnumerable<StationItem> GetItems(StationCategory StationCategory = StationCategory.All)
        {
            var list = new List<StationItem>();
            _ids.Where(kvp => (kvp.Key & StationCategory) != 0).Select(kvp => kvp.Value).ForEach(a => list.AddRange(a));
            return list;
        }

        private static Dictionary<StationCategory, StationItem[]> Ids  {
            get
            {
                if (_configIsOverriden)
                {
                    return _ids;
                }
                _ids[StationCategory.Modern] = OptionsWrapper<Config>.Options.ModernStations.Items.ToArray();
                _ids[StationCategory.Old] = OptionsWrapper<Config>.Options.OldStations.Items.ToArray();
                _ids[StationCategory.Tram] = OptionsWrapper<Config>.Options.TramStations.Items.ToArray();
                _configIsOverriden = true;
                return _ids;
            }
        }

        public static IEnumerable<long> GetConvertedIds(StationCategory StationCategory = StationCategory.All)
        {
            var list = new List<long>();
            Ids.Where(kvp => IsCategoryEnabled(kvp.Key) && (kvp.Key & StationCategory) != 0).Select(kvp => kvp.Value).ForEach(l => l.ForEach(t => list.Add(t.WorkshopId)));
            return list;
        }

        private static bool IsCategoryEnabled(StationCategory StationCategory)
        {
            switch (StationCategory)
            {
                case StationCategory.All: //TODO(earalov): add more options
                    return OptionsWrapper<Options>.Options.ConvertTrainStationsToMetroStations;
                default:
                    throw new ArgumentOutOfRangeException(nameof(StationCategory), StationCategory, null);
            }
        }

        public static void CustomConversions(VehicleInfo info, long id, StationCategory StationCategory)
        {
            {
                if (info.m_trailers != null && info.m_trailers.Length > 0) //TODO(earalov): implement take trailers feature
                {
                    switch (id)
                    {
                        default:
                            break;
                    }
                }

                if (!ReplaceLastCar(id, StationCategory))
                {
                    return;
                }
                if (info.m_trailers != null && info.m_trailers.Length > 0)
                {
                    info.m_trailers[info.m_trailers.Length - 1] = new VehicleInfo.VehicleTrailer()
                    {
                        m_info = info, m_probability = 100, m_invertProbability = 100
                    };
                }
            }
        }

        private static bool ReplaceLastCar(long id, StationCategory StationCategory)
        {
            var list = new List<long>();
            Ids.Where(kvp => (kvp.Key & StationCategory) != 0).Select(kvp => kvp.Value).ForEach(l => l.ForEach(t => list.Add(t.WorkshopId)));
            return list.Contains(id);
        }


    }
}