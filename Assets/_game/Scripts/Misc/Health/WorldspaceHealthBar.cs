using UnityEngine;
using UnityEngine.UI;

namespace Unicorn
{
    public class WorldspaceHealthBar : MonoBehaviour
    {
        [Tooltip("Health component to track")] public Health Health;

        public GameObject healthBarParent; 

        [Tooltip("Image component displaying health left")]
        public Image HealthBarImage;

        [Tooltip("The floating healthbar pivot transform")]
        public Transform HealthBarPivot;

        [Tooltip("Whether the health bar is visible when at full health or not")]
        public bool HideFullHealthBar = true;

        public Gradient gradient;

        void LateUpdate()
        {
            // update health bar value
            float currentRatio = Health.CurrentHealth / Health.maxHealth;

            HealthBarImage.color = gradient.Evaluate(currentRatio);

            float speedChange = 5f; 
            HealthBarImage.fillAmount  = Mathf.Lerp(HealthBarImage.fillAmount, currentRatio, Time.deltaTime * speedChange );
            
           Vector3 direction = Camera.main.transform.forward;
            HealthBarPivot.forward = direction;

            // hide health bar if needed
            if (HideFullHealthBar)
                HealthBarPivot.gameObject.SetActive(HealthBarImage.fillAmount != 1);
        }

        public void ActiveHealthBar(bool value)
        {
            healthBarParent.SetActive(value);
        }    
    }
}