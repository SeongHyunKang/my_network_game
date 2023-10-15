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
        if (turnCount >= 9)
        {
            SetGameOverText("Draw...");
        }
        // Turn Over
        TurnOver();

        if (playerTurn == "O")
        {
            MiniMaxAI();
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

    #region AI_MiniMax
    void MiniMaxAI()
    {
        int bestMove = -1;
        int bestValue = -1000;

        // Traverse all cells to find the best move
        for (int i = 0; i < 9; i++)
        {
            // Check if cell is empty
            if (buttonList[i].text == "")
            {
                // Make the move for AI
                buttonList[i].text = "O";

                // Compute evaluation function for this move
                int moveValue = MiniMax(0, false);

                // Undo the move
                buttonList[i].text = "";

                // If the current move is better than the best move
                if (moveValue > bestValue)
                {
                    bestMove = i;
                    bestValue = moveValue;
                }
            }
        }

        // Make the best move
        if (bestMove != -1)
        {
            buttonList[bestMove].text = "O";
            buttonList[bestMove].GetComponentInParent<Button>().interactable = false;
            EndTurn();
        }
    }

    private int EvaluateBoard()
    {
        // Check rows for victory
        for (int row = 0; row < 3; row++)
        {
            if (buttonList[row * 3].text == buttonList[row * 3 + 1].text && buttonList[row * 3 + 1].text == buttonList[row * 3 + 2].text)
            {
                if (buttonList[row * 3].text == "O")
                    return +10;
                else if (buttonList[row * 3].text == "X")
                    return -10;
            }
        }

        // Check columns for victory
        for (int col = 0; col < 3; col++)
        {
            if (buttonList[col].text == buttonList[col + 3].text && buttonList[col + 3].text == buttonList[col + 6].text)
            {
                if (buttonList[col].text == "O")
                    return +10;
                else if (buttonList[col].text == "X")
                    return -10;
            }
        }

        // Check diagonals for victory
        if (buttonList[0].text == buttonList[4].text && buttonList[4].text == buttonList[8].text)
        {
            if (buttonList[0].text == "O")
                return +10;
            else if (buttonList[0].text == "X")
                return -10;
        }
        if (buttonList[2].text == buttonList[4].text && buttonList[4].text == buttonList[6].text)
        {
            if (buttonList[2].text == "O")
                return +10;
            else if (buttonList[2].text == "X")
                return -10;
        }

        // No one has won
        return 0;
    }

    private bool AreMovesLeft()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            if (buttonList[i].text == "")
                return true;
        }
        return false;
    }

    private int MiniMax(int depth, bool isMax)
    {
        int boardVal = EvaluateBoard();

        // If AI has won the game, return his/her evaluated score
        if (boardVal == 10)
            return boardVal;

        // If the opponent has won the game, return his/her evaluated score
        if (boardVal == -10)
            return boardVal;

        // If there are no more moves and no winner, return 0
        if (AreMovesLeft() == false)
            return 0;

        // If this is AI's move
        if (isMax)
        {
            int best = -1000;

            // Traverse all cells
            for (int i = 0; i < 9; i++)
            {
                // Check if cell is empty
                if (buttonList[i].text == "")
                {
                    // Make the move
                    buttonList[i].text = "O";

                    // Call MiniMax recursively and choose the maximum value
                    best = Mathf.Max(best, MiniMax(depth + 1, !isMax));

                    // Undo the move
                    buttonList[i].text = "";
                }
            }
            return best;
        }

        // If this is player's move
        else
        {
            int best = 1000;

            // Traverse all cells
            for (int i = 0; i < 9; i++)
            {
                // Check if cell is empty
                if (buttonList[i].text == "")
                {
                    // Make the move
                    buttonList[i].text = "X";

                    // Call MiniMax recursively and choose the minimum value
                    best = Mathf.Min(best, MiniMax(depth + 1, !isMax));

                    // Undo the move
                    buttonList[i].text = "";
                }
            }
            return best;
        }
    }
    #endregion
}