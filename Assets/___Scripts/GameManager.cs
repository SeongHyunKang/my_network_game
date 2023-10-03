using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager manager;

    public static GameManager m_instance()
    {
        if (manager == null)
        {
            manager = new GameManager();
        }
        return manager;
    }

    public TextMeshProUGUI[] buttonList;

    private void Awake()
    {
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
        return "?";
    }

    public void EndTurn()
    {
        // TODO : Add code that ends the current turn
    }
}
