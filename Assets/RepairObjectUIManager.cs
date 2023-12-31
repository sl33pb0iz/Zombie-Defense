using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class RepairObjectUIManager : SingletonMonobehavior<RepairObjectUIManager>
    {
        [HideInInspector] public GameObject wallToRepair;
        public GameObject RepairButtonUI; 
        public void RepairWall()
        {
            /*if (wallToRepair == null)
                return;
            var repairable = wallToRepair.GetComponent<Repairable>();
            if (repairable == null)
                return;
            if (!repairable.GetBrokenObject() || !repairable.GetBrokenObject().activeSelf)
                return;
            repairable.OnRepair();*/
        }

        public void ShowRepairButton()
        {
            RepairButtonUI.SetActive(true);
        }
        public void HideRepairButton()
        {
            RepairButtonUI.SetActive(false);
        }
       
    }
}
