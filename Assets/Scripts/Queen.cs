using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour
{
    //public bool isPlaced = false;
    public int gridX, gridY;
    public List<int> gridYvalues;

    SpriteRenderer spriteRenderer;

    public void Init(int x)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridX = x;
        gridYvalues = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
    }
}
