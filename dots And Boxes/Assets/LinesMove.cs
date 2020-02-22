using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;


public class LinesMove : MonoBehaviour
{
    public int changeToValue;
    SpriteRenderer sr;
    public GameObject main;
    public MainDots mainScript;
    private void Start()
    {
        main =  GameObject.FindGameObjectWithTag("Main");
        mainScript = main.GetComponent<MainDots>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
    private void OnMouseDown()
    {
        changeToValue = MainDots.playerNumber;
        MainDots.moveNumberFromCollider = (MainDots.playerNumber + 1) > mainScript.thisBoard.numberPlayer ? 1 : MainDots.playerNumber+1 ;
        mainScript.plotBoardLines();

    }

    private void Update()
    {
        if(sr.color != MainDots.BRVG[0])
        {
            changeToValue = sr.color == MainDots.BRVG[1] ? 1 : sr.color == MainDots.BRVG[2] ? 2 : sr.color == MainDots.BRVG[3] ? 3 : sr.color == MainDots.BRVG[4] ? 4 : 0;
        }
    }
}
