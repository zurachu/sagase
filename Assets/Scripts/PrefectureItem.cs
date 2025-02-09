﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class PrefectureItem : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] GameObject right;
    [SerializeField] GameObject miss;
    [SerializeField] Text nameText;

    private bool isRight;
    private Action<bool> onTap;

    public void Initialize(Sprite sprite, string name, bool isRight, Action<bool> onTap)
    {
        this.isRight = isRight;
        this.onTap = onTap;

        image.sprite = sprite;
        nameText.text = name;
        ClearStatus();
    }

    public void OnTap()
    {
        if (right.activeSelf || miss.activeSelf)
        {
            return;
        }

        if (isRight)
        {
            right.SetActive(true);
        }
        else
        {
            miss.SetActive(true);
        }

        nameText.gameObject.SetActive(true);

        onTap?.Invoke(isRight);
    }

    public void ClearStatus()
    {
        right.SetActive(false);
        miss.SetActive(false);
        nameText.gameObject.SetActive(false);
    }
}
