using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace Unicorn
{
    /// <summary>
    /// Class sida phết, nếu dùng được thì dùng nhé, không có thì thôi (✿◡‿◡)
    /// </summary>
    [Singleton("SoundManager", true)]
    public class SoundManager : Singleton<SoundManager>
    {
        public enum GameSound
        {
            BGM,
            Footstep,
            Spin,
            Lobby,
            ClockTick,
            RewardClick,
            Warning,
        }

        [FoldoutGroup("Set Up")] public SoundData soundData;
        [FoldoutGroup("Set Up")] public AudioMixer audioMixer;
        
        [FoldoutGroup("Audio Source")] public AudioSource bgMusic;
        [FoldoutGroup("Audio Source")] public AudioSource fxSound;
        [FoldoutGroup("Audio Source")] public AudioSource fxSoundFootStep;
        [FoldoutGroup("Audio Source")] public AudioSource clockTickFast;
        [FoldoutGroup("Audio Source")] public AudioSource coffinSource;
        [FoldoutGroup("Audio Source")] public AudioSource warningSound;

        private float bgVol;
        private bool isPlayFootStep; 

        #region UNITY METHOD
        private void Start()
        {
            SettingFxSound(PlayerDataManager.Instance.GetSoundSetting());
            SettingMusic(PlayerDataManager.Instance.GetMusicSetting());
            isPlayFootStep = false;
        }
        #endregion

        #region PUBLIC METHOD
        public void PlayFxSound(Enum soundEnum)
        {
            switch (soundEnum)
            {
                case LevelResult levelResult:
                {
                    switch (levelResult)
                    {
                        case LevelResult.Win:
                            PlaySoundWinCrewmate();
                            break;
                        case LevelResult.Lose:
                            PlaySoundWinImposter();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
                case GameSound gameSound:
                {
                    switch (gameSound)
                    {
                        case GameSound.BGM:
                            PlayBGM(Random.Range(0, soundData.AudioBgs.Length));
                            break;
                        case GameSound.Footstep:
                            PlayFootStep();
                            break;
                        case GameSound.Spin:
                            PlaySoundSpin();
                            break;
                        case GameSound.Lobby:
                            PlayBGM(soundData.AudiosLobby[Random.Range(0, soundData.AudiosLobby.Length)]);
                            break;
                        case GameSound.ClockTick:
                            PlayFxSound(soundData.AudioClockTick);
                            break;
                        case GameSound.RewardClick:
                            PlayFxSound(soundData.AudioRewardClick);
                            break;
                            default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
                case TypeSoundIngame collectibleSound:
                {
                    PlaySoundCollectible(collectibleSound);
                    break;
                }
                default:
                    PlayFxSound(soundEnum, fxSound);
                    break;
            }
        }
        public void PlayFxSound(Enum soundEnum, AudioSource audioSource)
        {
            switch (soundEnum)
            {
                default:
                    throw new InvalidEnumArgumentException($"{soundEnum} is not supported");
            }
        }
        public void StopSound(Enum soundEnum)
        {
            switch (soundEnum)
            {
                case GameSound gameSound:
                {
                    switch (gameSound)
                    {
                        case GameSound.Lobby:
                        case GameSound.BGM:
                            bgMusic.DOFade(0, 1f).OnComplete(action: () => bgMusic.Stop());
                            break;
                        case GameSound.Footstep:
                            StopFootStep();
                            break;
                        case GameSound.Spin:
                            StopFxSound();
                            break;
                        case GameSound.ClockTick:
                            StopFxSound();
                            break;
                        case GameSound.RewardClick:
                            StopFxSound();
                            break;
                            default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
                case LevelResult levelResult:
                case TypeSoundIngame collectibleSound:
                {
                    StopFxSound();
                    break;
                }
                default:
                    throw new InvalidEnumArgumentException($"{soundEnum} is not supported");
            }
        }
        public void SettingFxSound(bool isOn)
        {
            var vol = isOn ? 1 : 0;
            fxSound.volume = vol;
            fxSoundFootStep.volume = vol;
            fxSound.mute = !isOn;
            fxSoundFootStep.mute = !isOn;
        }
        public void SettingMusic(bool isOn)
        {
            bgVol = isOn ? 1 : 0;
            bgMusic.volume = bgVol;
            bgMusic.mute = !isOn;
            //ValueBGMusic = vol;
        }
        #endregion

        #region PRIVATE METHOD
        private void PlayFxSound(AudioClip clip, AudioSource audioSource)
        {
            audioSource.PlayOneShot(clip);
        }
        public bool IsOnVibration
        {
            get { return PlayerPrefs.GetInt("OnVibration", 1) == 1 ? true : false; }
        }
        private void PlayBGM(int index)
        {
            var backgroundMusics = soundData.AudioBgs;
            PlayBGM(backgroundMusics[index]);

        }
        private void PlayBGM(AudioClip audioClip)
        {
            bgMusic.loop = true;
            bgMusic.clip = audioClip;
            bgMusic.volume = 0;
            bgMusic.DOKill();
            bgMusic.DOFade(bgVol, 1f);
            bgMusic.Play();
        }

        public void PlayWarningSound()
        {
            if (!warningSound.isPlaying) {
                warningSound.PlayOneShot(warningSound.clip);
            }

        }

        public void StopWarningSound()
        {
            warningSound.Stop();
        }

        private void PlayClockTick(AudioClip clip)
        {
            clockTickFast.clip = clip;
            clockTickFast.Play();
        }

        public void PlayFxSound(AudioClip clip)
        {
            fxSound.PlayOneShot(clip);
        }
        private void StopFxSound()
        {
            fxSound.Stop();
        }
        public void PlayCoffinTheme(bool isPlaying, float delay = 0)
        {
            if (isPlaying)
            {
                audioMixer.DOSetFloat("BGMVol", -80, delay / 3 * 2).SetEase(Ease.InSine).SetDelay(delay / 3);
                audioMixer.DOSetFloat("FXVol", -80, delay / 3 * 2).SetEase(Ease.InSine).SetDelay(delay / 3)
                    .OnComplete(() => coffinSource.Play());
            }
            else
            {
                audioMixer.SetFloat("BGMVol", 0);
                audioMixer.SetFloat("FXVol", 0);
                coffinSource.Stop();
            }
        }
        public void PlaySoundButton()
        {
            coffinSource.PlayOneShot(soundData.AudioClickBtn);
        }
        public void PlaySoundSpin()
        {
            PlayFxSound(soundData.AudioSpinWheel);
        }
        public void PlaySoundRevive()
        {
            PlayFxSound(soundData.AudioRevive);
        }
        public void PlaySoundReward()
        {
            PlayFxSound(soundData.AudioReward);
        }
        public void PlaySoundWinCrewmate()
        {
            PlayFxSound(soundData.AudioWin);
        }
        public void PlaySoundWinImposter()
        {
            PlayFxSound(soundData.AudioLose);
        }
        public void PlaySoundCollectible(TypeSoundIngame typeSound)
        {
            PlayFxSound(soundData.ListAudioCollects[(int) typeSound - 1]);
        }
        public void PlayFootStep()
        {
            if (isPlayFootStep)
                return;
           
            isPlayFootStep = true;
            fxSoundFootStep.Play();

            Analytics.LogFirstLogJoystick();
        }
        public void StopFootStep()
        {
            fxSoundFootStep.Stop();
            isPlayFootStep = false;
        }
        #endregion
    }

}