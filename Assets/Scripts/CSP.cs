using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSP : MonoBehaviour
{
    public const int _queenNumber = 8;
    int expandedCount = 0;// Total number of expanded nodes in Backtracking algorithm

    #region References
    [SerializeField] ChessGrid gridRef; // for showing queen on board.
    [SerializeField] Queen queenPref; // for spawning
    #endregion

    #region Recursive Algorithms
    public List<Queen> unassignedQueens = new List<Queen>(); //A list of problem variables (initial assignement)
    public List<Queen> assignedQueens = new List<Queen>(); // A list of all variables that have a value (all placed queens)
    #endregion

    #region AC3
    Queue<Arc> arcs = new Queue<Arc>(); 
    Arc[,] arcsTable = new Arc[_queenNumber, _queenNumber]; //a Table for keeping all arcs according to their Queen grid position.
                                                                // Used for finding neighbors in AC3 algorithm.
    #endregion


    private void Start() // this function runs at start of game.
    {
        InitProblem();
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
        for (int i = 0; i < _queenNumber; i++)
        {
            Queen queen = Instantiate(queenPref, this.transform); // Spawn a queen in game world.
            queen.Init(i);
            unassignedQueens.Add(queen); // at first all queens will added to Unassigned list.
            //queen.name = "Queen " + i;
        }
    }

    private void InitArcs_Neighbors() // Initial All arcs and add them to a queue and a table
    {
        for (int i = 0; i < _queenNumber; i++)
        {
            Queen left = unassignedQueens[i];

            for (int j = 0; j < _queenNumber; j++)
            {
                if (left == unassignedQueens[j]) // Don't make arcs with same left and right variable.
                    continue;

                Arc temp = new Arc(left, unassignedQueens[j]);
                arcs.Enqueue(temp);
                arcsTable[i, j] = temp;

                //print("Left is " + left + " Right is " + unassignedQueens[j] + " at " + i + " " + j);
            }
        }

    }

    /// <summary>
    /// This function Solve N-Queen problem with using of Recursive Backtracking.
    /// For choosing Variable, MRV heuristic used.
    /// For choosing Value, Arc Consistency used.
    /// </summary>
    /// <returns></returns>
    bool SolveQueenProblem()
    {
        Queen selectedQueen = SelectUnassignedQueen();
        //print(selectedQueen.name);

        expandedCount++;

        if (unassignedQueens.Count <= 0) // Check for reaching soloution
        {
            print("Problem Solved with " + expandedCount + " expanded nodes");
            return true;
        }


        for (int y = 0; y < selectedQueen.gridYvalues.Count; y++)
        {
            selectedQueen.gridY = selectedQueen.gridYvalues[y]; // Select a variable.

            //print("<color=yellow> Selected value : </color>" + selectedQueen.gridYvalues[y] + " for " + selectedQueen.name);
            //selectedQueen.PrintValues();

            if (RemoveIncosistentValues(selectedQueen))
                continue; // if We reach a blank set, try next value.
            else
            {
                if (AC3(selectedQueen) == false)
                {
                    //print("AC3 reach blank");
                    selectedQueen.RemoveFromValues(selectedQueen.gridYvalues[y]);
                }
            }

            if (CheckForConsistency(selectedQueen)) // for assigned variables
            {
                //print(selectedQueen.name + " <color=magenta> With </color>" + 
                //    selectedQueen.gridYvalues[y] + " selected");

                unassignedQueens.Remove(selectedQueen);
                assignedQueens.Add(selectedQueen);

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

    /// <summary>
    /// Using MRV
    /// </summary>
    Queen SelectUnassignedQueen()
    {
        int minLength = int.MaxValue;
        Queen selectedQueen = new Queen();
        foreach (Queen tempQueen in unassignedQueens) // Loop through all un-assigned Variables
        {
            //print(" for " + tempQueen.name + " is " + tempQueen.gridYvalues.Count);
            if (tempQueen.gridYvalues.Count <= minLength)
            {
                minLength = tempQueen.gridYvalues.Count; // Choose a variable with minimum Remained values.
                selectedQueen = tempQueen;
                continue;
            }
        }
        //print("<color=green> min: </color>" + selectedQueen.name);
        return selectedQueen;
    }

    /// <summary>
    /// This function check consistency with all assigned variable
    /// </summary>
    /// <param name="queen"></param>
    /// <returns></returns>
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

    /// <summary>
    /// This function take a Queen with a selected value as input
    /// and with this value, checks all unassigned variables and delete inconsistent values from them.
    /// Returning true means we reach a blank set and we should try a different value.
    /// </summary>
    /// <param name="queen"></param>
    /// <returns></returns>
    bool RemoveIncosistentValues(Queen queen)
    {
        int queenX = queen.gridX;
        int queenY = queen.gridY;

        foreach (Queen tempQueen in unassignedQueens)
        {
            if (queen.gameObject == tempQueen.gameObject) // Do not check Incosistency for same queen.
                continue;

            // when we reach a blank set of values by removing inconsistent values,
                //we should restore those values.
            tempQueen.Backup();

            //Calcualte possible threating values in tempQueen then remove them.

            tempQueen.RemoveFromValues(queenY);
            // Delta X of two threating queen is equal to their Delta Y.
            int diagonal_1 = Mathf.Abs(queenX - tempQueen.gridX) + queenY; 
            int diagonal_2 = queenY - Mathf.Abs(queenX - tempQueen.gridX); 
            tempQueen.RemoveFromValues(diagonal_1);
            tempQueen.RemoveFromValues(diagonal_2);

            if (tempQueen.gridYvalues.Count <= 0)
            {
                tempQueen.RestoreBackup();
                return true; // Reach blank set.
            }
        }

        return false;
    }

    /// <summary>
    /// With using Arc Consistency algorithm, this function try to remove all incosistent values.
    /// Take Queen with a selected value as input.
    /// if reachs a blank set of value, returns false.
    /// </summary>
    /// <param name="checkingQueen"></param>
    /// <returns></returns>
    bool AC3(Queen checkingQueen)
    {
        while(arcs.Count > 0)
        {
            Arc checkingArc = arcs.Dequeue();
            //If queen with selected value is same as starting queen of this arc, then don't check this arc.
            if (checkingArc.GetLeftQueen() == checkingQueen)
                continue;

            //Debug.Log("<color=magenta> Backup Called ---- </color>");

            // When we reach a blank set by checking Arcs, then we need to restore deleted values.
            checkingArc.GetRightQueen().Backup();
            checkingArc.GetLeftQueen().Backup();

            if (checkingArc.CheckForConsistency())
                AddNeighborArcs(checkingArc);
            else
            {
                if(checkingArc.isBlankReached)
                {
                    checkingArc.GetLeftQueen().RestoreBackup();
                    checkingArc.GetRightQueen().RestoreBackup();
                    return false; // We reach ab blank set.
                }
            }
        }
        return true;
    }

    /// <summary>
    /// This function take a queen with selected value as input and resotre all possible incosistent-
    /// -values in all unassigned variables.
    /// Used when algorithms backtrack from a wrong selected value.
    /// </summary>
    /// <param name="queen"></param>
    void RestoreIncosistentValues(Queen queen)
    {
        //print("<color=magenta> Resotring for </color>" + queen.name);
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
            if (!tempQueen.gridYvalues.Contains(diagonal_1) && diagonal_1 < _queenNumber)
                tempQueen.gridYvalues.Add(diagonal_1);

            int diagonal_2 = queenY - Mathf.Abs(queenX - tempQueen.gridX);
            if(!tempQueen.gridYvalues.Contains(diagonal_2) && diagonal_2 > 0 && diagonal_2 < _queenNumber)
                tempQueen.gridYvalues.Add(diagonal_2);
        }
    }

    /// <summary>
    /// All arcs with same right queen are neighbors and this function add all of them.
    /// </summary>
    /// <param name="mainArc"></param>
    void AddNeighborArcs(Arc mainArc)
    {
        int rightQueenX = mainArc.GetRightQueen().gridX;

        for (int i = 0; i < _queenNumber; i++)
        {
            if(arcsTable[i, rightQueenX] != null) //for i = rightQueenX this is always null.
            {
                //print("Neighbor : ");
                //arcsTable[i, rightQueenX].PrintArc();
                arcs.Enqueue(arcsTable[i, rightQueenX]);
            }

        }
    }

    /// <summary>
    /// This function set world position of queens according to their 
    /// </summary>
    void SetQueenPositions()
    {
        foreach (Queen queen in assignedQueens)
        {
            queen.transform.position = gridRef.board[queen.gridX, queen.gridY];
        }
    }

    private void PrintAssigned() //for debug
    {
        foreach (Queen queen in assignedQueens)
        {
            print(queen.name + "<color=yellow> is assigned at </color>" + queen.gridX + " " + queen.gridY);
        }
    }

    private void PrintUnassigned() // for debug
    {
        foreach (Queen queen in unassignedQueens)
        {
            print(queen.name + " is not assigned");
        }
    }



}
