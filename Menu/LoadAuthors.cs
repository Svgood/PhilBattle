using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAuthors : MonoBehaviour
{

    public GameObject nik;
    public GameObject max;
    public GameObject rus;
    public GameObject but;

    bool back_time = false;
    
    float start_pos_nik;
    float start_pos_max;
    float start_pos_rus;
    float start_pos_but;
    float move_out_pos;

    float acc = 0.0002f;
    float speed_starting = 0.2f;
    float speed = 0.2f;
    bool moving_out = false;
    bool moving_in = false;


    void Start()
    {
        start_pos_nik = nik.transform.position.y;
        start_pos_max = max.transform.position.y;
        start_pos_rus = rus.transform.position.y;
        start_pos_but = rus.transform.position.y;
        move_out_pos = -6f;

        nik.transform.position = new Vector3(nik.transform.position.x, start_pos_nik - 8, nik.transform.position.z);
        max.transform.position = new Vector3(max.transform.position.x, start_pos_max - 8, max.transform.position.z);
        rus.transform.position = new Vector3(rus.transform.position.x, start_pos_rus - 8, rus.transform.position.z);
        but.transform.position = new Vector3(but.transform.position.x, start_pos_rus - 8, but.transform.position.z);
    }

    void Update()
    {
        if (back_time)
        {
            moving_out = true;
        }
        if (LoadMenu.start_authors)
            moving_in = true;
        moveOut();
        moveIn();
    }

    void moveOut()
    {
        if (moving_out)
        {
            if (nik.transform.position.y <= move_out_pos)
            {
                moving_out = false;
                speed = speed_starting;
                back_time = false;
                LoadMenu.back_time = true;
                return;
            }
            nik.transform.position = new Vector3(nik.transform.position.x, nik.transform.position.y - speed, nik.transform.position.z);
            max.transform.position = new Vector3(max.transform.position.x, max.transform.position.y - speed, max.transform.position.z);
            rus.transform.position = new Vector3(rus.transform.position.x, rus.transform.position.y - speed, rus.transform.position.z);
            but.transform.position = new Vector3(but.transform.position.x, but.transform.position.y - speed, but.transform.position.z);
            speed += acc;
        }
    }

    void moveIn()
    {
        if (moving_in)
        {
            if (nik.transform.position.y >= start_pos_nik)
            {
                moving_in = false;
                speed = speed_starting;
                LoadMenu.start_authors = false;
                return;
            }
            nik.transform.position = new Vector3(nik.transform.position.x, nik.transform.position.y + speed_starting, nik.transform.position.z);
            max.transform.position = new Vector3(max.transform.position.x, max.transform.position.y + speed_starting, max.transform.position.z);
            rus.transform.position = new Vector3(rus.transform.position.x, rus.transform.position.y + speed_starting, rus.transform.position.z);
            but.transform.position = new Vector3(but.transform.position.x, but.transform.position.y + speed_starting, but.transform.position.z);
        }
    }

    public void loadMenu()
    {
        back_time = true;
    }
}
