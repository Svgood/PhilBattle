using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Network : MonoBehaviour
{

    public static TcpClient client;
    public static int state = 0;

    int userId;
    string host = "46.101.164.179";
    string localHost = "192.168.1.7";
    int port = 8085;
    int localPort = 9090;
   
    NetworkStream stream;
    StreamWriter writer;
    StreamReader reader;
    NetworkControllerScript game;
    MenuController menu;
    bool rdy = false;

    private void Start()
    {
        startNetwork();
        if (state == 0)
            menu = GetComponent<MenuController>();
        if (state == 1)
        {
            game = GetComponent<NetworkControllerScript>();
            ready();
        }
        
    }

    void Update()
    {

    }
    public void connectToLocal()
    {
        if (client != null)
            client.Close();
        client = new TcpClient(localHost, localPort);
        Debug.Log("Connected to Local");
    }

    public void connectToInet()
    {
        if (client != null)
            client.Close();
        client = new TcpClient(host, port);
        Debug.Log("Connected to Inet");
    }

    public void startNetwork()
    {
        setup();
        StartCoroutine(CheckForMessages());
    }

    void setup()
    {
        if (client == null)
        {
            try
            {
                //connectToLocal();
                connectToInet();
            }
            catch (SocketException e)
            {
                print(e.Message);
                GameObject.Find("serverImg").GetComponent<Image>().color = new Color(1, 0, 0, 1);
                return;
            }
        }
        stream = client.GetStream();
        writer = new StreamWriter(stream);
        reader = new StreamReader(stream);
        rdy = true;
    }

    public void writeSocket(string msg)
    {
        if (!rdy)
            return;
        writer.Write(msg);
        writer.Flush();
    }
    public string readSocket()
    {
        if (!rdy)
            return "";
        if (stream.DataAvailable)
        {
            byte[] data = new byte[1024];
            int bytes = stream.Read(data, 0, data.Length);
            string msg = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
            Debug.Log(msg);
            return msg;
        }
        return "";
    }
    public void closeSocket()
    {
        if (!rdy)
            return;
        writer.Close();
        reader.Close();
        client.Close();
        rdy = false;
    }


    public IEnumerator CheckForMessages()
    {
        while (true)
        {
            string response = readSocket();
            string[] commandsStack = response.Split(';');
            foreach (string cmd in commandsStack)
            {
                if (cmd.Length < 1)
                    continue;
                string[] data = cmd.Split(':');

                //InGame check
                if (data[0] == "sq")
                {
                    string[] tmp = new string[]{ data[1], data[2], data[3], data[4], data[5]};
                    game.questionUI.hideMark();
                    game.networkSetQuestions(tmp);
                }

                if (data[0] == "sp") // set playa id
                {
                    game.playerId = int.Parse(data[1]);
                }
                if (data[0] == "c") // capture cell
                {
                    game.changeCellOwner(int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]));
                }
                if (data[0] == "scp") // set current player
                {
                    game.state = "none";
                    game.contest = false;
                    game.questionUI.hideWaitForOther();
                    game.hideQuestions();
                    game.removeDots();
                    game.curPlayerId = int.Parse(data[1]);
                    game.cur_player = Player.findById(game.curPlayerId);
                    game.createDots();
                }
                if (data[0] == "scq") // common question
                {
                    game.state = "cq";
                    game.contest = true;
                    game.startTransitionScreen();
                }
                if (data[0] == "cq") // contest question
                {
                    game.contest = true;
                    game.hrts.setPlayers(int.Parse(data[1]), int.Parse(data[2]));
                    game.changeResponsePosition();
                    game.showQuestions();
                }
                if (data[0] == "cnq") // contest new question
                {
                    game.changeResponsePosition();
                    game.questionUI.hideWaitForOther();
                }
                if (data[0] == "cw") // contest winner
                {
                    game.questionUI.hideWaitForOther();
                    game.hideQuestions();
                    game.contest = false;
                }

                if (data[0] == "nt")
                {
                    game.updateTurns();
                }

                if (data[0] == "winner")
                {
                    game.endGame();
                }
                if (data[0] == "kick")
                {
                    game.removePlayer(int.Parse(data[1]));
                }
                //Pre game
                if (data[0] == "error")
                {
                    menu.showErrorScreen(data[1]);
                }

                if (data[0] == "jl") // Join lobby
                {
                    menu.showJoinedLobbyScreen();
                    menu.updateLobby();
                }
                if (data[0] == "log") // Login
                {
                    if (data[1] == "1")
                    {
                        menu.showLobbyScreen();
                        writeSocket("gl:");
                    }
                    else
                    {

                    }
                }
                if (data[0] == "lobbie") // Add lobbie
                {
                    menu.addLobby(new Lobby(data[1], data[2] + "/" + data[3], int.Parse(data[4])));
                    menu.showLobbies();
                }
                if (data[0] == "sl") // Show lobbies
                {
                    menu.showLobbies();
                }


                if (data[0] == "cp")
                {
                    menu.clearPlayers();
                }
                if (data[0] == "player") //Add player
                {
                    menu.addPlayer(data[1]);
                }
                if (data[0] == "ul")
                {
                    menu.updateLobby();
                }
                if (data[0] == "close")
                {
                    menu.showLobbyScreen();
                    updateLobbies();
                }

                if (data[0] == "start")
                {
                    state = 1;
                    menu.savePlayersNames();
                    PlayerPrefs.SetInt("PlayersCount", int.Parse(data[1]));
                    Application.LoadLevel(1);
                }

                if (data[0] == "minusHealth")
                {
                    game.hrts.removeHeart(int.Parse(data[1]));
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
        //yield return new WaitForSeconds(0.3f);

    }

    //Pregame

    public void updateLobbies()
    {
        menu.clearLobbies();
        menu.clearLobbiesBtns();
        writeSocket("gl:");
    }

    public void login(string name, string password)
    {
        writeSocket("login:" + name + ":" + password);
    }

    public void createLobby(int type)
    {
        writeSocket("cl:" + type + ";");
    }

    public void closeLobby()
    {
        writeSocket("closel:");
    }

    public void joinLobby(int lobbyId)
    {
        writeSocket("jl:" + lobbyId);
        menu.clearPlayers();
    }

    public void startLobby()
    {
        writeSocket("sl:");
    }
    //Game

    public void ready()
    {
        writeSocket("rdy:;");
    }

    public void captureCell(int playedId, int x, int y)
    {
        writeSocket("c:" + playedId + ":" + x + ":" + y + ";");
    }

    public void contestAnwer(bool yes)
    {
        if (yes)
            writeSocket("ca:1");
        else
            writeSocket("ca:0");
    }

    public void startContest(int enemyId, int x, int y)
    {
        writeSocket("sc:" + enemyId + ":" + x + ":" + y);
    }

    public void nextTurn()
    {
        writeSocket("nt:");
    }

    public void exitGame()
    {
        writeSocket("quitLobby:");
        game.returnToLobby();
    }

    public void OnApplicationQuit()
    {
        writeSocket("closeConnection:;");
    }



    /*
    public string [] requestQuestion()
    {
        return sendMsg("gq:1").Split(':');
    }

    public string [] getGameInfo()
    {
        return sendMsg("info:1").Split(':');
    }

    public void setCell(int playerdId, int lobbyId, int x, int y)
    {

    }
    */


}
