using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChessGrid : MonoBehaviour //inherited from MonoBehaviour. This will allow us to use this class as a component in unity editor.
{
    [SerializeField] float tileDistanceMultiplier = 10;
    [SerializeField] GameObject square;

    public Vector2[,] board = new Vector2[8, 8];//2D array of positions for keeping grid.

    private void Awake() // This Function will run at Start of the game.
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        Vector2 firstPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)); // Convert Screen position of (0,0) which is most down-left point on screen, to a world position.
                                                                              //World position is position of any object in game world.

        Vector2 tileDistance = CalculateTileDistance(); //Distance between each cell in grid according to number of them.

        for (int i = 0; i < 8; i++) // Loop through all grids cell and make (Instantiate) a cell in each position
        {
            for (int j = 0; j < 8; j++)
            {
                Vector2 pos = Vector2.zero;
                pos.x = (firstPos.x + 0.5f) - i * tileDistance.x * tileDistanceMultiplier; // 0.25 is distance from most down-left point of camera.
                pos.y = (firstPos.y + 0.5f) - j * tileDistance.y * tileDistanceMultiplier;
                board[i, j] = pos;
                GameObject temp = Instantiate(square,pos,Quaternion.identity,transform);
                if((j+i) % 2 == 0)
                    temp.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }

    private Vector2 CalculateTileDistance()
    {
        Vector2 firstPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 temp = Vector2.zero; //Vector2 is used for not indicating a direction or position but just for holding two numbers.

        /// FIXED  => 25 to 8

        temp.x = firstPos.x / 8;
        temp.y = firstPos.y / 8;
        return temp;
    }

}
