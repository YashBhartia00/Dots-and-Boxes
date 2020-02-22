using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Matches
{
    public string gameType, userName;
    public bool found;
    public board gameBoard;
    public int timeCreated;

    public Matches(string gameTypes, string userNames, board gameBoards, int timeCreateds, bool founds = false)
    {
        this.gameType = gameTypes;
        this.userName = userNames;
        this.found = founds;
        this.timeCreated = timeCreateds;

    }

}

public class board
{
   public int[] positions = new int[10]; //10th for which player played last;
   
    public board(int[] argpos)
    {
        this.positions = argpos;
    }
}

public class boardDots
{
    public int[,,] gridLine; // H/V , X, Y: value = Player
    public int[] compactGridLines;
    public int numberPlayer, moves;
    public boardDots(int[,,] gridLines, int numberPlayers, int movess)
    {
        this.compactGridLines = new int[gridLines.GetLength(0) * gridLines.GetLength(1) * gridLines.GetLength(2)];
        this.gridLine = gridLines;
        this.numberPlayer = numberPlayers;
        this.moves = movess;
        
    }
    public void randomize()
    {
        for(int i = 0; i < gridLine.GetLength(0); i++)
        {
            for (int j = 0; j < gridLine.GetLength(1); j++)
            {
                for (int k = 0; k < gridLine.GetLength(2); k++)
                {
                    gridLine[i, j, k] = UnityEngine.Random.Range(1, numberPlayer +1);
                }
            }
        }
    }

    public void updateCompact()
    {
        for (int i = 0; i < gridLine.GetLength(0); i++)
        {
            for (int j = 0; j < gridLine.GetLength(1); j++)
            {
                for (int k = 0; k < gridLine.GetLength(2); k++)
                {
                    compactGridLines[i * (gridLine.GetLength(1)) * gridLine.GetLength(2) + j * gridLine.GetLength(2) + k] = gridLine[i, j, k];
                }
            }
        }
    }

    public int[,,] updategrids()
    {
        int x,y,z;
        
       for(int i = 0; i < compactGridLines.Length; i++)
        {
            x = i / (gridLine.GetLength(1) * gridLine.GetLength(2));
            y = (i % (gridLine.GetLength(1) * gridLine.GetLength(2)))/gridLine.GetLength(2);
            z = (i % (gridLine.GetLength(1) * gridLine.GetLength(2))) % gridLine.GetLength(2);
            gridLine[x,y,z] = compactGridLines[i];
        }
        
        return gridLine;
    }
}