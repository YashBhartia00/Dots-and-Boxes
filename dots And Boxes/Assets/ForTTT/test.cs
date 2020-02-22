using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Proyecto26;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    public int userNumber, drawNumber;
    public Text gameType, userName, youName, enemyName;
    public static int playerNumber = 1;
    public static int[] positions = new int[10];
    public GameObject[] pcsX;
    public GameObject[] pcsO;
    public board thisBoard = new board(positions);
    public bool lookingForMatch, draw;
    int[,] winCombos = new int[8, 3] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };
    public int[] check = new int[10];

    public GameObject[] disableOnPlay, enableOnPlay, winLose, postitionsObjs; //enableOnPlay 0 is positions

    void Start()
    {
        int[] positions = new int[10];
        for (int i = 0; i < winLose.Length; i++) { winLose[i].SetActive(false); print("bl"); } 
        print("Player number = 1 or 2, 1 by default");
        InvokeRepeating("live", 1f, 0.5f);
    }

    public void lookForBoard()
    { userNumber = getTime();

        for (int i = 0; i < disableOnPlay.Length; i++)
        {
            disableOnPlay[i].SetActive(false);
        }
        Matches newMatch = new Matches(gameType.text.ToString(), userName.text.ToString(), thisBoard, userNumber);

        RestClient.Get<Matches>("https://dots-68b2c.firebaseio.com/Pendings/" + newMatch.gameType + "/1/" + ".json").Then(res =>
              {
                  youName.text = "O: " + userName.text;
                  RestClient.Put("https://dots-68b2c.firebaseio.com/Pendings/" + newMatch.gameType + "/1/" + ".json", newMatch);
                  //print(res.userName);
                  enemyName.text = "X: " + res.userName;
                  newMatch.found = true;
                  userNumber = res.timeCreated;
                  RestClient.Put("https://dots-68b2c.firebaseio.com/Playings/" + newMatch.gameType + "/" + res.timeCreated + ".json", thisBoard);
                  OnPlay();
                  playerNumber = 2;
              }).Finally(() => createMatch(newMatch.found, newMatch));

    }

    public void createMatch(bool execute, Matches thisMatch)
    {
        if (!execute)
        {
            lookingForMatch = true;
            RestClient.Put("https://dots-68b2c.firebaseio.com/Pendings/" + thisMatch.gameType + "/" + 1 + ".json", thisMatch);
            youName.text = "X: " + userName.text;
        }
    }

    private void Update()
    {
        plotBoard();
        gameRefree();
        if (lookingForMatch)
        {
            RestClient.Get<board>("https://dots-68b2c.firebaseio.com/Playings/" + gameType.text.ToString() + "/" + userNumber + ".json").Then(res =>
            { lookingForMatch = false;
                OnPlay();
                playerNumber = 1;
                RestClient.Get<Matches>("https://dots-68b2c.firebaseio.com/Pendings/" + gameType.text.ToString() + "/1/" + ".json").Then(resu =>
                {
                    //print(rest.userName)
                    enemyName.text = "O: " + resu.userName;
                    RestClient.Delete("https://dots-68b2c.firebaseio.com/Pendings/" + gameType.text.ToString() + "/1/" + ".json");
                });

            });
        }
        check = positions;

    }

    public board live()
    {
        if (!lookingForMatch)
        {
            RestClient.Get<board>("https://dots-68b2c.firebaseio.com/Playings/" + gameType.text.ToString() + "/" + userNumber + ".json").Then(res =>
            { positions = res.positions; });
        }
        return (new board(positions));
    }

    public void OnPlay()
    {

        for (int i = 0; i < enableOnPlay.Length; i++)
        {
            enableOnPlay[i].SetActive(true);
        }

    }

    public void plotBoard()
    {
        drawNumber = 0;
        for (int i = 0; i < 9; i++)
        {
            pcsX[i].SetActive(false);
            pcsO[i].SetActive(false);

            if (positions[i] == 1)
            {
                pcsX[i].SetActive(true);
                drawNumber++;
            }
            else if (positions[i] == 2)
            {
                pcsO[i].SetActive(true);
                drawNumber++;
            }
        }
        if (drawNumber == 9)
        {
            draw = true;
        }
    }

    public void gameRefree()
    {
        if (playerNumber == positions[9]) { changeBox(false); } else { changeBox(true); }
        if (checkWin() != 0)
        {
            //enableOnPlay[0].SetActive(false);
            changeBox(false);
            winLose[4].SetActive(true);
            if (checkWin() == 3) { winLose[2].SetActive(true); } else if (playerNumber == checkWin()) { winLose[0].SetActive(true); } else { winLose[1].SetActive(true); }
            winLose[3].SetActive(true);
            

        }

    }
    public void changeBox(bool active)
    {
        for (int i = 0; i < 9; i++)
        {
            postitionsObjs[i].GetComponent<BoxCollider2D>().enabled = active;
        }
    }
    public int checkWin()
    {
        for (int i = 0; i < 8; i++)
        {
            if (positions[winCombos[i, 0]] == positions[winCombos[i, 1]] && positions[winCombos[i, 1]] == positions[winCombos[i, 2]]) {
                return positions[winCombos[i, 0]];
            }
        }
        if (draw) { return 3; }
        return 0;
    }
    public int getTime()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        return cur_time;

    }

    public void restart()
    {
        //for (int i = 0; i < 4; i++) { if (winLose[i].activeSelf) { winLose[i].SetActive(false); print("bl"); } }
        SceneManager.LoadScene("StartScreen");
        
    }
}
