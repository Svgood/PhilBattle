using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsControll : MonoBehaviour {

    public GameObject[] dHrts;
    public GameObject[] aHrts;
    Player def, at;
    GameObject bg;
    private void Start()
    {
        bg = transform.GetChild(0).gameObject;
        hrtsOff();
    }

    public void setPlayers(int def, int at)
    {
        this.def = Player.findById(def);
        this.at = Player.findById(at);
        hrtsOn();
    }

    public void hrtsOn()
    {
        bg.SetActive(true);
        foreach (var obj in dHrts)
        {
            obj.SetActive(true);
            obj.transform.GetChild(0).GetComponent<Image>().color = def.color;
        }
        foreach (var obj in aHrts)
        {
            obj.SetActive(true);
            obj.transform.GetChild(0).GetComponent<Image>().color = at.color;
        }
    }

    public void hrtsOff()
    {
        bg.SetActive(false);
        foreach (var obj in dHrts)
            obj.SetActive(false);
        foreach (var obj in aHrts)
            obj.SetActive(false);
    }

    public void removeHeart(int id)
    {

        if (id == def.id)
        {
            foreach (var obj in dHrts)
                if (obj.activeSelf)
                {
                    obj.SetActive(false);
                    break;
                }
        }
        else
        {
            foreach (var obj in aHrts)
                if (obj.activeSelf)
                {
                    obj.SetActive(false);
                    break;
                }
        }


  


    }
}
