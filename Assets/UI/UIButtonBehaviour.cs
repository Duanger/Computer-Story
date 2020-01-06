using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonBehaviour : MonoBehaviour,IPointerEnterHandler
{
    [SerializeField] private bool canResetAfterPress;
    public bool isRightClickButton;
    public bool rightClicked;
    [SerializeField] private Camera camdam;
    [SerializeField] private GameObject clickBox;
    [SerializeField] private Image cursorImage;
    [SerializeField] private Sprite pointerSprite,handSprite;
    [SerializeField] private RectTransform _r;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckResetState()
    {
        if (canResetAfterPress)
        {
            anim.SetBool("ResetState",true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursorImage.sprite = handSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursorImage.sprite = pointerSprite;
    }
}
