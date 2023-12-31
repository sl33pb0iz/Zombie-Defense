using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowyy.EquipmentSystem
{
    [CreateAssetMenu(fileName = "Resources Data", menuName = "Equipment System Data/Data Resources Data")]
    public class DataResources : SerializedScriptableObject
    {
        [Title("Resource Type Information")]
        [InfoBox("This dictionary stores resource type information like icon sprite, etc...")]
        [PropertyOrder(0)]
        public Dictionary<ResourceType, ResourceInfo> dictResourceInfos;
        
    }
    [Serializable]
    public class ResourceInfo
    {
        public Sprite iconResource;
    }
}
