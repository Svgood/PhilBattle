using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NetworkControllerScript : MonoBehaviour
{

    //NetworkShit
    public int playerId = 1;
    public int curPlayerId = 0;
    public int lobbyId;
    public string state = "capturing";
    public Player cur_player;
    public bool contest = false;
    Network net;
    public QuestionWindowTransition questionUI;
    



    public Image hider, flag;
    public Text turn_text;
    public Text question_text, btn1_text, btn2_text, btn3_text, btn4_text;
    public GameObject questions_window;
    public GameObject cell, cell3, cell2;
    public GameObject transScreen;
    public GameObject[] playerNames;
    public bool questions_time = false;
    public bool gameOver = false;

    public HeartsControll hrts;



    int turn = 1;
    int current_question_id;

    //Contest
    public static int winner = 0;
    int contest_attacker, contest_defender;
    int contest_attacker_points, contest_defender_points;
    int contesters_turn;
    bool contesting = false;
    bool attacker_answer = false;
    bool defender_answer = false;

    //Endturn
    public bool endturnQuestion = false;
    bool bonusTurn = false;
    int[] answers = new int[4];
    int[] players_in = new int[4];
    int player_answering = 1;

    public bool transition_screen = false;
    float delay = 3.5f;
    float delay_count = 0;

    //Other shit

    public CellScript current_cell;
    List<Question> questions = new List<Question>();
    public Text[] terText = new Text[4];

    public static GameObject[,] field;

    //
    Vector2 start_pos = new Vector2(-6.31f, 3.362f);
    Vector2 start_pos3 = new Vector2(-6.35f, 2.6f);
    Vector2 offset4 = new Vector2(0.93f, -0.98f);
    Vector2 offset3 = new Vector2(1.17f, -1.17f);
    Vector2 offset2 = new Vector2(2.15f, -2.1f);


    Color32 red, blue, green, orange, tmp, white;

    public static IntVec cell_pos;
    public static NetworkControllerScript controller;


    //For visual changing
    public int maxTurns = 10;
    public int players_count = 4;
    public int field_size = 6;

    //Playas
    Player[] players;

    public static Player networkPlayer;

    //Network
    Question cur_question;

    private void Awake()
    {
        players_count = PlayerPrefs.GetInt("PlayersCount", 2);
        questionUI = GameObject.Find("voprosi").GetComponent<QuestionWindowTransition>();
        net = GetComponent<Network>();
        //colors
        tmp = new Color32();
        red = new Color32(219, 99, 79, 125);
        green = new Color32(71, 255, 71, 125);
        blue = new Color32(105, 105, 216, 125);
        orange = new Color32(255, 174, 0, 125);
        white = new Color32(255, 255, 255, 255);

        //Init players
        players = new Player[players_count];
        players[0] = new Player(1, PlayerPrefs.GetString("PlayerName1", "Test"), PlayerPrefs.GetInt("Player1", 1), red);
        players[1] = new Player(2, PlayerPrefs.GetString("PlayerName2", "Test"), PlayerPrefs.GetInt("Player2", 2), green);
        if (players_count >= 3)
            players[2] = new Player(3, PlayerPrefs.GetString("PlayerName3", "Test"), PlayerPrefs.GetInt("Player3", 3), blue);
        if (players_count == 4)
            players[3] = new Player(4, PlayerPrefs.GetString("PlayerName4", "Test"), PlayerPrefs.GetInt("Player4", 4), orange);

        for (int i = 0; i < playerNames.Length; i++)
        {
            playerNames[i].GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName" + (i + 1), "Test");
        }


        cur_player = players[0];

        //field
        field = new GameObject[2 + players_count, 2 + players_count];
        field_size = 2 + players_count;

        //Test questions
        controller = this;

        hrts = GameObject.Find("Hearts").GetComponent<HeartsControll>();

        turn_text.text = "ХОД - " + turnTranslate(turn);

        //Create field
        for (int i = 0; i < field_size; i++)
            for (int k = 0; k < field_size; k++)
            {
                cell_pos = new IntVec(i, k);

                if (players_count == 4)
                {
                    field[i, k] = Instantiate(cell, start_pos + new Vector2(offset4.x * i, offset4.y * k), transform.rotation);
                }
                if (players_count == 3)
                {
                    field[i, k] = Instantiate(cell3, start_pos + new Vector2(offset3.x * i, offset3.y * k), transform.rotation);
                }
                if (players_count == 2)
                {
                    field[i, k] = Instantiate(cell2, start_pos + new Vector2(offset2.x * i, offset2.y * k), transform.rotation);
                }


                field[i, k].GetComponent<CellScript>().setPos(cell_pos.x, cell_pos.y);
                field[i, k].GetComponent<CellScript>().setNet(net);
                field[i, k].GetComponent<CellScript>().setGame(this);
            }

        createDots();
    }

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        transScreenShowing();
        for (int i = 0; i < Player.players.Count; i++)
        {
            terText[i].text = "" + Player.players[i].ters;
        }
    }



    void endTurn()
    {

    }

    public void returnToLobby()
    {
        Network.state = 0;
        Application.LoadLevel(0);
    }

    public void updateTurns()
    {
        turn += 1;
        if (turn == maxTurns)
        {
            endGame();
        }
        turn_text.text = turnTranslate(turn);
    }

    public void answer(int btn_id)
    {
        if (!questions_time)
            return;

        if (!contest)
        {
            if (checkAnswer(btn_id))
            {
                net.captureCell(playerId, current_cell.pos.x, current_cell.pos.y);
            }
            else
            {
                net.nextTurn();
            }
            hideQuestions();
        }
        if (contest)
        {
            if (checkAnswer(btn_id))
            {
                net.contestAnwer(true);
                questionUI.showWaitForOther();
            }
            else
            {
                net.contestAnwer(false);
                questionUI.showWaitForOther();
            }
        }
    }


    public void endGame()
    {
        gameOver = true;
        removeDots();
        GetComponent<EndGameScreen>().showEndGame(players);
    }


    public void restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void exit()
    {
        Application.Quit();
    }

    //VISUALS
    //
    //
    //
    public void startTransitionScreen()
    {
        delay_count = 0;
        transition_screen = true;
    }

    void transScreenShowing()
    {
        if (transition_screen)
        {
            transScreen.SetActive(true);
            delay_count += Time.deltaTime;

            if (delay_count < delay / 2)
            {
                tmp = transScreen.GetComponent<Image>().color;

                tmp.a += 1;
                transScreen.GetComponent<Image>().color = tmp;
            }
            else
            {
                tmp = transScreen.GetComponent<Image>().color;
                tmp.a -= 1;
                transScreen.GetComponent<Image>().color = tmp;
            }

            if (delay_count >= delay)
            {
                transition_screen = false;
                showQuestions();
                endTurn();

            }
        }
        else
        {
            transScreen.SetActive(false);
        }
    }

    public Color32 getPlayerColor(int p)
    {
        switch (p)
        {
            case 1:
                return red;
            case 2:
                return green;
            case 3:
                return blue;
            case 4:
                return orange;

        }
        return red;
    }

    public void createDots()
    {
        //Creating dots
        for (int i = 0; i < field_size; i++)
            for (int k = 0; k < field_size; k++)
            {
                if (field[i, k].GetComponent<CellScript>().capturable(cur_player.id))
                {
                    field[i, k].GetComponent<CellScript>().dot.GetComponent<SpriteRenderer>().color = cur_player.color;
                    field[i, k].GetComponent<CellScript>().dot.SetActive(true);
                }
                else
                    field[i, k].GetComponent<CellScript>().dot.SetActive(false);
            }
    }

    public void removePlayer(int id)
    {
        for (int i = 0; i < field_size; i++)
            for (int k = 0; k < field_size; k++)
                if (field[i, k].GetComponent<CellScript>().getOwner() == id)
                    field[i, k].GetComponent<CellScript>().changeOwner(0);
    }

    public void removeDots()
    {
        for (int i = 0; i < field_size; i++)
            for (int k = 0; k < field_size; k++)
            {
                field[i, k].GetComponent<CellScript>().dot.SetActive(false);
            }
    }

    public void hideQuestions()
    {
        questions_time = false;
        questionUI.hideQuestions();
    }

    public void showQuestions()
    {
        changeResponsePosition();
        setHiderColor(cur_player.color);
        questions_time = true;
        questions_window.SetActive(true);
        questionUI.showQuestions();
    }

    bool checkAnswer(int btn_id)
    {
        switch (btn_id)
        {
            case 1:
                if (btn1_text.text == cur_question.ans1)
                {
                    questionUI.showMark(true);
                    return true;

                }
                break;
            case 2:
                if (btn2_text.text == cur_question.ans1)
                {
                    questionUI.showMark(true);
                    return true;
                }
                break;
            case 3:
                if (btn3_text.text == cur_question.ans1)
                {
                    questionUI.showMark(true);
                    return true;
                }
                break;
            case 4:
                if (btn4_text.text == cur_question.ans1)
                {
                    questionUI.showMark(true);
                    return true;
                }
                break;
            default:
                questionUI.showMark(false);
                return false;
        }
        questionUI.showMark(false);
        return false;
    }

    public void changeResponsePosition()
    {
        int rand;
        string[] questions_temp = new string[4];
        string temp;
        questions_temp[0] = btn1_text.text;
        questions_temp[1] = btn2_text.text;
        questions_temp[2] = btn3_text.text;
        questions_temp[3] = btn4_text.text;
        for (int i = 0; i < 4; i++)
        {
            rand = Random.Range(0, 4);
            temp = questions_temp[rand];
            questions_temp[rand] = questions_temp[i];
            questions_temp[i] = temp;
        }
        btn1_text.text = questions_temp[0];
        btn2_text.text = questions_temp[1];
        btn3_text.text = questions_temp[2];
        btn4_text.text = questions_temp[3];
    }

    public void setHiderColor(Color32 col)
    {
        if (!endturnQuestion && !contesting)
        {
            tmp = col;
            tmp.a = 0;
            hider.color = tmp;

            tmp.a = 255;
            flag.color = tmp;
        }
        else
        {
            tmp = col;
            tmp.a = 50;
            hider.color = tmp;

            tmp.a = 255;
            flag.color = tmp;
        }
    }

    string turnTranslate(int t)
    {
        switch (t)
        {
            case 1:
                return "I";
            case 2:
                return "II";
            case 3:
                return "III";
            case 4:
                return "IV";
            case 5:
                return "V";
            case 6:
                return "VI";
            case 7:
                return "VII";
            case 8:
                return "VIII";
            case 9:
                return "IX";
            case 10:
                return "X";
            default:
                return "I";
        }
    }

    //Cells shit

    public void changeCellOwner(int playerId, int x, int y)
    {
        Debug.Log("x :" + x + "y :" + y + " id " + playerId);
        Debug.Log(field[x, y]);
        field[x, y].GetComponent<CellScript>().changeOwner(playerId);
    }


    //Networking
    public void networkSetQuestions(string[] question)
    {
        cur_question = new Question(question[0], question[1], question[2], question[3], question[4]);
        question_text.text = question[0];
        btn1_text.text = question[1];
        btn2_text.text = question[2];
        btn3_text.text = question[3];
        btn4_text.text = question[4];
        changeResponsePosition();
    }


    public void startGame(string [] gameInfo)
    {
        
    }
}
