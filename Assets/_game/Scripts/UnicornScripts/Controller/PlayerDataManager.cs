using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unicorn.UI.Shop;
using Unicorn.Utilities;
using UnityEngine;
using Spicyy.System; 
using Random = UnityEngine.Random;

namespace Unicorn
{
    /// <summary>
    /// Lưu giữ toàn bộ data game. 
    /// </summary>
    public class PlayerDataManager : MonoBehaviour, IDataSkin
    {
        public static PlayerDataManager Instance;
        public Action<TypeItem> actionUITop;
        
        public DataTexture DataTexture;
        public DataTextureSkin DataTextureSkin;
        public DataRewardEndGame DataRewardEndGame;
        public DataLuckyWheel DataLuckyWheel;
        public PlayerStatData DataPlayerStat; 

        private IDataLevel unicornDataLevel;

        private void Awake()
        {
            Instance = this;
            unicornDataLevel = null;
        }

        #region Bunker
        public int GetFloorInBunker()
        {
            return PlayerPrefs.GetInt(Helper.CurrentFloorInBunker, 0);
        }

        public void SetCurrentFloorInBunker(int floorIndex)
        {
            PlayerPrefs.SetInt(Helper.CurrentFloorInBunker, floorIndex);
        }

        #endregion

        #region Datalevel


        public bool CanChangeUsername()
        {
            // Kiểm tra xem trạng thái lần đầu tiên mở game đã được lưu trong PlayerPrefs chưa
            return PlayerPrefs.GetInt(Helper.ChangeableUsername, 1) == 1;
            // Giá trị mặc định là 1, nếu không có giá trị trong PlayerPrefs thì coi như đây là lần đầu tiên mở game
        }

        public void SetChangeableUsername(bool canChange)
        {
            // Lưu trạng thái lần đầu tiên mở game vào PlayerPrefs
            int intValue = canChange ? 1 : 0;
            PlayerPrefs.SetInt(Helper.ChangeableUsername, intValue);
            PlayerPrefs.Save();
        }

        public string GetUsername()
        {
            return PlayerPrefs.GetString(Helper.Username, "UNKNOWN");
        }
        public void SetUsername(string name)
        {
            PlayerPrefs.SetString(Helper.Username, name);
        }

        public void SetDataLevel(IDataLevel unicornDataLevel)
        {
            this.unicornDataLevel = unicornDataLevel;
            PlayerPrefs.SetString(Helper.DataLevel, JsonConvert.SerializeObject(unicornDataLevel));
        }
        public IDataLevel GetDataLevel(LevelConstraint levelConstraint)
        {
            var dataLevelJson = PlayerPrefs.GetString(Helper.DataLevel, string.Empty);
            unicornDataLevel = dataLevelJson == string.Empty
                ? new UnicornDataLevel(levelConstraint)
                : JsonConvert.DeserializeObject<UnicornDataLevel>(dataLevelJson);

            return unicornDataLevel ?? new UnicornDataLevel(levelConstraint);
        }

        public bool GetUnlockedLevel(int id)
        {
            return PlayerPrefs.GetInt(Helper.LevelUnlocked + id, 0) != 0;
        }

        public void SetUnlockedLevel(int id)
        {
            PlayerPrefs.SetInt(Helper.LevelUnlocked + id, 1);
        }

        public PlayerStatBonus GetStatDataAtLevel(PlayerUpgradeStatEvent.UpgradeStatType upgradeStatType, int level = 0)
        {
            Debug.Log(upgradeStatType);
            List<PlayerStatBonus> playerStatBonus = DataPlayerStat.dictStatTemplates[upgradeStatType];
            return playerStatBonus[level];
        }

        #endregion

        #region DataSkin
        public bool GetUnlockSkin(TypeEquipment type, int id)
        {
            return PlayerPrefs.GetInt(Helper.DataTypeSkin + type + id, 0) != 0;
        }
        public void SetUnlockSkin(TypeEquipment type, int id)
        {
            PlayerPrefs.SetInt(Helper.DataTypeSkin + type + id, 1);
            SetIdEquipSkin(type, id);
        }
        public int GetIdEquipSkin(TypeEquipment type)
        {
            return PlayerPrefs.GetInt(Helper.DataEquipSkin + type, 0);
        }
        public void SetIdEquipSkin(TypeEquipment type, int id)
        {
            PlayerPrefs.SetInt(Helper.DataEquipSkin + type, id);
        }
        public int GetVideoSkinCount(TypeEquipment type, int id)
        {
            return PlayerPrefs.GetInt(Helper.DataNumberWatchVideo + type + id, 0);
        }
        public void SetVideoSkinCount(TypeEquipment type, int id, int number)
        {
            PlayerPrefs.SetInt(Helper.DataNumberWatchVideo + type + id, number);
        }
        #endregion

        #region Gold, Gem, and Key
        public int GetGold()
        {
            return PlayerPrefs.GetInt(Helper.GOLD, 0);
        }
        public void SetGold(int _count)
        {
            PlayerPrefs.SetInt(Helper.GOLD, _count);
        }
        public int GetGem()
        {
            return PlayerPrefs.GetInt(Helper.GEM, 0);
        }
        public void SetGem(int _count)
        {
            PlayerPrefs.SetInt(Helper.GEM, _count);
        }
        public int GetKey()
        {
            return PlayerPrefs.GetInt(Helper.KEY, 0);
        }
        public void SetKey(int _count)
        {
            PlayerPrefs.SetInt(Helper.KEY, _count);
        }
        #endregion

        #region Reward End Game
        public int GetCurrentIndexRewardEndGame()
        {
            return PlayerPrefs.GetInt(Helper.CurrentRewardEndGame, 0);
        }
        public void SetCurrentIndexRewardEndGame(int index)
        {
            PlayerPrefs.SetInt(Helper.CurrentRewardEndGame, index);
        }
        public int GetProcessReceiveRewardEndGame()
        {
            return PlayerPrefs.GetInt(Helper.ProcessReceiveEndGame, 0);
        }

        public void SetProcessReceiveRewardEndGame(int number)
        {
            PlayerPrefs.SetInt(Helper.ProcessReceiveEndGame, number);
        }
        public int GetNumberWatchDailyVideo()
        {
            return PlayerPrefs.GetInt("NumberWatchDailyVideo", DataLuckyWheel.NumberSpinDaily);
        }
        public void SetNumberWatchDailyVideo(int number)
        {
            PlayerPrefs.SetInt("NumberWatchDailyVideo", number);
        }
        #endregion

        #region Lucky Spin
        public bool GetFreeSpin()
        {
            return PlayerPrefs.GetInt("FreeSpin", 1) > 0;
        }
        public void SetFreeSpin(bool isFree)
        {
            int free = isFree ? 1 : 0;
            PlayerPrefs.SetInt("FreeSpin", free);
        }
        public int GetNumberWatchVideoSpin()
        {
            return PlayerPrefs.GetInt("NumberWatchVideoSpin", 0);

        }
        public void SetNumberWatchVideoSpin(int count)
        {
            PlayerPrefs.SetInt("NumberWatchVideoSpin", count);
        }
        public string GetTimeLoginSpinFreeWheel()
        {
            return PlayerPrefs.GetString("TimeSpinFreeWheel", "");
        }
        public void SetTimeLoginSpinFreeWheel(string time)
        {
            PlayerPrefs.SetString("TimeSpinFreeWheel", time);
        }
        public string GetTimeLoginSpinVideo()
        {
            return PlayerPrefs.GetString("TimeLoginSpinVideo", "");
        }
        public void SetTimeLoginSpinVideo(string time)
        {
            PlayerPrefs.SetString("TimeLoginSpinVideo", time);
        }
        #endregion

        #region Setting
        public void SetSoundSetting(bool isOn)
        {
            PlayerPrefs.SetInt(Helper.SoundSetting, isOn ? 1 : 0);
        }
        public bool GetSoundSetting()
        {
            return PlayerPrefs.GetInt(Helper.SoundSetting, 1) == 1;
        }
        public void SetMusicSetting(bool isOn)
        {
            PlayerPrefs.SetInt(Helper.MusicSetting, isOn ? 1 : 0);
        }
        public bool GetMusicSetting()
        {
            return PlayerPrefs.GetInt(Helper.MusicSetting, 1) == 1;
        }
        public void SetVibrateSetting(bool isOn)
        {
            PlayerPrefs.SetInt(Helper.VibrateSetting, isOn ? 1 : 0);
        }
        public bool GetVibrateSetting()
        {
            return PlayerPrefs.GetInt(Helper.VibrateSetting, 1) == 1;
        }
        #endregion

        #region Ads
        public bool IsNoAds()
        {
            return PlayerPrefs.GetInt("NoAds", 0) == 1;
        }
        public void SetNoAds()
        {
            PlayerPrefs.SetInt("NoAds", 1);
        }
        #endregion
    }

}