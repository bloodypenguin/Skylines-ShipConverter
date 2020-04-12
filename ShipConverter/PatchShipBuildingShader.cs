using System.Linq;
using System.Reflection;
using FerryConverter.Config;
using UnityEngine;

namespace FerryConverter
{
    public class PatchShipBuildingShader
    {
        private static readonly FieldInfo _uiCategoryfield = typeof(PrefabInfo).GetField("m_UICategory", BindingFlags.NonPublic | BindingFlags.Instance);


        public static bool Convert(BuildingInfo info)
        {
            if (!Util.TryGetWorkshopId(info, out long id) || !Buildings.GetConvertedIds(BuildingCategory.ShipBuilding).Contains(id))
            {
                return false;
            }
            var oldAi = info.GetComponent<BuildingAI>();
            if (oldAi == null)
            {
                return false;
            }
            UnityEngine.Debug.Log("Converting " + info.name);
            Object.DestroyImmediate(oldAi);
            var ai = info.gameObject.AddComponent<DummyBuildingAI>();
            ai.m_ignoreNoPropsWarning = true;
            ai.m_info = info;
            info.m_buildingAI = ai;
            info.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Buildings/Building/Floating");
            info.m_lodObject.GetComponent<Renderer>().material.shader = Shader.Find("Custom/Buildings/Building/Floating");
            info.m_requireHeightMap = true;
            info.m_requireWaterMap = true;
            info.m_placementMode = BuildingInfo.PlacementMode.OnWater;
            info.m_class = PrefabCollection<BuildingInfo>.FindLoaded("Ship Wreck 01").m_class;
            _uiCategoryfield.SetValue(info, "LandscapingWaterStructures");
            return true;
        }
    }
}