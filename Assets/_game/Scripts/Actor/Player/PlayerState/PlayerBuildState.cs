using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Unicorn
{
    public class PlayerBuildState : PlayerBaseState
    {

        public PlayerBuildState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory):base(currentContext, playerStateFactory){
        }
        public override void EnterState(){
            //CTX.Animator.SetBool("isBuild", true);
        }

        public override void UpdateState()
        {
            PayMoneyToUpgrade();
            CheckSwitchStates();
        }
        public override void ExitState(){
            //CTX.Animator.SetBool("isBuild", false);
        }
        public override void CheckSwitchStates(){

            switch (CTX.subState)
            {
                case PlayerStateMachine.SubState.Walk:
                    ExitState();
                    SwitchState(Factory.Walk());
                    break;
                    
            }
        }
        public override void InitializeSubState(){}

        public void PayMoneyToUpgrade()
        {
            if (CTX.m_bodyCollision.canUpgradeTurrret && CTX.m_bodyCollision.activeTurretUpgrade)
            {
                if (CTX.timeDelayCountDown > 0) CTX.timeDelayCountDown -= Time.deltaTime;
                else
                {
                    if (PlayerDataManager.Instance.GetGold() - CTX.m_CurrencyManager._goldPaidPerTime > 0)
                    {
                        GameObject gold = PoolManager.Instance.ReuseObject(CTX.m_CurrencyManager._goldPrefab, CTX.transform.position, Quaternion.identity);
                        gold.SetActive(true);
                        CTX.m_CurrencyManager.MoneyPaidToUpgrade(CTX.m_CurrencyManager._goldPaidPerTime, gold, CTX.m_bodyCollision._turretUpgradePosition + new Vector3(0,2,0), CTX.m_bodyCollision.activeTurretUpgrade);
                        CTX.timeDelayCountDown = CTX.reloadPaid;
                    }
                }
            }
        }
    }
}
