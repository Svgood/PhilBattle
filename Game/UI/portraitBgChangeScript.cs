using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class portraitBgChangeScript : MonoBehaviour
{


    public Sprite inactive, active;
    public int player;
    private Image spr;
    NetworkControllerScript game;
    // Use this for initialization
    void Start()
    {
        game = GameObject.Find("Controller").GetComponent<NetworkControllerScript>();
        spr = GetComponent<Image>();
        if (player > NetworkControllerScript.controller.players_count)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (game.cur_player.id == player)
        {
            spr.sprite = active;
        }
        else
        {
            spr.sprite = inactive;
        }
    }
}
