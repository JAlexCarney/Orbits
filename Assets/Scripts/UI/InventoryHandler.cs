﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryHandler : MonoBehaviour
{
    private GameObject openButton = null;
    private GameObject openedInventory = null;

    public int numSlots;
    private string[] items;
    private GameObject[] slots;
    private GameObject[] itemObjs;
    private bool open = false;
    private bool animating = false;

    private string collecting;

    [System.Serializable]
    public struct Pickupable {
        public string label;
        public Sprite invSprite;
    }

    public Pickupable[] pickupableObjs;
    private Dictionary<string, Sprite> pickupDict = new Dictionary<string, Sprite>();

    public void Start()
    {
        Util.player = 1;

        foreach(Pickupable pickupable in pickupableObjs)
        {
            pickupDict[pickupable.label] = pickupable.invSprite;
        }


        slots = new GameObject[numSlots];
        itemObjs = new GameObject[numSlots];
        items = new string[numSlots];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = "";
            slots[i] = transform.Find("Opened").Find("Background").Find("Slot" + i).gameObject;
            itemObjs[i] = slots[i].transform.Find("Item").gameObject;
        }
        openButton = transform.Find("Open").gameObject;
        openedInventory = transform.Find("Opened").gameObject;
    }

    public void Open()
    {
        if (!open && !animating)
        {
            Util.ActivateChildren(openedInventory);
            Animation anim = openedInventory.GetComponent<Animation>();
            anim["InventoryOpen"].speed = 1;
            anim["InventoryOpen"].time = 0;
            anim.Play("InventoryOpen");
            animating = true;
            Invoke("OpenDelayed", 0.5f);

        }
    }

    public void Toggle()
    {
        if (!open && !animating)
        {
            Open();
        }
        else if(!animating)
        {
            Close();
        }
    }

    public void Close()
    {
        if (open && !animating)
        {
            Animation anim = openedInventory.GetComponent<Animation>();
            anim["InventoryOpen"].speed = -1;
            anim["InventoryOpen"].time = anim["InventoryOpen"].length;
            anim.Play("InventoryOpen");
            animating = true;
            Invoke("CloseDelayed", 0.5f);
        }
    }

    public void CloseDelayed()
    {
        Util.DeactivateChildren(openedInventory);
        open = false;
        animating = false;
    }

    public void OpenDelayed()
    {
        animating = false;
        open = true;
    }

    public void Remove(string objToRemove)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].ToLower() == objToRemove.ToLower())
            {
                items[i] = "";
                itemObjs[i].GetComponent<Draggable>().label = "";
                itemObjs[i].GetComponent<Image>().color = Color.clear;
            }
        }
        if (Empty())
        {
            Close();
        }
    }

    public bool Empty()
    {
        foreach(string item in items)
        {
            if (item != "") { return false; }
        }
        return true;
    }

    public void Collect(string labelAndObj)
    {
        // make sure inventory is opened
        Open();
        
        // parse input
        List<string> input = Util.Split(labelAndObj, '+');
        string label = input[0];
        collecting = input[1];
        
        // Get reference to object being collected
        GameObject tmp = GameObject.Find(collecting);

        // Add reference to Open Slot
        int index = 0;
        while (items[index] != "") { index++; }
        items[index] = label;
        GameObject item = itemObjs[index];
        item.GetComponent<Draggable>().label = label;

        // play animation and destroy Pickup Object
        item.transform.position = tmp.transform.position;
        item.GetComponent<Draggable>().dropPos = tmp.transform.position;
        item.GetComponent<Draggable>().isGoingBack = true;
        item.GetComponent<Image>().sprite = pickupDict[label];
        item.GetComponent<Image>().color = Color.white;
        Destroy(tmp);
    }
}