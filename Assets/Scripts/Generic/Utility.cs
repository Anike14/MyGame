using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


static public class Utility
{
    public static bool IsUnitInLayerMask(UnitBase unit, LayerMask layerMask) {
        return (layerMask.value & (1 << unit.gameObject.layer)) > 0;
    }
}
