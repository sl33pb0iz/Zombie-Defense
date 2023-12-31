using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn.UI;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using Unicorn;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using Snowyy;

namespace Arena
{
    public class UiTurretPack : UICanvas
    {
        [SerializeField] private List<ItemTurretShop> itemTurretShops;

        public void Init()
        {
            for(int index = 0; index < itemTurretShops.Count; index++)
            {
                itemTurretShops[index].InitItem(index);
            }
        }
    }
}

