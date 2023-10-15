using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    private string playerTurn;
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
    }

    
    void GameOver()
    {
        for(int i = 0;i < buttonList.Length;i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false;
        }
    }
}
