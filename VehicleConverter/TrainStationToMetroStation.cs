﻿using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using FerryConverter.Config;

namespace FerryConverter
{
    public static class TrainStationToMetroStation
    {

        private static readonly FieldInfo _uiCategoryfield = typeof(PrefabInfo).GetField("m_UICategory", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool Convert(BuildingInfo info)
        {
            long id;
            if (!Util.TryGetWorkshopId(info, out id) || !Stations.GetConvertedIds(StationCategory.All).Contains(id))
            {
                return false;
            }
            UnityEngine.Debug.Log("Converting " + info.name);
            var metroEntrance = PrefabCollection<BuildingInfo>.FindLoaded("Metro Entrance");
            var ai = info.GetComponent<PlayerBuildingAI>();
            if (ai == null)
            {
                return false;
            }
            var stationAi = ai as TransportStationAI;
            if (stationAi != null)
            {
                if (stationAi.m_transportInfo == PrefabCollection<TransportInfo>.FindLoaded("Metro"))
                {
                    return true; //already a metro station
                }
                info.m_class = (ItemClass)ScriptableObject.CreateInstance(nameof(ItemClass));
                info.m_class.name = info.name;
                info.m_class.m_subService = ItemClass.SubService.PublicTransportMetro;
                info.m_class.m_service = ItemClass.Service.PublicTransport;
                stationAi.m_transportLineInfo = PrefabCollection<NetInfo>.FindLoaded("Metro Line");
                stationAi.m_transportInfo = PrefabCollection<TransportInfo>.FindLoaded("Metro");
                stationAi.m_maxVehicleCount = 0;
            }
            if (Stations.ToDecoration(id))
            {
                GameObject.Destroy(ai);
                var newAi = info.gameObject.AddComponent<DecorationBuildingAI>();
                info.m_buildingAI = newAi;
                newAi.m_info = info;
                newAi.m_allowOverlap = true;
                info.m_placementMode = BuildingInfo.PlacementMode.OnGround;
            }
            else
            {
                ai.m_createPassMilestone = metroEntrance.GetComponent<PlayerBuildingAI>().m_createPassMilestone;
            }
            _uiCategoryfield.SetValue(info, metroEntrance.category);
            info.m_UnlockMilestone = metroEntrance.m_UnlockMilestone;


            if (info.m_paths == null)
            {
                return true;
            }
            var nameConverter = Stations.GetCategory(id) == StationCategory.Old ? new Func<string, string>(s => "Steel " + s) : (s => s);
            var metroTrack = PrefabCollection<NetInfo>.FindLoaded(nameConverter.Invoke("Metro Track Ground")) ?? PrefabCollection<NetInfo>.FindLoaded("Metro Track Ground");
            var metroTrackElevated = PrefabCollection<NetInfo>.FindLoaded(nameConverter.Invoke("Metro Track Elevated")) ?? PrefabCollection<NetInfo>.FindLoaded("Metro Track Elevated");
            var metroTrackSlope = PrefabCollection<NetInfo>.FindLoaded(nameConverter.Invoke("Metro Track Slope")) ?? PrefabCollection<NetInfo>.FindLoaded("Metro Track Slope");
            var metroTrackTunnel = PrefabCollection<NetInfo>.FindLoaded(nameConverter.Invoke("Metro Track")) ?? PrefabCollection<NetInfo>.FindLoaded("Metro Track");
            var metroStationTrack = PrefabCollection<NetInfo>.FindLoaded(nameConverter.Invoke("Metro Station Track Ground")) ?? PrefabCollection<NetInfo>.FindLoaded("Metro Station Track Ground");
            var metroStationTracElevated = PrefabCollection<NetInfo>.FindLoaded(nameConverter.Invoke("Metro Station Track Elevated")) ?? PrefabCollection<NetInfo>.FindLoaded("Metro Station Track Elevated");
            var metroStationTracSunken = PrefabCollection<NetInfo>.FindLoaded(nameConverter.Invoke("Metro Station Track Sunken")) ?? PrefabCollection<NetInfo>.FindLoaded("Metro Station Track Sunken");
            foreach (var path in info.m_paths)
            {
                if (path?.m_netInfo?.name == null || path.m_netInfo.m_class?.m_subService != ItemClass.SubService.PublicTransportTrain)
                {
                    continue;
                }
                if (metroTrackTunnel != null)
                {
                    if (path.m_netInfo.name.Contains("Train Track Tunnel"))
                    {
                        path.m_netInfo = metroTrackTunnel;
                        continue;
                    }
                }
                if (metroTrackElevated != null)
                {
                    if (path.m_netInfo.name.Contains("Train Track Elevated"))
                    {
                        path.m_netInfo = metroTrackElevated;
                        continue;
                    }
                }
                if (metroTrackSlope != null)
                {
                    if (path.m_netInfo.name.Contains("Train Track Slope"))
                    {
                        path.m_netInfo = metroTrackSlope;
                        continue;
                    }
                }
                if (metroStationTracElevated != null)
                {
                    if (path.m_netInfo.name.Contains("Station Track Eleva"))
                    {
                        path.m_netInfo = metroStationTracElevated;
                        continue;
                    }
                }
                if (metroStationTracSunken != null)
                {
                    if (path.m_netInfo.name.Contains("Station Track Sunken"))
                    {
                        path.m_netInfo = metroStationTracSunken;
                        continue;
                    }
                }
                if (metroStationTrack != null)
                {
                    if (path.m_netInfo.name.Contains("Train Station Track"))
                    {
                        path.m_netInfo = metroStationTrack;
                        continue;
                    }
                }
                if (metroTrack != null)
                {
                    if (path.m_netInfo.name.Contains("Train Track"))
                    {
                        path.m_netInfo = metroTrack;
                        continue;
                    }
                }

                //TODO(earalov): add more More Tracks and ETST tracks ?
            }
            return true;
        }
    }
}