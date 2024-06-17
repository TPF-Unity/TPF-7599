using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DamageLayerMapping", menuName = "ScriptableObjects/DamageLayerMapping", order = 1)]
public class DamageLayerMapping : ScriptableObject
{
    [System.Serializable]
    public class LayerDamageInfo
    {
        public string layerName;
        public List<string> canDamageLayerNames;
    }

    public List<LayerDamageInfo> layerDamageInfos;

    public bool CanDamage(string attackerLayer, string targetLayer)
    {
        foreach(var info in layerDamageInfos)
        {
            if(info.layerName == attackerLayer)
            {
                return info.canDamageLayerNames.Contains(targetLayer);
            }
        }
        return false;
    }
}
