﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageHint : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //drag stuff
    private bool isHeld = false;
    public Vector3 startPos;
    public Vector3 dropPos;
    public string label;
    public static bool justReleased = false;
    public int swapID;
    static public bool holding = false;
    static public string held = "";
    static public GameObject heldObj = null;
    private Touch touch;

    //page boundaries
    private Transform bottomLeftCorner;
    private Transform topRightCorner; 

    private Transform parent;

    [Header("Page # the sketch will be in")]
    public int pageNumber = 1; 

    private void Start()
    {
        label = gameObject.name;
        startPos = transform.position;
        parent = transform.parent;

        //get text children of the journal canvas
        //unity can't find inactive children using Find() :(
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            string name = child.gameObject.name;
            if (child != null && name.Equals("TopRight"))
            {
                topRightCorner = child;
            }
            else if (child != null && name.Equals("BottomLeft"))
            {
                bottomLeftCorner = child;
            }
        }
    }

    // Update is called once per frame
    void Update()
        {   
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
        }
        if (isHeld)
        {
            if (Input.touchCount == 0)
            {
                transform.position = new Vector2(Clamp(Input.mousePosition.x, bottomLeftCorner.position.x, topRightCorner.position.x),
                    Clamp(Input.mousePosition.y, bottomLeftCorner.position.y, topRightCorner.position.y));
            }
            else
            {
                transform.position = touch.position;
            }
        }
    }

    public void OnPointerDown(PointerEventData d)
    {
        //Debug.Log("Clicked");
        Hold();
    }

    public void OnPointerUp(PointerEventData d)
    {
        //Debug.Log("released");
        dropPos = transform.position;
        justReleased = true;
        Invoke("Drop", 0.1f);
    }

    public void Hold()
    {
        if (!holding)
        {
            isHeld = true;
            holding = true;
            held = label;
            heldObj = gameObject;
        }
    }

    public void Drop()
    {
        isHeld = false;
        holding = false;
        held = "";
        heldObj = null;
        justReleased = false;
    }

    //ty https://stackoverflow.com/questions/3176602/how-to-force-a-number-to-be-in-a-range-in-c/3176617
    public static float Clamp(float value, float min, float max)
    {
        return (value < min) ? min : (value > max) ? max : value;
    }

}
