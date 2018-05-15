using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenu : MonoBehaviour {

    public GameObject backview;
    public GameObject logo;
    public GameObject buttons;
    public GameObject squares_left, squares_right;
    public GameObject instr;
    public GameObject authors;

    bool load_time = false;

    static public bool mode_time = false;
    static public bool back_time = true;
    static public bool authors_time = false;
    static public bool start_authors = false;

    float start_pos_logo;
    float start_pos_backview;
    float start_pos_buttons;
    float start_pos_sq_left;
    float start_pos_sq_right;
    float move_out_pos;

    float acc = 0.0002f;
    float speed_starting = 0.2f;
    float speed = 0.2f;
    bool moving_out = false;
    bool moving_in = false;

   
    void Start()
    {
        start_pos_logo = logo.transform.position.y;
        start_pos_backview = backview.transform.position.y;
        start_pos_buttons = buttons.transform.position.y;
        start_pos_sq_left = squares_left.transform.position.x;
        start_pos_sq_right = squares_right.transform.position.x;
        move_out_pos = 9.5f;

        logo.transform.position = new Vector3(logo.transform.position.x, start_pos_logo + 8, logo.transform.position.z);
        backview.transform.position = new Vector3(backview.transform.position.x, start_pos_backview + 8, backview.transform.position.z);
        buttons.transform.position = new Vector3(buttons.transform.position.x, start_pos_buttons - 8, buttons.transform.position.z);
        squares_left.transform.position = new Vector3(start_pos_sq_left - 8, squares_left.transform.position.y, squares_left.transform.position.z);
        squares_right.transform.position = new Vector3(start_pos_sq_right +8 , squares_right.transform.position.y, squares_right.transform.position.z);

    }

    void Update()
    {
        if (load_time)
        {
            moving_out = true;
        }
        if (back_time)
            moving_in = true;
        if (authors_time)
            moving_out = true;
        moveOut();
        moveIn();
    }

    void moveOut()
    {
        if (moving_out)
        {
            if (logo.transform.position.y >= move_out_pos)
            {
                moving_out = false;
                speed = speed_starting;
                if (authors_time)
                {
                    authors_time = false;
                    start_authors = true;
                }
                else mode_time = true;
                load_time = false;
                return;
            }
            logo.transform.position = new Vector3(logo.transform.position.x, logo.transform.position.y + speed, logo.transform.position.z);
            backview.transform.position = new Vector3(backview.transform.position.x, backview.transform.position.y + speed, backview.transform.position.z);
            buttons.transform.position = new Vector3(buttons.transform.position.x, buttons.transform.position.y - speed, buttons.transform.position.z);
            squares_left.transform.position = new Vector3(squares_left.transform.position.x - speed, squares_left.transform.position.y, squares_left.transform.position.z);
            squares_right.transform.position = new Vector3(squares_right.transform.position.x + speed, squares_right.transform.position.y, squares_right.transform.position.z);
            speed += acc;
        }
    }

    void moveIn()
    {
        if (moving_in)
        {
            if (logo.transform.position.y <= start_pos_logo)
            {
                moving_in = false;
                speed = speed_starting;
                back_time = false;
                return;
            }
            logo.transform.position = new Vector3(logo.transform.position.x, logo.transform.position.y - speed_starting, logo.transform.position.z);
            backview.transform.position = new Vector3(backview.transform.position.x, backview.transform.position.y - speed_starting, backview.transform.position.z);
            buttons.transform.position = new Vector3(buttons.transform.position.x, buttons.transform.position.y + speed_starting, buttons.transform.position.z);
            squares_left.transform.position = new Vector3(squares_left.transform.position.x + speed_starting, squares_left.transform.position.y, squares_left.transform.position.z);
            squares_right.transform.position = new Vector3(squares_right.transform.position.x - speed_starting, squares_right.transform.position.y, squares_right.transform.position.z);
            
        }
    }

    public void loadMode()
    {
        load_time = true;
    }

    public void loadAuthors()
    {
        authors_time = true;
    }

    public void loadInsturct()
    {
        instr.SetActive(true);
    }
    
    public void closeInstruct()
    {
        instr.SetActive(false);
    }

    public void openAuthors()
    {
        authors.SetActive(true);
    }

    public void closeAuthors()
    {
        authors.SetActive(false);
    }
}
