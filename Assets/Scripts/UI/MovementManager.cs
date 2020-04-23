﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementManager : MonoBehaviour
{
    public CanvasSwapper canvasSwapper;
    public GameObject LeftButton;
    public GameObject RightButton;
    public GameObject BackButton;
    public bool backIsDisabled = false;
    public bool leftIsDisabled = false;
    public bool rightIsDisabled = false;
    public bool lookingEnabled = true;
    
    // 0 -> left
    // 1 -> center
    // 2 -> right
    private int curPos;

    private bool isPanning = false;
    private int panningTime = 25; // one second
    private int count = 0;
    private RectTransform bgRect = null;
    private Vector3 from;
    private Vector3 to;

    // called 50 times a second regardless of frameRate
    private void FixedUpdate()
    {
        if (isPanning)
        {
            count++;
            bgRect.anchoredPosition = Vector3.Lerp(from, to, (float)count / panningTime);
            if (count == panningTime)
            {
                count = 0;
                isPanning = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 1 = room center
        curPos = 1;
    }

    public void CenterAndEnableLooking()
    {
        EnableLeft();
        EnableRight();
        curPos = -1;
        SetPosition(1);
        lookingEnabled = true;
    }

    public void DisableLooking()
    {
        DisableLeft();
        DisableRight();
        curPos = -1;
        SetPosition(1);
        lookingEnabled = false;
    }

    public void LookLeft()
    {
        if (!leftIsDisabled && lookingEnabled)
        {
            int pos = curPos - 1;
            if (pos >= 0)
            {
                SetPosition(pos);
                if (pos == 0)
                {
                    DisableLeft();
                }
                if (rightIsDisabled)
                {
                    EnableRight();
                }
            }
        }
    }

    public void LookRight()
    {
        if (!rightIsDisabled && lookingEnabled)
        {
            int pos = curPos + 1;
            if (pos <= 2)
            {
                SetPosition(pos);
                if (pos == 2)
                {
                    DisableRight();
                }
                if (leftIsDisabled)
                {
                    EnableLeft();
                }
            }
        }
    }

    public void SetPosition(int pos)
    {
        
        GameObject background = canvasSwapper.currentCanvas.transform.GetChild(0).gameObject;
        bgRect = background.GetComponent<RectTransform>();

        from = bgRect.anchoredPosition;

        if (pos != curPos)
        {
            // set room left
            if (pos == 0)
            {
                to = new Vector3((bgRect.rect.width - 1600) / 2, 0, 0);
            }
            // set room center
            else if (pos == 1)
            {
                to = new Vector3(0, 0, 0);
            }
            // set room right
            else if (pos == 2)
            {
                to = new Vector3(-(bgRect.rect.width - 1600) / 2, 0, 0);
            }
            isPanning = true;
            curPos = pos;
        }
    }

    public void DisableBack()
    {
        backIsDisabled = true;
        BackButton.GetComponent<Image>().color = Color.gray;
        BackButton.GetComponent<Button>().interactable = false;
    }

    public void EnableBack()
    {
        backIsDisabled = false;
        BackButton.GetComponent<Image>().color = Color.white;
        BackButton.GetComponent<Button>().interactable = true;
    }

    private void EnableRight()
    {
        RightButton.SetActive(true);
        RightButton.GetComponent<Image>().color = Color.white;
        rightIsDisabled = false;
        RightButton.GetComponent<Button>().interactable = true;
    }

    private void DisableRight()
    {
        RightButton.GetComponent<Image>().color = Color.clear;
        rightIsDisabled = true;
        RightButton.GetComponent<Button>().interactable = false;
    }

    private void EnableLeft()
    {
        LeftButton.SetActive(true);
        LeftButton.GetComponent<Image>().color = Color.white;
        leftIsDisabled = false;
        LeftButton.GetComponent<Button>().interactable = true;
    }

    private void DisableLeft()
    {
        LeftButton.GetComponent<Image>().color = Color.clear;
        LeftButton.GetComponent<Button>().interactable = false;
        leftIsDisabled = true;
    }
}
