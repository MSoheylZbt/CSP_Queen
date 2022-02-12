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

    public bool DeleteInconsitentValues()
    {
        bool result = false;
        for (int i = 0; i < leftQueen.gridYvalues.Count;i++)
        {
            int checkingValue = leftQueen.gridYvalues[i];

            //Make a duplicate of Values of rightQueen (We can't use reference)
            int[] temp = new int[rightQueen.gridYvalues.Count];
            rightQueen.gridYvalues.CopyTo(temp);

            //for each value of leftQueen,delete all inconsitent value from rightQueen.
            rightQueen.gridYvalues.Remove(checkingValue);
            int diagonal = Mathf.Abs(leftQueen.gridX - rightQueen.gridX) + checkingValue; // Delta X of two threating queen is equal to their Delta Y.
            rightQueen.gridYvalues.Remove(diagonal);

            if(rightQueen.gridYvalues.Count <= 0)// It means checkingValue is inconsistent.
            {
                leftQueen.gridYvalues.Remove(checkingValue);
                PrintArc();
                Debug.Log(checkingValue + " from " + leftQueen.name);
                result = true;
            }

            // Restore right queen values.
            rightQueen.gridYvalues.Clear(); 
            rightQueen.gridYvalues.AddRange(temp);

        }

        return result;
    }

    public void PrintArc()
    {
        Debug.Log(leftQueen.name + "<color=red> To </color>" + rightQueen.name );
    }


    public int GetRightQueenX()
    {
        return rightQueen.gridX;
    }
}
