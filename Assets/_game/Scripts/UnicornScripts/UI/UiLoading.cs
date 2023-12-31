using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn.UI
{
    public class UiLoading : UICanvas
    {

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (!_isShown)
            {
                return; 
            }
            
        }

    }
}
