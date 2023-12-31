using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class UiStage : UICanvas
    {
        protected override void Awake()
        {

        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (!isShow)
            {
                return;
            }
        }

        
    }

}
