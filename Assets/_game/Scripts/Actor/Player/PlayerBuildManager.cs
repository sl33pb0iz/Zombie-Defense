using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class PlayerBuildManager : MonoBehaviour
    { 
        private Node activeNode;
        private TurretBlueprint turretToBuild;
        
        public TurretBlueprint GetTurretToBuild()
        {
            return turretToBuild;
        }

        public void BuildTurretOn(TurretBlueprint turret)
        {
            if(activeNode == null)
            {
                return;
            }
            turretToBuild = turret;
            if(turretToBuild.cost > PlayerDataManager.Instance.GetGold())
            {
                Debug.Log("Dont have enough money");
                return;
            }
            activeNode.BuildTurret();
        }
       

        public void SetActiveNode(Node node)
        {
            activeNode = node;
        }
    }
}
