using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    public static List<Player> players = new List<Player>();
    public string name;
    public int id;
    public int ava_id;
    public int ters;
    public int bonus_questions_answered;
    public Color32 color;

    //for multi answering
    public bool answered_right;
    public bool attacker;
    public bool defender;

    public int hp;

    public Player(int id, string name, int ava_id, Color32 clr)
    {
        this.id = id;
        this.name = name;
        this.ava_id = ava_id;
        this.color = clr;
        ters = 0;
        players.Add(this);
    }

    public void changeTer(int t)
    {
        ters += t;
    }

    public static Player findById(int id)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].id == id)
                return players[i];
        }
        return null;
    }


}
