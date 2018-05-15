using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {


    private int gameVer = 1;

    public GameObject lobbyBtn;
    public GameObject menu, login, lobbyScreen, lobby, lobbyOptions, errorScreen;
    public Dropdown lobbyType;

    GameObject[] lobbyPlayers;
    List<Lobby> lobbies = new List<Lobby>();
    List<GameObject> lobbiesBtns = new List<GameObject>();
    List<string> playerNames = new List<string>();
    Network net;

    Color32 red, green, blue, orange;
    Color32[] colors;

    GameObject[] screens;
	// Use this for initialization
	void Start () {
        net = GetComponent<Network>();
		screens = new GameObject[]{ menu, login, lobbyScreen, lobby, lobbyOptions};
        lobbyPlayers = new GameObject[] {
                                        lobby.transform.GetChild(3).gameObject,
                                        lobby.transform.GetChild(4).gameObject,
                                        lobby.transform.GetChild(5).gameObject,
                                        lobby.transform.GetChild(6).gameObject};


        red = new Color32(219, 99, 79, 125);
        green = new Color32(71, 255, 71, 125);
        blue = new Color32(105, 105, 216, 125);
        orange = new Color32(255, 174, 0, 125);
        colors = new Color32[] { red, green, blue, orange };
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
            errorScreen.SetActive(false);
	}

    public void showErrorScreen(string msg)
    {
        errorScreen.SetActive(true);
        errorScreen.transform.GetChild(1).GetComponent<Text>().text = msg;
    }

    public void showLoginScreen()
    {
        hideAll();
        login.SetActive(true);
        InputField name = GameObject.Find("Login").GetComponent<InputField>();
        name.text = PlayerPrefs.GetString("loginName", "");
    }

    public void showLobbyScreen()
    {
        hideAll();
        lobbyScreen.SetActive(true);
    }

    public void showJoinedLobbyScreen()
    {
        hideAll();
        lobby.SetActive(true);
    }

    public void showLobbyOptionsScreen()
    {
        hideAll();
        lobbyOptions.SetActive(true);
    }

    void hideAll()
    {
        foreach (GameObject s in screens)
        {
            s.SetActive(false);
        }
    }

    public void addLobby(Lobby l)
    {
        lobbies.Add(l);
    }

    public void showLobbies()
    {
        clearLobbiesBtns();
        int i = 0;
        foreach (Lobby l in lobbies)
        {
            var tmp = Instantiate(lobbyBtn, lobbyScreen.transform.GetChild(0).transform);
            tmp.transform.position += Vector3.down * transform.localScale.y * 1.2f * i;
            tmp.transform.GetChild(0).GetComponent<Text>().text = l.name;
            tmp.transform.GetChild(1).GetComponent<Text>().text = l.players;
            tmp.transform.GetChild(2).GetComponent<Text>().text = "Id - " + l.id;
            tmp.GetComponent<Button>().onClick.AddListener(delegate() { net.joinLobby(l.id); });
            i++;
            lobbiesBtns.Add(tmp);
        }
    }

    public void clearLobbiesBtns()
    {
        foreach (GameObject obj in lobbiesBtns)
        {
            Destroy(obj);
        }
        lobbiesBtns = new List<GameObject>();
    }

    public void clearLobbies()
    {
        lobbies = new List<Lobby>();
    }

    public void loginPlayer()
    {
        string name = GameObject.FindGameObjectWithTag("login").GetComponent<Text>().text;
        if (name.Length > 0)
        {
            string pas = "zatichka";
            PlayerPrefs.SetString("loginName", name);
            net.login(name, pas);
        }
        else
            showErrorScreen("Введите имя");
        //string pas = GameObject.FindGameObjectWithTag("pas").GetComponent<Text>().text;
        
    }

    public void fillLobby()
    {
        for (int i = 0; i < playerNames.Count; i++)
        {
            lobbyPlayers[i].transform.GetChild(2).GetComponent<Text>().text = playerNames[i];
            lobbyPlayers[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            lobbyPlayers[i].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,1);
        }
    }
    
    void resetLobby()
    {
        for (int i = 0; i < lobbyPlayers.Length; i++)
        {
            lobbyPlayers[i].transform.GetChild(2).GetComponent<Text>().text = "Empty";
            lobbyPlayers[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
            lobbyPlayers[i].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,0.2f);
        }
    }

    public void updateLobby()
    {
        resetLobby();
        fillLobby();
    }

    public void createLobby()
    {
        net.createLobby(lobbyType.value);
    }

    public void addPlayer(string name)
    {
        playerNames.Add(name);
        updateLobby();
    }

    public void clearPlayers()
    {
        playerNames = new List<string>();
    }

    public void savePlayersNames()
    {
        for (int i = 0; i < playerNames.Count; i++)
        {
            PlayerPrefs.SetString("PlayerName" + (i + 1), playerNames[i]);
        }
    }

    public void closeGame()
    {
        Application.Quit();
    }
}
