using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour
{
    //public bool isPlaced = false;
    public int gridX, gridY;
    public List<int> gridYvalues;
    int[] backupValues; // A array of values for keeping a backup version of values.


    public void Init(int x)
    {
        gridX = x;
        gridYvalues = new List<int> {0, 1, 2, 3, 4, 5, 6, 7};
    }

    /// <summary>
    /// Make a backup array version of values.
    /// </summary>
    public void Backup()
    {
        //Debug.Log("<color=green> ---- Backup ---- </color>");
        //PrintValues();
        backupValues = new int[gridYvalues.Count];
        gridYvalues.CopyTo(backupValues);
    }

    /// <summary>
    /// set List of values equal to back up array.
    /// </summary>
    public void RestoreBackup() 
    {
        //print(this.name + "<color=yellow> Backup values restored </color>");
        gridYvalues.Clear();
        gridYvalues.AddRange(backupValues);
        //PrintValues();
    }
    public void RemoveFromValues(int value) // It is a seperate function because of debug purposes.
    {
        //print(value + "<color=black> is removed from </color>" + name);
        gridYvalues.Remove(value);
    }

    public void PrintValues() //for Debug
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


}
