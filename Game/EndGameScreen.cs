using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{

    public GameObject[] placesObj;
    public Button btn1, btn2;
    public Image[] avas;
    public Text[] txts;
    public Sprite[] sprites;
    int[] places = new int[4];
    int playersCount = 4;
    int curPlaceMove = 0;
    public static bool MoveOut = false;
    // Use this for initialization
    void Start()
    {
        foreach (GameObject obj in placesObj)
        {
            obj.transform.position += Vector3.down * (4 - NetworkControllerScript.controller.players_count) * 0.9f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (MoveOut)
            movePlaces();
    }

    public void showEndGame(Player[] players)
    {
        Debug.Log("Konec");
        bool swaped = true;
        MoveOut = true;
        Player temp;
        playersCount = players.Length;

        while (swaped)
        {
            swaped = false;
            for (int i = 1; i < players.Length; i++)
            {
                if (players[i].ters > players[i - 1].ters)
                {
                    temp = players[i];
                    players[i] = players[i - 1];
                    players[i - 1] = temp;

                    swaped = true;
                }
            }
        }

        for (int i = 0; i < players.Length; i++)
        {
            avas[i].sprite = sprites[players[i].ava_id];
            txts[i].color = players[i].color;
            txts[i].text = "Территорий - " + players[i].ters;
        }






    }

    public void movePlaces()
    {
        if (curPlaceMove > NetworkControllerScript.controller.players_count - 1)
        {
            if (btn1.transform.position.x > 4)
            {
                btn1.transform.position += Vector3.left * 0.7f;
                //btn2.transform.position += Vector3.left * 0.7f;
            }
            else
            {
                MoveOut = false;
            }
        }
        else
        {
            if (placesObj[curPlaceMove].transform.position.x > -18)
                placesObj[curPlaceMove].transform.position += Vector3.left * 0.7f;
            else
                curPlaceMove++;
        }
    }
}
