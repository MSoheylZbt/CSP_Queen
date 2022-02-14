using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour
{
    //public bool isPlaced = false;
    public int gridX, gridY;
    public List<int> gridYvalues;
    int[] backupValues;

    SpriteRenderer spriteRenderer;

    public void Init(int x)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridX = x;
        gridYvalues = new List<int> {0, 1, 2, 3, 4, 5, 6, 7};
    }


    public void Backup()
    {
        //Debug.Log("<color=green> ---- Backup ---- </color>");
        //PrintValues();
        backupValues = new int[gridYvalues.Count];
        gridYvalues.CopyTo(backupValues);
    }


    public void RestoreBackup()
    {
        //print(this.name + "<color=yellow> Backup values restored </color>");
        gridYvalues.Clear();
        gridYvalues.AddRange(backupValues);
        //PrintValues();
    }

    public void PrintValues()
    {
        if (gridYvalues.Count <= 0)
        {
            //print("<color=black> List is Empty </color>");
            return;
        }

        foreach (int item in gridYvalues)
        {
            print(this.name + " " + item);
        }
    }

    public void RemoveFromValues(int value)
    {
        //print(value + "<color=black> is removed from </color>" + name);
        gridYvalues.Remove(value);
    }
}
