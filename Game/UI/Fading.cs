using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour {

    float delay = 2f;
    float timer = 0f;
    Image img;
    Color tmp;
	// Use this for initialization
	void Start () {
        img = GetComponent<Image>();		
	}
	
	// Update is called once per frame
	void Update () {
        if (timer < delay)
        {
            timer += Time.deltaTime;
        }

		if (img.color.a > 0 && timer > delay)
        {
            tmp = img.color;
            tmp.a -= 0.01f;
            img.color = tmp;
            

        }
        if (img.color.a <= 0)
        {
            Destroy(this.gameObject);
        }
	}
}
