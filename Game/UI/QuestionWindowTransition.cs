using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionWindowTransition : MonoBehaviour
{

    NetworkControllerScript game;
    public GameObject question, btn, mark, falseMark, waitForOther;
    public Image img;
    Color tmp, tmpFlag;


    float start_pos;
    float start_pos_btn;
    float move_out_pos;


    float alpha;
    float alphaDecaying = 0.007f;
    float acc = 0.01f;
    float speed_starting = 0.05f;
    float speed = 2f;


    //For winner mark
    float delay = 1f;
    float delayCount = 2f;
    //

    bool moving_out = true;
    bool moving_in = false;

    // Use this for initialization
    void Start()
    {
        game = GameObject.Find("Controller").GetComponent<NetworkControllerScript>();
        alpha = 0.25f;
        tmp = img.color;
        tmp.a = 0;
        img.color = tmp;
        start_pos = question.transform.position.x;
        start_pos_btn = btn.transform.position.x;
        move_out_pos = -15;
        hideMark();
        game.hideQuestions();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveOut();
        moveIn();
    }

    void moveOut()
    {
        if (moving_out && delayCount > delay)
        {
            if (question.transform.position.x <= move_out_pos)
            {
                hideMark();
                game.hrts.hrtsOff();
                moving_out = false;
                speed = speed_starting;
                return;
            }
            question.transform.position = new Vector3(question.transform.position.x - speed, question.transform.position.y, question.transform.position.z);
            btn.transform.position = new Vector3(btn.transform.position.x + speed, btn.transform.position.y, btn.transform.position.z);
            speed += acc;

            //Color
            tmp = img.color;
            tmp.a -= alphaDecaying / 1.3f;
            img.color = tmp;
        }
    }

    void moveIn()
    {
        if (moving_in)
        {
            if (question.transform.position.x + speed >= start_pos)
            {
                moving_in = false;
                speed = speed_starting;
                question.transform.position = new Vector3(start_pos, question.transform.position.y, question.transform.position.z);
                btn.transform.position = new Vector3(start_pos_btn, btn.transform.position.y, btn.transform.position.z);
                return;
            }
            question.transform.position = new Vector3(question.transform.position.x + speed, question.transform.position.y, question.transform.position.z);
            btn.transform.position = new Vector3(btn.transform.position.x - speed, btn.transform.position.y, btn.transform.position.z);
            speed += acc;

            //Color
            tmp = img.color;

            if (tmp.a <= alpha)
            {
                tmp.a += alphaDecaying;
                img.color = tmp;
            }
        }
    }

    public void showQuestions()
    {
        moving_in = true;
    }

    public void hideQuestions()
    {
        moving_out = true;
    }

    public void showWaitForOther()
    {
        waitForOther.SetActive(true);
    }

    public void hideWaitForOther()
    {
        waitForOther.SetActive(false);
    }

    public void showMark(bool answer)
    {
        if (answer)
        {
           mark.SetActive(true);
           tmp = Player.findById(game.playerId).color;
           tmp.a = 1;
           mark.GetComponent<Image>().color = tmp;

         }
         else
         {
           falseMark.SetActive(true);
         }
        
    }

    public void hideMark()
    {
        mark.SetActive(false);
        falseMark.SetActive(false);
    }
}
