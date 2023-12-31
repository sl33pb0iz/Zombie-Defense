using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowyy.EquipmentSystem
{
    [Serializable]
    public class Resource
    {
        public ResourceType resourceType;
        public int quantity;

        public Resource()
        {
        }

        public Resource(ResourceType resourceType, int quantity)
        {
            this.resourceType = resourceType;
            this.quantity = quantity;
        }
    }
}
