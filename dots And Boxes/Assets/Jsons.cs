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
    public int numberPlayer, move;
    public boardDots(int[,,] gridLines, int numberPlayers, int moves = 1)
    {
        this.gridLine = gridLines;
        this.numberPlayer = numberPlayers;
        this.move = moves;
        
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
}