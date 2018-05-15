using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby {

    public string name;
    public int id;
    public string players;

    public Lobby(string name, string players, int id)
    {
        this.name = name;
        this.players = players;
        this.id = id;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
