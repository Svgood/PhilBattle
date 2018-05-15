using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public GameObject menu;

	public void toggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }
}
