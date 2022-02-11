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
    Arc[,] neighbors = new Arc[8, 8];

    bool isFirstRun = true;

    private void Start()
    {
        InitProblem();
        AC3();
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
                if (left == unassignedQueens[i])
                    continue;

                Arc temp = new Arc(left, unassignedQueens[j]);
                arcs.Enqueue(temp);
                neighbors[i, j] = temp;
            }
        }

    }


    bool SolveQueenProblem()
    {
        Queen selectedQueen = SelectUnassignedQueen();

        if(!selectedQueen && !isFirstRun)
        {
            print("Problem Solved !");
            return true;
        }

        //print(selectedQueen.name + " <color=magenta> Selected </color>");

        for (int y = 0; y < selectedQueen.gridYvalues.Count; y++)
        {
            selectedQueen.gridY = y;

            if(CheckForConsistency(selectedQueen))
            {
                unassignedQueens.Remove(selectedQueen);
                //print(selectedQueen.name + " <color=red>  Removed from unassigned </color>");    
                assignedQueens.Add(selectedQueen);
                //print(selectedQueen.name + "<color=green> Added to assigned </color>");    

                bool result = SolveQueenProblem();
                if (result)
                    return result;

                unassignedQueens.Add(selectedQueen);
                //print(selectedQueen.name + "<color=green> Added to unassigned </color>");    
                assignedQueens.Remove(selectedQueen);
                //print(selectedQueen.name + "<color=red> Removed from assigned </color>");    
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

        //PrintAssigned();

        foreach (Queen tempQueen in assignedQueens)
        {
            if (queen.gameObject == tempQueen.gameObject)
                continue;

            bool diagonal = Mathf.Abs(tempQueen.gridX - queenX) == Mathf.Abs(tempQueen.gridY - queenY);
            if (diagonal || (tempQueen.gridX == queenX) || (tempQueen.gridY == queenY))
            {
                //print( queen.name + " at " + queenX + " " + queenY + " is not consistent " );
                return false;
            }
            else
                continue;
        }
        //print( queen.name + " at " + queenX + " " + queenY + " is consistent " );
        return true;
    }

    bool DeleteInconsitentValues(Queen queen)
    {
        int queenX = queen.gridX;
        int queenY = queen.gridY;
        foreach (Queen tempQueen in unassignedQueens)
        {
            tempQueen.gridYvalues.Remove(queenY);
            int diagonal = Mathf.Abs(queenX - tempQueen.gridX) + queenY; // Delta X of two threating queen is equal to their Delta Y.
            tempQueen.gridYvalues.Remove(diagonal);

            if (tempQueen.gridYvalues.Count <= 0)
            {
                return false; // We reach a blank set
            }
        }

        return true;

    }

    void AC3()
    {
        while(arcs.Count > 0)
        {
            Arc checkingArc = arcs.Dequeue();
            if(checkingArc.CheckforConsistency())
            {
                AddNeighborArcs(checkingArc);
            }
        }
    }

    void AddNeighborArcs(Arc mainArc)
    {
        for (int i = 0; i < 8; i++)
        {
            
        }
    }

    private void PrintAssigned()
    {
        foreach (Queen queen in assignedQueens)
        {
            print(queen.name + "<color=yellow> is assigned </color>");
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
