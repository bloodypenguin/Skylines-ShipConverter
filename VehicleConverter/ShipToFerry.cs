using System.Linq;
using FerryConverter.Config;
using UnityEngine;

namespace FerryConverter
{
    public class ShipToFerry
    {
        public static bool Convert(VehicleInfo info)
        {
            long id;
            if (!Util.TryGetWorkshopId(info, out id) || !Ships.GetConvertedIds(ShipCategory.PassengerShip).Contains(id))
            {
                return false;
            }
            var ferry = Cache.FerryVehicle;
            if (ferry == null)
            {
                return false;
            }
            UnityEngine.Debug.Log("Converting " + info.name);
            info.m_class = (ItemClass) ScriptableObject.CreateInstance(nameof(ItemClass));
            info.m_class.name = info.name;
            info.m_class.m_subService = ItemClass.SubService.PublicTransportShip;
            info.m_class.m_service = ItemClass.Service.PublicTransport;
            info.m_class.m_level = ItemClass.Level.Level2;

            info.m_vehicleType = VehicleInfo.VehicleType.Ferry;
            var oldAi = info.GetComponent<PassengerShipAI>();
            Object.DestroyImmediate(oldAi);
            var ai = info.gameObject.AddComponent<PassengerFerryAI>();
            ai.m_transportInfo = Cache.FerryTransport;
            ai.m_info = info;
            info.m_vehicleAI = ai;

            info.m_acceleration = ferry.m_acceleration;
            info.m_braking = ferry.m_braking;
            info.m_leanMultiplier = ferry.m_leanMultiplier;
            info.m_dampers = ferry.m_dampers;
            info.m_springs = ferry.m_springs;
            info.m_maxSpeed = ferry.m_maxSpeed;
            info.m_nodMultiplier = ferry.m_nodMultiplier;

            return true;
        }
    }
}