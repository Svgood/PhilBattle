using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{

    Network net;
    NetworkControllerScript game;
    public IntVec pos;
    private bool claiming;
    bool isStarting = false;
    public SpriteRenderer sprite;
    private int owner = 0;
    Color32 tmp, standart;
    public GameObject dot;


    void Start()
    {
        tmp = new Color32();
        sprite = GetComponent<SpriteRenderer>();
        standart = sprite.color;
    }

    public void setNet(Network n)
    {
        net = n;
    }

    public void setGame(NetworkControllerScript g)
    {
        game = g;
    }
    //Utils

    public int getOwner()
    {
        return owner;
    }

    public void setStarting()
    {
        isStarting = true;
    }

    public void setPos(int xx, int yy)
    {
        pos = new IntVec(xx, yy);
    }

    public bool capturable(int playerdId)
    {
        if (pos.x - 1 >= 0)
            if (NetworkControllerScript.field[pos.x - 1, pos.y].GetComponent<CellScript>().getOwner() == game.cur_player.id && owner != game.cur_player.id && !isStarting)
            {
                //Debug.Log("Da");
                return true;
            }
        if (pos.x + 1 < NetworkControllerScript.controller.field_size)
            if (NetworkControllerScript.field[pos.x + 1, pos.y].GetComponent<CellScript>().getOwner() == game.cur_player.id && owner != game.cur_player.id && !isStarting) 
            {
                //Debug.Log("Da");
                return true;
            }
        if (pos.y - 1 >= 0)
            if (NetworkControllerScript.field[pos.x, pos.y - 1].GetComponent<CellScript>().getOwner() == game.cur_player.id && owner != game.cur_player.id && !isStarting)
            {
                //Debug.Log("Da");
                return true;
            }
        if (pos.y + 1 < NetworkControllerScript.controller.field_size)
            if (NetworkControllerScript.field[pos.x, pos.y + 1].GetComponent<CellScript>().getOwner() == game.cur_player.id && owner != game.cur_player.id && !isStarting)
            {
                //Debug.Log("Da");
                return true;
            }
        return false;
    }

    //Mouse Logic

    private void OnMouseOver()
    {

        //Transparent
        if (!NetworkControllerScript.controller.questions_time)
        {
            ChangeColorTemp(Player.findById(game.playerId).color);
            setAlpha(125);
            if (owner != 0)
            {

            }
        }


        //Claiming  if (Input.GetMouseButtonDown(0) && capturable(NetworkControllerScript.cur_player.id))
        if (Input.GetMouseButtonDown(0) && game.curPlayerId == game.playerId && capturable(game.cur_player.id) && !game.questions_time)
        {
            game.current_cell = this;
            if (owner == 0)
            {
                game.state = "capturing";
                game.showQuestions();
            }
            else
            {
                net.startContest(owner, pos.x, pos.y);
                //game.showQuestions();
            }

        }


    }

    private void OnMouseExit()
    {
        claiming = false;
        ChangeColor(standart);
    }

    //Upgrade
    public void changeOwner(int playerId)
    {
        if (playerId == 0)
        {
            Player.findById(owner).changeTer(-1);
            ChangeColor(new Color(0, 0, 0, 0));
            owner = 0;
            return;
        }
        if (Player.findById(playerId).ters == 0)
        {
            isStarting = true;
            GetComponent<avaScript>().Invoke();
        }
        if (owner != 0)
        {
            Player.findById(owner).changeTer(-1);
        }
        owner = playerId;
        Player.findById(playerId).changeTer(1);
        ChangeColor(Player.findById(playerId).color);
   
            
    }


    //Visuals
    public void ChangeColor(Color32 color)
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = color;
        standart = color;
    }

    public void ChangeColorTemp(Color32 color)
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = color;
    }

    public void setAlpha(byte alpha)
    {
        tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = alpha;
        GetComponent<SpriteRenderer>().color = tmp;
    }

    public void setAlphaSave(byte alpha)
    {
        tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = alpha;
        GetComponent<SpriteRenderer>().color = tmp;
        standart = GetComponent<SpriteRenderer>().color;
    }

}
