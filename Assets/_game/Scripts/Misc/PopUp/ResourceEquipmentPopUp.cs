using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Snowyy;

public class ResourceEquipmentPopUp : ItemInfoPopUp
{
    public TextMeshPro m_Text;
    public Image m_Image;
    public Color m_Color;

    public Sprite m_Icons; 

    public float addHeigh;
    public float _duration;

    private void Start()
    {
        m_Image.sprite = m_Icons;
        m_Text.color = m_Color;
    }

    private void OnEnable()
    {
        Animated();
    }

    public override void Animated()
    {
        transform.DOMove(transform.position + new Vector3(0, addHeigh, 0), _duration, false).OnComplete(() => gameObject.SetActive(false));
        transform.DOScale(1.5f, _duration);
    }

    public override void SetUp(string text, int index = 0)
    {
        m_Text.text = text;
    }
}
