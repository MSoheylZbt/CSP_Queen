using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc
{
    Queen leftQueen;
    Queen rightQueen;

    public bool isBlankReached = false;

    public Arc(Queen leftToSet,Queen rightToSet)
    {
        leftQueen = leftToSet;
        rightQueen = rightToSet;

    }

    public bool CheckForConsistency()
    {

        bool isValueDeleted = false;

        //Debug.Log("<color=red> ---- New Call ---- </color>");
        //PrintArc();
        List<int> tempLeftValues = MakeTempValues(leftQueen);


        for (int i = 0; i < leftQueen.gridYvalues.Count; i++)
        {
            int checkingValue = leftQueen.gridYvalues[i];

            List<int> tempRightValues = MakeTempValues(rightQueen);

            //Debug.Log("checkingValue: " + checkingValue);



            //for each value of leftQueen,delete all inconsitent value from rightQueen.
            tempRightValues.Remove(checkingValue);

            // Delta X of two threating queen is equal to their Delta Y.
            int diagonal_1 = Mathf.Abs(leftQueen.gridX - rightQueen.gridX) + checkingValue;
            tempRightValues.Remove(diagonal_1);
            int diagonal_2 = checkingValue - Mathf.Abs(leftQueen.gridX - rightQueen.gridX);
            tempRightValues.Remove(diagonal_2);

            if (tempRightValues.Count <= 0)// It means checkingValue is inconsistent.
            {
                tempLeftValues.Remove(checkingValue);
                if (tempLeftValues.Count <= 0)
                {
                    isBlankReached = true;
                    return false;
                }
                else
                    leftQueen.RemoveFromValues(checkingValue);

                isValueDeleted = true;
            }
        }

        return isValueDeleted;
    }

    private List<int> MakeTempValues(Queen queen)
    {
        int[] temp = new int[queen.gridYvalues.Count];
        queen.gridYvalues.CopyTo(temp);
        List<int> tempRightValues = new List<int>();
        tempRightValues.AddRange(temp);
        return tempRightValues;
    }

    public void PrintArc()
    {
        Debug.Log(leftQueen.name + "<color=red> To </color>" + rightQueen.name );
    }

    public Queen GetLeftQueen()
    {
        return leftQueen;
    }


    public Queen GetRightQueen()
    {
        return rightQueen;
    }


}
