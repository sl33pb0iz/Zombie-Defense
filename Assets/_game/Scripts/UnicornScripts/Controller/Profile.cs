using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    /// <summary>
    /// Quản lý resourses trong game như Gold hay Key
    /// </summary>
    /// <remarks>
    /// Ai refactor class này vào <see cref="PlayerDataManager"/> được thì làm giúp em nhé ;-;
    /// </remarks>
    public class Profile
    {
        public void AddGold(int goldBonus, string _analytic)
        {
            var playerdata = GameManager.Instance.PlayerDataManager;
            int _count = GetGold() + goldBonus;
            PlayerDataManager.Instance.SetGold(_count);

            if (playerdata.actionUITop != null)
            {
                playerdata.actionUITop(TypeItem.Coin);
            }
        }

        public int GetGold()
        {
            return PlayerDataManager.Instance.GetGold();
        }

        public void AddGem(int gemBonus, string _analytic)
        {
            var playerdata = GameManager.Instance.PlayerDataManager;
            int _count = GetGem() + gemBonus;
            PlayerDataManager.Instance.SetGem(_count);

            if (playerdata.actionUITop != null)
            {
                playerdata.actionUITop(TypeItem.Gem);
            }
        }

        public int GetGem()
        {
            return PlayerDataManager.Instance.GetGem();
        }

        public void AddKey(int amount, string _analytic)
        {
            var playerdata = GameManager.Instance.PlayerDataManager;

            PlayerDataManager.Instance.SetKey(GetKey() + amount);

            if (playerdata.actionUITop != null && amount == 1)
            {
                playerdata.actionUITop(TypeItem.Key);
            }
        }

        public int GetKey()
        {
            return PlayerDataManager.Instance.GetKey();
        }
    }

}