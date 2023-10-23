using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vsPlayer : MonoBehaviour
{
    enum currentState
    {
        Start = 0,
        Game,
        End,
    }

    enum Turn
    {
        Me = 0,
        You,
    }

    enum Sign
    {
        None = 0,
        PlayerX,
        PlayerO,
    }

    Tcp tcp;
    public InputField inputField;

    public Button emptyGrid;
    public Button gridX;
    public Button gridO;

    int[] board = new int[9];

    currentState state;

    Sign playerTurn;
    Sign myTurn;
    Sign opponentTurn;
    Sign winner;

    private void Start()
    {
        tcp = GetComponent<Tcp>();
        state = currentState.Start;

        for (int i = 0; i < board.Length; ++i)
        {
            board[i] = (int)Sign.None;
        }
    }

    public void ServerStart()
    {
        tcp.StartServer(10000, 10);
    }

    public void ClientStart()
    {
        tcp.Connect(inputField.text, 10000);
    }

    //private void OnGUI()
    //{
    //    if (!Event.current.type.Equals(EventType.Repaint)) return;
    //    Graphics.DrawTexture(new Rect(0, 0, 400, 400), emptyGrid);
    //}

    private void Update()
    {
        if (!tcp.IsConnect()) return;
        if (state == currentState.Start)
        {
            UpdateStart();
        }

        if (state == currentState.Game)
        {
            UpdateGame();
        }

        if (state == currentState.End)
        {
            UpdateEnd();
        }
    }

    void UpdateStart()
    {
        state = currentState.Game;
        playerTurn = Sign.PlayerX;

        if (tcp.IsServer())
        {
            myTurn = Sign.PlayerX;
            opponentTurn = Sign.PlayerO;
        }
        else
        {
            myTurn = Sign.PlayerO;
            opponentTurn = Sign.PlayerX;
        }
    }

    void UpdateGame()
    {
        bool bSet = false;

        if(playerTurn == myTurn)
        {
            bSet = MyTurn();
        }
    }

    bool SetTurn(int i, Sign sign)
    {
        if (board[i] == (int)Sign.None)
        {
            board[i] = (int)sign;
            return true;
        }
        return false;
    }

    int PosToGrid(Button button)
    {
        //TODO
    }

    bool MyTurn()
    {
        bool bClick = Input.GetMouseButtonDown(0);
        if (!bClick) return false;

        Vector3 pos = Input.mousePosition;

        int i = PosToGrid(pos);
        if (i == -1) return false;

    }
}
