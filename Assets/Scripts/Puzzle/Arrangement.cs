﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Arrangement : MonoBehaviour
{
    public int width;
    public int height;
    public UnityEvent onWin;
    private Arrangable[][] arrangables;

    // Start is called before the first frame update
    void Start()
    {
        arrangables = new Arrangable[width][];
        for (int i = 0; i < width; i++)
        {
            arrangables[i] = new Arrangable[height];
        }

        foreach (Arrangable child in gameObject.GetComponentsInChildren<Arrangable>())
        {
            arrangables[child.currentPos.x][child.currentPos.y] = child;
        }

        Scramble(100);
    }

    public void Lock()
    {
        foreach (Arrangable[] arr in arrangables)
        {
            foreach (Arrangable a in arr)
            {
                a.locked = true;
                a.Deselect();
            }
        }
    }

    public void Check()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (arrangables[i][j].currentPos.x != arrangables[i][j].correctPos.x ||
                    arrangables[i][j].currentPos.y != arrangables[i][j].correctPos.y)
                {
                    //Debug.Log("Incorrect");
                    return;
                }
            }
        }
        //Debug.Log("Correct");
        Lock();
        Invoke("Win", 1.2f);
    }

    public void Win()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Flipable flip = arrangables[i][j].gameObject.GetComponent<Flipable>();
                if (flip)
                {
                    flip.Flip();
                }
            }
        }
        onWin.Invoke();
    }

    void Scramble(int swaps)
    {
        for (int i = 0; i < swaps; i++)
        {
            Arrangable one = arrangables[Random.Range(0, width)][Random.Range(0, height)];
            Arrangable two = arrangables[Random.Range(0, width)][Random.Range(0, height)];


            Vector2Int tmppos = one.currentPos;
            one.currentPos = two.currentPos;
            two.currentPos = tmppos;


            // swap objects
            Vector3 tmpposition = one.gameObject.transform.position;
            one.gameObject.transform.position = two.gameObject.transform.position;
            two.gameObject.transform.position = tmpposition;
        }
    }
}
