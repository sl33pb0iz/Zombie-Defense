using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Spicyy.System;
using UnityEngine.SceneManagement;
using Unicorn.Utilities;


namespace Unicorn
{
    public class SkillUISystem : MonoBehaviour
    {
        //public 
        public List<ButtonSkillController> buttonSkills;
        private readonly Dictionary<PlayerCastSkillEvent.ActivatedSkillType, ButtonSkillController> m_Skills = new Dictionary<PlayerCastSkillEvent.ActivatedSkillType, ButtonSkillController>();

        //private 
        private readonly float targetSceneIndex = 4;
        private int currentSceneIndex;

        private void Start()
        {
            currentSceneIndex = SceneManager.GetSceneAt(1).buildIndex;
            foreach (var buttonSkill in buttonSkills)
            {
                m_Skills.Add(buttonSkill.skillType, buttonSkill);
            }
        }

        private void OnEnable()
        {
            foreach (var buttonskill in buttonSkills)
            {
                buttonskill.m_Button.onClick.AddListener(() => CheckCastSkill(buttonskill.skillType));
            }
        }

        private void OnDisable()
        {
            foreach (var buttonskill in buttonSkills)
            {
                buttonskill.m_Button.onClick.RemoveListener(() => CheckCastSkill(buttonskill.skillType));
            }
        }

        //kiểm tra xem có cần phải bắn quảng cáo trước khi cast skill không
        public void CheckCastSkill(PlayerCastSkillEvent.ActivatedSkillType skillType)
        {
            if (currentSceneIndex < targetSceneIndex)
            {
                CastSkill(skillType);
            }
            else UnicornAdManager.ShowAdsReward(() => CastSkill(skillType), Helper.video_cast_player_skill);
        }

        //Cast skill dựa vào loại skill được chọn khi bấm nút
        private void CastSkill(PlayerCastSkillEvent.ActivatedSkillType skillType)
        {
            if (m_Skills.TryGetValue(skillType, out ButtonSkillController button))
            {
                if (button.onTimeCountDown)
                {
                    return;
                }
                else
                {
                    Events.PlayerCastSkillEvent.skillType = skillType;
                    button.onTimeCountDown = true;
                    void OnComplete() { button.onTimeCountDown = false; }
                    DeactiveSkill(button.m_Button, button.m_Text, button.timeCoolDown, OnComplete);
                    EventManager.Broadcast(Events.PlayerCastSkillEvent);
                }
            }
            return;
        }

        //Khi skill được sử dụng , sẽ có thời gian chờ để có thể tiếp tục sử dụng kĩ năng 
        public void DeactiveSkill(Button button, TextMeshProUGUI text, float duration, UnityAction OnComplete)
        {
            button.interactable = false;
            void onComplete() { button.interactable = true; OnComplete.Invoke(); }
            StartCoroutine(SkillTime(text, duration, onComplete));

            IEnumerator SkillTime(TextMeshProUGUI textTime, float duration, UnityAction OnComplete)
            {
                textTime.text = duration.ToString();
                textTime.gameObject.SetActive(true);
                float timer = duration;
                while (timer > 0)
                {
                    yield return Yielders.Get(1f);
                    timer--;
                    textTime.text = timer.ToString();
                }
                textTime.gameObject.SetActive(false);
                OnComplete.Invoke();
            }
        }

    }

    [System.Serializable]
    public class ButtonSkillController
    {
        public PlayerCastSkillEvent.ActivatedSkillType skillType;
        public Button m_Button;
        public TextMeshProUGUI m_Text;
        public float timeCoolDown;

        [HideInInspector] public bool onTimeCountDown = false;
    }
}