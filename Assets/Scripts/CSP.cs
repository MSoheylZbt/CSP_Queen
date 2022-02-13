using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSP : MonoBehaviour
{
    [SerializeField] ChessGrid gridRef;
    [SerializeField] Queen queenPref;

    public List<Queen> unassignedQueens = new List<Queen>(); //Variables
    public List<Queen> assignedQueens = new List<Queen>();



    Queue<Arc> arcs = new Queue<Arc>();
    Arc[,] arcsTable = new Arc[8, 8];

    bool isFirstRun = true;
    int expandedCount = 0;

    private void Start()
    {
        InitProblem();
        //AC3();
        SolveQueenProblem();
        SetQueenPositions();
    }


    void InitProblem()
    {
        InitQueens();
        InitArcs_Neighbors();
    }


    private void InitQueens()
    {
        for (int i = 0; i < 8; i++)
        {
            Queen queen = Instantiate(queenPref, this.transform);
            queen.Init(i);
            queen.name = "Queen " + i;
            unassignedQueens.Add(queen);
        }
    }

    private void InitArcs_Neighbors()
    {
        for (int i = 0; i < 8; i++)
        {
            Queen left = unassignedQueens[i];

            for (int j = 0; j < 8; j++)
            {
                if (left == unassignedQueens[j])
                    continue;

                Arc temp = new Arc(left, unassignedQueens[j]);
                arcs.Enqueue(temp);
                arcsTable[i, j] = temp;

                //print("Left is " + left + " Right is " + unassignedQueens[j] + " at " + i + " " + j);
            }
        }

    }


    bool SolveQueenProblem()
    {
        Queen selectedQueen = SelectUnassignedQueen();
        //print(selectedQueen.name);

        expandedCount++;

        for (int y = 0; y < selectedQueen.gridYvalues.Count; y++)
        {
            selectedQueen.gridY = selectedQueen.gridYvalues[y];

            if (RemoveIncosistentValues(selectedQueen))
                continue;

            //AC3();

            if (CheckForConsistency(selectedQueen)) // for assigned variables
            {
                //print(selectedQueen.name + " <color=magenta> With </color>" + 
                //    selectedQueen.gridYvalues[y] + " selected");

                unassignedQueens.Remove(selectedQueen);
                assignedQueens.Add(selectedQueen);

                if (unassignedQueens.Count <= 0)
                {
                    print("Problem Solved with " + expandedCount + " expanded nodes");
                    return true;
                }

                bool result = SolveQueenProblem();
                if (result)
                    return result;

                //print("<color=green> Change Selected Value for </color>" + selectedQueen.name);

                unassignedQueens.Add(selectedQueen);
                assignedQueens.Remove(selectedQueen);

                RestoreIncosistentValues(selectedQueen);
            }
        }
        return false;
    }




    //TODO : Use degree instead => we are not deleting any value so we should use degree.
    /// <summary>
    /// Using MRV
    /// </summary>
    Queen SelectUnassignedQueen()
    {
        int minLength = int.MaxValue;
        isFirstRun = false;

        foreach (Queen tempQueen in unassignedQueens)
        {
            if(tempQueen.gridYvalues.Count < minLength)
            {
                minLength = tempQueen.gridYvalues.Count;
                return tempQueen;
            }
        }
        return null;
    }

    bool CheckForConsistency(Queen queen)
    {
        int queenX = queen.gridX;
        int queenY = queen.gridY;


        foreach (Queen tempQueen in assignedQueens)
        {
            if (queen.gameObject == tempQueen.gameObject)
                continue;

            bool diagonal = Mathf.Abs(tempQueen.gridX - queenX) == Mathf.Abs(tempQueen.gridY - queenY);
            if (diagonal || (tempQueen.gridY == queenY))
            {
                //print( "for "+ queen.name + " with value of  " + queen.gridY + " " + tempQueen.name + " at " + tempQueen.gridX + " " + tempQueen.gridY+ " is not consistent " );
                return false;
            }
            else
                continue;
        }
        //print( queen.name + " at " + queenX + " " + queenY + " is consistent " );
        return true;
    }


    bool RemoveIncosistentValues(Queen queen)
    {
        int queenX = queen.gridX;
        int queenY = queen.gridY;

        foreach (Queen tempQueen in unassignedQueens)
        {
            if (queen.gameObject == tempQueen.gameObject)
                continue;

            if (tempQueen.gridYvalues.Count <= 0)
                continue;

            tempQueen.Backup();

            tempQueen.gridYvalues.Remove(queenY);

            // Delta X of two threating queen is equal to their Delta Y.
            int diagonal_1 = Mathf.Abs(queenX - tempQueen.gridX) + queenY; 
            int diagonal_2 = queenY - Mathf.Abs(queenX - tempQueen.gridX); 
            tempQueen.gridYvalues.Remove(diagonal_1);
            tempQueen.gridYvalues.Remove(diagonal_2);

            if (tempQueen.gridYvalues.Count <= 0)
            {
                tempQueen.RestoreBackup();
                return true; // Reach blank set.
            }

            //print(queenY + "<color=red> Removed from </color> " + tempQueen.name);
            //print(diagonal_1 + "<color=red> Removed from </color>" + tempQueen.name);
            //print(diagonal_2 + "<color=red> Removed from </color>" + tempQueen.name);
        }

        return false;
    }

    void RestoreIncosistentValues(Queen queen)
    {
        int queenX = queen.gridX;
        int queenY = queen.gridY;
        foreach (Queen tempQueen in unassignedQueens)
        {
            if (queen.gameObject == tempQueen.gameObject)
                continue;

            if (tempQueen.gridYvalues.Count <= 0)
                continue;

            if (!tempQueen.gridYvalues.Contains(queenY))
                tempQueen.gridYvalues.Add(queenY);

            int diagonal_1 = Mathf.Abs(queenX - tempQueen.gridX) + queenY;
            if (!tempQueen.gridYvalues.Contains(diagonal_1) && diagonal_1 < 8)
                tempQueen.gridYvalues.Add(diagonal_1);

            int diagonal_2 = queenY - Mathf.Abs(queenX - tempQueen.gridX);
            if(!tempQueen.gridYvalues.Contains(diagonal_2) && diagonal_2 > 0 && diagonal_2 < 8)
                tempQueen.gridYvalues.Add(diagonal_2);
        }
    }



    void AC3()
    {
        while(arcs.Count > 0)
        {
            Arc checkingArc = arcs.Dequeue();
            if(checkingArc.DeleteInconsitentValues())
            {
                AddNeighborArcs(checkingArc);
            }
        }
    }

    void AddNeighborArcs(Arc mainArc)
    {
        int rightQueenX = mainArc.GetRightQueenX();

        for (int i = 0; i < 8; i++)
        {
            if(arcsTable[i, rightQueenX] != null) //for i = rightQueenX this is always null.
             arcs.Enqueue(arcsTable[i, rightQueenX]);
        }
    }


    private void PrintAssigned()
    {
        foreach (Queen queen in assignedQueens)
        {
            print(queen.name + "<color=yellow> is assigned at </color>" + queen.gridX + " " + queen.gridY);
        }
    }

    private void PrintUnassigned()
    {
        foreach (Queen queen in unassignedQueens)
        {
            print(queen.name + " is not assigned");
        }
    }

    void SetQueenPositions()
    {
        foreach (Queen queen in assignedQueens)
        {
            queen.transform.position = gridRef.board[queen.gridX, queen.gridY];
        }
    }

}
