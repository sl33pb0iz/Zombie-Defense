using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;

namespace Unicorn
{
    /// <summary>
    /// Lưu dữ liệu để tặng quà vào cuối game
    /// </summary>
    [CreateAssetMenu(fileName = "DataRewardEndGame", menuName = "ScriptableObjects/Data Reward EndGame")]
    public class DataRewardEndGame : SerializedScriptableObject
    {
        public List<RewardEndGame> Datas;
        public List<RewardLuckyWheel> DataRewardCurrency;
    }
    
    /// <summary>
    /// <see cref="DataRewardEndGame"/> component
    /// </summary>
    public class RewardEndGame
    {
        public TypeEquipment Type;
        public int Id;
        public int NumberWin;
        public int NumberCoinReplace;
    }

    public class RewardLuckyWheel
    {
        public TypeGiftLuckyWheel Type;
        public int quantity;
    }

}