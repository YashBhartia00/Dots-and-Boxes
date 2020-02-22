using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDots : MonoBehaviour
{
    public GameObject dotPrefab, dotParent, linePrefab, lineParent, boxPrefab, boxParent;
    GameObject[,,] gridLines = new GameObject[2, 11, 9]; //1st for H[0] or V[1], (0,0) startingform leftbottom
    public int[] gridSize = new int[2] { 5, 4 };
    public boardDots thisBoard = new boardDots(new int[2, 2, 2], 1);
    SpriteRenderer[,] boxes = new SpriteRenderer[1, 1];
    public int playerNumber;
    public bool move;

    //Color blue, red, violet, green,blank;
    public static Color[] BRVG = new Color[5];

    private void Start()
    {
        gridLines = new GameObject[2, 2 * gridSize[0] + 1, 2 * gridSize[1] + 1];
        thisBoard = new boardDots(new int[2, 2 * gridSize[0] + 1, 2 * gridSize[1] + 1], 2);
        boxes = new SpriteRenderer[gridSize[0] * 2, gridSize[1] * 2];
        initializeColors();
        makeBoard(gridSize);
        InvokeRepeating("plotBoardLines", 1, 2);
    }
    private void Update()
    {
        playerNumber = Input.GetKeyDown(KeyCode.A) ? 1 : Input.GetKeyDown(KeyCode.B) ? 2 : Input.GetKeyDown(KeyCode.C) ? 3 : Input.GetKeyDown(KeyCode.D) ? 4 : 0;
        print(playerNumber);
    }
    void initializeColors()
    {
        ColorUtility.TryParseHtmlString("#0048FF", out BRVG[0]);
        ColorUtility.TryParseHtmlString("#FF002D", out BRVG[1]);
        ColorUtility.TryParseHtmlString("#F300FF", out BRVG[2]);
        ColorUtility.TryParseHtmlString("#00CC1F", out BRVG[3]);
        ColorUtility.TryParseHtmlString("#FFFFFF", out BRVG[3]);
        BRVG[0].a = 0.5f;
        BRVG[1].a = 0.5f;
        BRVG[2].a = 0.5f;
        BRVG[3].a = 0.5f;
        BRVG[4].a = 0f;
    }

    void makeBoard(int[] gs)
    {
        Vector3 temp = new Vector3(0, thisBoard.gridLine.GetLength(1) - 1, thisBoard.gridLine.GetLength(2) - 1);
        for (int i = -gs[0]; i <= gs[0]; i++)
        {
            for (int j = -gs[1]; j <= gs[1]; j++)
            {
                Instantiate(dotPrefab, new Vector3(i, j, 0), Quaternion.identity, dotParent.transform);

            }
        } //for dots
        for (int i = -gs[0]; i < gs[0]; i++) //for H
        {
            for (int j = -gs[1]; j <= gs[1]; j++)
            {
                var a = Instantiate(linePrefab, new Vector3(i + 0.5f, j, 0), Quaternion.identity, lineParent.transform);
                a.transform.up = new Vector2(1, 0);
                gridLines[0, i + gs[0], j + gs[1]] = a;
            }
        } //for H
        for (int i = -gs[0]; i <= gs[0]; i++) //for V
        {
            for (int j = -gs[1]; j < gs[1]; j++)
            {
                var a = Instantiate(linePrefab, new Vector3(i, j + 0.5f, 0), Quaternion.identity, lineParent.transform);
                gridLines[1, i + gs[0], j + gs[1]] = a;
            }
        } //for V
        for (int i = 0; i < temp.y; i++)
        {
            for (int j = 0; j < temp.z; j++)
            {
                var a = Instantiate(boxPrefab, new Vector3(i - 5 + 0.5f, j - 4 + 0.5f, 0), Quaternion.identity, boxParent.transform);
                boxes[i, j] = a.GetComponent<SpriteRenderer>();
                boxes[i, j].color = BRVG[4];
            }
        }

    }
    void plotBoardLines()
    {
        thisBoard.randomize();
        for (int j = 0; j + 1 < thisBoard.gridLine.GetLength(1); j++)
        {
            for (int k = 0; k < thisBoard.gridLine.GetLength(2); k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    gridLines[0, j, k].transform.GetChild(i).gameObject.SetActive(false);
                }
                gridLines[0, j, k].transform.GetChild(thisBoard.gridLine[0, j, k] - 1).gameObject.SetActive(true);
            }
        }
        for (int j = 0; j < thisBoard.gridLine.GetLength(1); j++)
        {
            for (int k = 0; k + 1 < thisBoard.gridLine.GetLength(2); k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    gridLines[1, j, k].transform.GetChild(i).gameObject.SetActive(false);
                }
                gridLines[1, j, k].transform.GetChild(thisBoard.gridLine[1, j, k] - 1).gameObject.SetActive(true);
            }
        }
        plotBoxes();
    }
    void plotBoxes()
    {
        Vector3 temps = new Vector3(0, thisBoard.gridLine.GetLength(1) - 1, thisBoard.gridLine.GetLength(2) - 1);
        for (int i = 0; i < temps.y; i++)
        {
            for (int j = 0; j < temps.z; j++)
            {
                boxes[i, j].color = BRVG[4];
                if (thisBoard.gridLine[0, i, j] == thisBoard.gridLine[0, i, j + 1])
                {
                    if (thisBoard.gridLine[1, i, j] == thisBoard.gridLine[1, i + 1, j])
                    {
                        if (thisBoard.gridLine[0, i, j] == thisBoard.gridLine[1, i, j])
                        {

                            boxes[i, j].color = BRVG[thisBoard.gridLine[0, i, j] - 1];
                        }
                    }
                }
            }
        }
    }
}
