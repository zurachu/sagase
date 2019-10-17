using System;
using UnityEngine;

public class NoneItem : MonoBehaviour
{
    [SerializeField] GameObject right;
    [SerializeField] GameObject miss;

    private bool isRight;
    private Action<bool> onTap;

    public void Initialize(bool isRight, Action<bool> onTap)
    {
        this.isRight = isRight;
        this.onTap = onTap;

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

        onTap?.Invoke(isRight);
    }

    public void ClearStatus()
    {
        right.SetActive(false);
        miss.SetActive(false);
    }
}
