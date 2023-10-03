using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridSpace : MonoBehaviour
{
    private GameManager m_manager;

    public Button gridButton;
    public TextMeshProUGUI buttonText;
    public Image buttonImage;
    public string playerTurn;

    private void Start()
    {
        gridButton = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonImage = GetComponentInChildren<Image>();
    }

    public void SetSpace()
    {
        buttonText.text = m_manager.GetPlayerTurn();
        gridButton.interactable = false;
        m_manager.EndTurn();
    }

    public void SetController(GameManager manager)
    {
        m_manager = manager;
    }
}
