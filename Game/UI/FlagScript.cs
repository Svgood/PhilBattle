using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagScript : MonoBehaviour
{

    public Image img1;
    Image img;
    Color32 tmp, white;
    NetworkControllerScript game;

    bool movingOut = false;
    float move_out_pos, start_pos;
    float speed = 0.15f;
    // Use this for initialization
    void Start()
    {
        game = GameObject.Find("Controller").GetComponent<NetworkControllerScript>();
        white = new Color32(255, 255, 255, 255);
        img = GetComponent<Image>();
        move_out_pos = 10f;
        start_pos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkControllerScript.controller.transition_screen)
        {
            movingOut = false;
        }


        if (NetworkControllerScript.controller.endturnQuestion || NetworkControllerScript.controller.transition_screen)
        {
            if (NetworkControllerScript.controller.transition_screen)
                tmp = white;
            else
                tmp = game.cur_player.color;
            tmp.a = 255;
            img.color = tmp;

            tmp = img1.color;
            tmp.a = 255;
            img1.color = tmp;
        }
        else
        {
            if (!NetworkControllerScript.controller.transition_screen)
            {
                movingOut = true;
                //tmp.a = 0;
                img.color = tmp;
                tmp = img1.color;
                //tmp.a = 0;
                img1.color = tmp;
            }
        }
        moveOut();
        moveIn();
    }

    void moveOut()
    {
        if (movingOut)
        {
            if (transform.position.y >= move_out_pos)
            {
                //speed = speed_starting;
                return;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);

        }
    }

    void moveIn()
    {
        if (!movingOut)
        {
            if (transform.position.y - speed <= start_pos)
            {
                transform.position = new Vector3(transform.position.x, start_pos, transform.position.z);
                return;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
        }
    }


}
