using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class avaScript : MonoBehaviour {


    public int id = 1;
    public Sprite[] sprites;
    Image spr;
    Color32 tmp;
    
	// Use this for initialization
	void Start () {
        spr = GetComponent<Image>();
        switch (id)
        {
            case 1:
                spr.sprite = sprites[PlayerPrefs.GetInt("Player1")];
                break;
            case 2:
                spr.sprite = sprites[PlayerPrefs.GetInt("Player2")];
                break;
            case 3:
                spr.sprite = sprites[PlayerPrefs.GetInt("Player3")];
                break;
            case 4:
                spr.sprite = sprites[PlayerPrefs.GetInt("Player4")];
                break;
            case 0:
                break;
            default:
                spr.sprite = sprites[PlayerPrefs.GetInt("Player1")];
                break;
        }
        if (id == 0)
        {
            //Debug.Log(GetComponent<CellScript>().getOwner());
           
        }
	}

    public void Invoke()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        switch (GetComponent<CellScript>().getOwner())
        {
            case 1:
                sprite.sprite = sprites[PlayerPrefs.GetInt("Player1", 1)];
                GetComponent<CellScript>().setAlphaSave(200);
                break;
            case 2:
                sprite.sprite = sprites[PlayerPrefs.GetInt("Player2", 2)];
                GetComponent<CellScript>().setAlphaSave(200);
                break;
            case 3:
                sprite.sprite = sprites[PlayerPrefs.GetInt("Player3", 3)];
                GetComponent<CellScript>().setAlphaSave(200);
                break;
            case 4:
                sprite.sprite = sprites[PlayerPrefs.GetInt("Player4", 4)];
                GetComponent<CellScript>().setAlphaSave(200);
                break;
        }
    }

    
}
