using System.Reflection;

namespace FerryConverter
{
    public static class Bonus
    {
        private static readonly FieldInfo _uiCategoryfield = typeof(PrefabInfo).GetField("m_UICategory", BindingFlags.NonPublic | BindingFlags.Instance);
        
        public static void apply(BuildingInfo prefab)
        {
            if (prefab == null)
            {
                return;
            }
            if (prefab.name != "Boat Museum Steam Boat")
            {
                return;
            }
            prefab.m_placementStyle = ItemClass.Placement.Manual;
            prefab.m_class = PrefabCollection<BuildingInfo>.FindLoaded("Ship Wreck 01").m_class;
            _uiCategoryfield.SetValue(prefab, "LandscapingWaterStructures");
        }
    }
}