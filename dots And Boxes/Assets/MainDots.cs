using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;

public class MainDots : MonoBehaviour
{
    public GameObject dotPrefab, dotParent, linePrefab, lineParent, boxPrefab, boxParent;
    SpriteRenderer[,,] gridLines = new SpriteRenderer[2, 11, 9]; //1st for H[0] or V[1], (0,0) startingform leftbottom
    public int[] gridSize = new int[2] { 5, 4 };
    public boardDots thisBoard = new boardDots(new int[2, 2, 2], 1, 1), randomBoard = new boardDots(new int[2, 2, 2], 1, 1);
    SpriteRenderer[,] boxes = new SpriteRenderer[1, 1];
    public static int playerNumber =2;
    public bool move, timeToSend;
    public static int moveNumberFromCollider =2 ;
    public int playerId;

    public int[,,] gridLineFoo;

    //Color  blank, blue, red, violet, green,;
    public static Color[] BRVG = new Color[5];


    private void Start()
    {
        gridLines = new SpriteRenderer[2, 2 * gridSize[0] + 1, 2 * gridSize[1] + 1];
        thisBoard = new boardDots(new int[2, 2 * gridSize[0] + 1, 2 * gridSize[1] + 1], 2,1);
        randomBoard = new boardDots(new int[2, 2 * gridSize[0] + 1, 2 * gridSize[1] + 1], 4,1);
        gridLineFoo = new int[2, 2 * gridSize[0] + 1, 2 * gridSize[1] + 1];
        boxes = new SpriteRenderer[gridSize[0] * 2, gridSize[1] * 2];
        initializeColors();
        makeBoard(gridSize);
       // StartCoroutine(Load());
        InvokeRepeating("plotBoardLines", 5, 0.3f);
        print(thisBoard.gridLine.Length);
        playerNumber = PlayerPrefs.GetInt("playerNumber");
        playerId = PlayerPrefs.GetInt("playerId");
        RestClient.Put("https://dots-68b2c.firebaseio.com/Playings/" + "Dots And Boxes" + "/" + playerId + ".json", thisBoard);

    }
    private void Update()
    {
        if (playerNumber == thisBoard.moves) { move = true; }else { move = false; }        
    }
    void initializeColors()
    {
        ColorUtility.TryParseHtmlString("#0048FF", out BRVG[1]);
        ColorUtility.TryParseHtmlString("#FF002D", out BRVG[2]);
        ColorUtility.TryParseHtmlString("#F300FF", out BRVG[3]);
        ColorUtility.TryParseHtmlString("#00CC1F", out BRVG[4]);
        ColorUtility.TryParseHtmlString("#FFFFFF", out BRVG[0]);
        BRVG[1].a = 0.5f;
        BRVG[2].a = 0.5f;
        BRVG[3].a = 0.5f;
        BRVG[4].a = 0.5f;
        BRVG[0].a = 0f;
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
                var b = a.GetComponent<SpriteRenderer>();
                gridLines[0, i + gs[0], j + gs[1]] = b;
            }
        } //for H
        for (int i = -gs[0]; i <= gs[0]; i++) //for V
        {
            for (int j = -gs[1]; j < gs[1]; j++)
            {
                var a = Instantiate(linePrefab, new Vector3(i, j + 0.5f, 0), Quaternion.identity, lineParent.transform);
                var b = a.GetComponent<SpriteRenderer>();
                gridLines[1, i + gs[0], j + gs[1]] = b;
            }
        } //for V
        for (int i = 0; i < temp.y; i++) //for Boxes
        {
            for (int j = 0; j < temp.z; j++)
            {
                var a = Instantiate(boxPrefab, new Vector3(i - 5 + 0.5f, j - 4 + 0.5f, 0), Quaternion.identity, boxParent.transform);
                boxes[i, j] = a.GetComponent<SpriteRenderer>();
                boxes[i, j].color = BRVG[0];
            }
        } //for Boxes

    }
    public void plotBoardLines()
    {
        //thisBoard.randomize();
       RestClient.Get<boardDots>("https://dots-68b2c.firebaseio.com/Playings/" + "Dots And Boxes" + "/" + playerId + ".json").Then(res =>
           {
               thisBoard.compactGridLines = res.compactGridLines;
               thisBoard.updategrids();
               thisBoard.moves = res.moves;



               
           }
        );
        for (int j = 0; j + 1 < gridLines.GetLength(1); j++)
        {
            for (int k = 0; k < gridLines.GetLength(2); k++)
            {
                gridLines[0, j, k].color = BRVG[thisBoard.gridLine[0, j, k]];
                thisBoard.gridLine[0, j, k] = gridLines[0, j, k].gameObject.GetComponent<LinesMove>().changeToValue;
                changBox(move, 0, j, k);
            }
        }
        for (int j = 0; j < gridLines.GetLength(1); j++)
        {
            for (int k = 0; k + 1 < gridLines.GetLength(2); k++)
            {
                gridLines[1, j, k].color = BRVG[thisBoard.gridLine[1, j, k]];
                thisBoard.gridLine[1, j, k] = gridLines[1, j, k].gameObject.GetComponent<LinesMove>().changeToValue;
                changBox(move, 1, j, k);
            }
        }

        thisBoard.updateCompact();
        thisBoard.moves = moveNumberFromCollider;

        if (timeToSend)
        {
            RestClient.Put("https://dots-68b2c.firebaseio.com/Playings/" + "Dots And Boxes" + "/" + playerId + ".json", thisBoard);
            timeToSend=false;
        }
        plotBoxes();
    }
    public void plotBoardLinesRand()
    {
            print("welp");
            randomBoard.randomize();
            for (int j = 0; j + 1 < gridLines.GetLength(1); j++)
            {
                for (int k = 0; k < gridLines.GetLength(2); k++)
                {
                    gridLines[0, j, k].color = BRVG[randomBoard.gridLine[0, j, k]];
                }
            }
            for (int j = 0; j < gridLines.GetLength(1); j++)
            {
                for (int k = 0; k + 1 < gridLines.GetLength(2); k++)
                {
                    gridLines[1, j, k].color = BRVG[randomBoard.gridLine[1, j, k]];
                }
            }
            plotBoxes();
    }

    IEnumerator Load()
    {
        for (int i = 0; i < 45; i++)
        {
            plotBoardLinesRand();
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
    void plotBoxes()
     
    {
        Vector3 temps = new Vector3(0, thisBoard.gridLine.GetLength(1) - 1, thisBoard.gridLine.GetLength(2) - 1);
        for (int i = 0; i < temps.y; i++)
        {
            for (int j = 0; j < temps.z; j++)
            {
                boxes[i, j].color = BRVG[0];
                if (thisBoard.gridLine[0, i, j] == thisBoard.gridLine[0, i, j + 1])
                {
                    if (thisBoard.gridLine[1, i, j] == thisBoard.gridLine[1, i + 1, j])
                    {
                        if (thisBoard.gridLine[0, i, j] == thisBoard.gridLine[1, i, j])
                        {

                            boxes[i, j].color = BRVG[thisBoard.gridLine[0, i, j]];
                        }
                    }
                }
            }
        }
    }
    void changBox(bool change, int i, int j, int k)
    {

        gridLines[i, j, k].gameObject.GetComponent<BoxCollider2D>().enabled= change;
    }

    // SERVER
    //
    //
    // Functions


          
}
