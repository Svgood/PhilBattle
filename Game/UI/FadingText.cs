using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingText : MonoBehaviour {

    float delay = 2f;
    float timer = 0f;
    Text txt;
    Color tmp;
    // Use this for initialization
    void Start()
    {
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < delay)
        {
            timer += Time.deltaTime;
        }

        if (txt.color.a > 0 && timer > delay)
        {
            tmp = txt.color;
            tmp.a -= 0.01f;
            txt.color = tmp;


        }
        if (txt.color.a <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
