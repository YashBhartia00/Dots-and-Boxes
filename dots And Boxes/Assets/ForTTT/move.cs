using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;

public class move : MonoBehaviour
{
    public GameObject x, o, tests;
    private void OnMouseDown()
    {
        if (test.positions[int.Parse(gameObject.name)] ==0)
        {
            test.positions[int.Parse(gameObject.name)] = test.playerNumber;
            test.positions[9] = test.playerNumber;
            board blah = new board(test.positions);
            // RestClient.Put("https://dots-68b2c.firebaseio.com/boards/" + 1 + ".json", blah);
            RestClient.Put("https://dots-68b2c.firebaseio.com/Playings/" + tests.GetComponent<test>().gameType.text.ToString() + "/" + tests.GetComponent<test>().userNumber + ".json", blah);
        }
    }
}
