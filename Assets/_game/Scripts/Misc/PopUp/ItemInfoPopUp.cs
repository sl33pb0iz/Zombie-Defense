using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemInfoPopUp : MonoBehaviour
{
    public abstract void SetUp(string text, int index = 0 ); 
    public abstract void Animated(); 
}
