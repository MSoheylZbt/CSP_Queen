using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc
{
    Queen leftQueen;
    Queen rightQueen;

    public bool isBlankReached = false; //for checking if we reach a blank sets of values.

    public Arc(Queen leftToSet,Queen rightToSet) //Constructor
    {
        leftQueen = leftToSet;
        rightQueen = rightToSet;

    }

    /// <summary>
    /// Loop through all values of left queen and see if there is any value in right queen that 
    /// satisfy constraint.
    /// </summary>
    /// <returns></returns>

    public bool CheckForConsistency()
    {
        //Debug.Log("<color=red> ---- New Call ---- </color>");
        //PrintArc();

        bool isValueDeleted = false;
        // Make a temporary list only for checking getting empty after deleting values.
        List<int> tempLeftValues = MakeTempValues(leftQueen);


        for (int i = 0; i < leftQueen.gridYvalues.Count; i++)
        {
            int checkingValue = leftQueen.gridYvalues[i];

            // It is here because, with each value of leftQueen, should be compared to all of right queen values.
            // ( for not deleting real values )
            List<int> tempRightValues = MakeTempValues(rightQueen);

            //Debug.Log("checkingValue: " + checkingValue);

            //for each value of leftQueen,delete all inconsitent value from rightQueen.
            tempRightValues.Remove(checkingValue);

            // Delta X of two threating queen is equal to their Delta Y.
            int diagonal_1 = Mathf.Abs(leftQueen.gridX - rightQueen.gridX) + checkingValue;
            tempRightValues.Remove(diagonal_1);
            int diagonal_2 = checkingValue - Mathf.Abs(leftQueen.gridX - rightQueen.gridX);
            tempRightValues.Remove(diagonal_2);

            // We delete all inconsistent values and we reach empty set,it means checkingValue is inconsistent.
            if (tempRightValues.Count <= 0)
            {
                tempLeftValues.Remove(checkingValue);
                if (tempLeftValues.Count <= 0)
                {
                    isBlankReached = true;
                    return false; // Arc consistency failled, it means we should try another value for selected variable in CSP.
                }
                else
                    leftQueen.RemoveFromValues(checkingValue); // Remove inconsistent value.

                isValueDeleted = true;
            }
        }

        return isValueDeleted;
    }

    /// <summary>
    /// This function returns a copy of a list without being direct reference to it.
    /// </summary>
    /// <param name="queen"></param>
    /// <returns></returns>
    private List<int> MakeTempValues(Queen queen)
    {
        int[] temp = new int[queen.gridYvalues.Count];
        queen.gridYvalues.CopyTo(temp);
        List<int> tempRightValues = new List<int>();
        tempRightValues.AddRange(temp);
        return tempRightValues;
    }

    public Queen GetLeftQueen()
    {
        return leftQueen;
    }


    public Queen GetRightQueen()
    {
        return rightQueen;
    }

    public void PrintArc() // for debug
    {
        Debug.Log(leftQueen.name + "<color=red> To </color>" + rightQueen.name );
    }


}
