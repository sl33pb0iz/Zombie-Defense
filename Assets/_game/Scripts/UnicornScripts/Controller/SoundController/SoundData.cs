using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundData : MonoBehaviour
{
    public AudioClip AudioClickBtn;

    public AudioClip AudioFootStep;
    public AudioClip AudioRevive;
    public AudioClip AudioReward;
    public AudioClip AudioSpinWheel;
    public AudioClip AudioWin;
    public AudioClip AudioLose;
    public AudioClip AudioClockTick;
    public AudioClip AudioRewardClick;

    public AudioClip[] AudiosLobby;
    public AudioClip[] AudioBgs;

    public AudioClip[] AudioPlayerFalls;

    public List<AudioClip> ListAudioCollects;

    public AudioClip AudioWarning; 


}
