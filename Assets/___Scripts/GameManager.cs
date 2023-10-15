using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


[System.Serializable]
public class Player
{
    public Image panel;
    public TextMeshProUGUI text;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    private string playerTurn;
    private int turnCount;
    public Player playerX;
    public Player playerO;

    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    public TextMeshProUGUI[] buttonList;
    private static GameManager manager;

    public static GameManager instance
    {
        get
        {
            if (manager == null)
            {
                manager = new GameManager();
            }
            return manager;
        }
    }

    private void Awake()
    {
        playerTurn = "X";
        SetPlayerTurnDisplay(playerX, playerO);
        turnCount = 0;
        gameOverPanel.SetActive(false);
        SetControllerOnButtons();
    }

    void SetControllerOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetController(this);
        }
    }

    public string GetPlayerTurn()
    {
        // TODO : this will return X or O based on which player turn it is!
        return playerTurn;
    }

    public void EndTurn()
    {
        turnCount++;

        // Row
        if (buttonList[0].text == playerTurn && buttonList[1].text == playerTurn && buttonList[2].text == playerTurn)
        {
            GameOver();
        }
        if (buttonList[3].text == playerTurn && buttonList[4].text == playerTurn && buttonList[5].text == playerTurn)
        {
            GameOver();
        }
        if (buttonList[6].text == playerTurn && buttonList[7].text == playerTurn && buttonList[8].text == playerTurn)
        {
            GameOver();
        }

        // Column
        if (buttonList[0].text == playerTurn && buttonList[3].text == playerTurn && buttonList[6].text == playerTurn)
        {
            GameOver();
        }
        if (buttonList[1].text == playerTurn && buttonList[4].text == playerTurn && buttonList[7].text == playerTurn)
        {
            GameOver();
        }
        if (buttonList[2].text == playerTurn && buttonList[5].text == playerTurn && buttonList[8].text == playerTurn)
        {
            GameOver();
        }

        // Diagonal
        if (buttonList[0].text == playerTurn && buttonList[4].text == playerTurn && buttonList[8].text == playerTurn)
        {
            GameOver();
        }
        if (buttonList[2].text == playerTurn && buttonList[4].text == playerTurn && buttonList[6].text == playerTurn)
        {
            GameOver();
        }

        // Draw
        if(turnCount >= 9)
        {
            SetGameOverText("Draw...");
        }
        // Turn Over
        TurnOver();

        if(playerTurn == "O")
        {
            TurnAI();
        }
    }

    
    void GameOver()
    {
        boardInteractionManager(false);
        SetGameOverText(playerTurn + " Wins!!");
    }

    void TurnOver()
    {
        playerTurn = (playerTurn == "X") ? "O" : "X";
        if (playerTurn == "X")
        {
            SetPlayerTurnDisplay(playerX, playerO);
        }
        else
        {
            SetPlayerTurnDisplay(playerO, playerX);
        }
    }

    void SetGameOverText(string text)
    {
        gameOverText.text = text;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        playerTurn = "X";
        turnCount = 0;
        gameOverPanel.SetActive(false);

        boardInteractionManager(true);

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }

        SetPlayerTurnDisplay(playerX, playerO);
    }

    void boardInteractionManager(bool set)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = true;
        }
    }

    void SetPlayerTurnDisplay(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;

        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    void TurnAI()
    {
        bool foundEmptySpot = false;

        while(!foundEmptySpot)
        {
            int randomNumber = Random.Range(0, 9);
            if (buttonList[randomNumber].GetComponentInParent<Button>().IsInteractable())
            {
                buttonList[randomNumber].GetComponentInParent<Button>().onClick.Invoke();
                foundEmptySpot = true;
            }
        }
    }
}
