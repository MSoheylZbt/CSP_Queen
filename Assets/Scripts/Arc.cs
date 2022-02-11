using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc
{
    Queen leftQueen;
    Queen rightQueen;

    public Arc(Queen leftToSet,Queen rightToSet)
    {
        leftQueen = leftToSet;
        rightQueen = rightToSet;
    }

    public bool CheckforConsistency()
    {
        return true;
    }

    public void PrintArc()
    {
        Debug.Log(leftQueen.name + "<color=red> To </color>" + rightQueen.name );
    }


    public int GetRightQueenIndex()
    {
        return rightQueen.gridX;
    }
}
