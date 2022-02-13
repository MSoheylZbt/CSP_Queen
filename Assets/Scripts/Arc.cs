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

            rightQueen.Backup();

            //for each value of leftQueen,delete all inconsitent value from rightQueen.
            rightQueen.gridYvalues.Remove(checkingValue);
            // Delta X of two threating queen is equal to their Delta Y.
            int diagonal_1 = Mathf.Abs(leftQueen.gridX - rightQueen.gridX) + checkingValue; 
            rightQueen.gridYvalues.Remove(diagonal_1);
            int diagonal_2 = checkingValue - Mathf.Abs(leftQueen.gridX - rightQueen.gridX);
            rightQueen.gridYvalues.Remove(diagonal_2);

            PrintArc();
            Debug.Log(rightQueen.gridYvalues.Count);


            if (rightQueen.gridYvalues.Count <= 0)// It means checkingValue is inconsistent.
            {
                leftQueen.gridYvalues.Remove(checkingValue);
                PrintArc();
                Debug.Log(checkingValue + " from " + leftQueen.name);
                result = true;
            }

            rightQueen.RestoreBackup();
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
