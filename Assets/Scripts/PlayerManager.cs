using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static int healthValue;
    public Collider clickableArea;

    private GameObject maybeCard;

    public GameObject playedCard;
    public GameObject payTollPanel;
    public GameObject playerCardPlace;
    public GameObject npcWinPanel;

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI cardNameText;

    public int cardValue;
    public string cardTitle;
    public int cardHPCost;

    public static bool playerHasPlayed = false;

    public GameManager gameManager;

    private Camera mainCamera;

    public void Start()
    {
        healthValue = 100;
        mainCamera = Camera.main;   
       
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (clickableArea.bounds.Contains(hit.point))
                {
                    maybeCard = hit.collider.gameObject;
                    cardHPCost = maybeCard.GetComponent<Card>().cardInfo.hpCost;
                    cardTitle = maybeCard.GetComponent<Card>().cardInfo.cardName;
                    PrintHPCost();
                    PrintCardName();

                    payTollPanel.SetActive(true);
                }

            }
        }

        if (healthValue == 0)
        {

        }

    }

    public int GetCurrentHealth()
    {
        return healthValue;
    }

    public void PrintHPCost()
    {
        if (hpText != null && cardHPCost != 0)
        {
            hpText.text = cardHPCost.ToString() + " HP";
        }
    }

    public void PrintCardName()
    {
        if (cardNameText != null && cardTitle != null)
        {
            cardNameText.text = cardTitle;
        }
    }

    public void PayToll()
    {
        playedCard = maybeCard;

        List<GameObject> playerHandList = gameManager.GetPlayerHand();

        int cardIndex = playerHandList.IndexOf(playedCard);
        bool[] playerHandBools = gameManager.GetPlayerBools();

        if (cardIndex >= 0 && cardIndex < playerHandBools.Length)
        {
            playerHandBools[cardIndex] = true; // Mark the slot as empty
            playerHandList.Remove(playedCard);
        }
        else
        {
            Debug.LogError($"Invalid card index: {cardIndex}");
        }


        //playerHandBools[cardIndex] = true;
        //playerHandList.Remove(playedCard);


        cardValue = playedCard.GetComponent<Card>().cardInfo.value;

        switch (cardValue)
        {
            case 0:
                Debug.Log("player has paid the 5hp!");
                healthValue -= 5;
                break;
            case 1:
                Debug.Log("player has paid the 10hp!");
                healthValue -= 10;
                break;
            case 2:
                Debug.Log("player has paid the 15hp!");
                healthValue -= 15;
                break;
            case 3:
                Debug.Log("player has paid the 20hp!");
                healthValue -= 20;
                break;
            case 4:
                Debug.Log("player has paid the 25hp!");
                healthValue -= 25;
                break;
        }

        payTollPanel.SetActive(false);

        if (playedCard != null)
        {
            playedCard.transform.parent = playerCardPlace.transform;
            playedCard.transform.position = playerCardPlace.transform.position;
            playedCard.transform.rotation = Quaternion.Euler(0, 0, 0);
            playerHasPlayed = true;

        }

    }

    public void DoNotPayToll()
    {
        payTollPanel.SetActive(false);
    }

    public void GameOver()
    {
        if (healthValue < 0)
        {
            Debug.Log("Game is over!");
        }
    }

    public static void Pay1()
    {
        Debug.Log("player has paid the 1hp!");
        healthValue -= 1;
        UIManager.instance.UpdateHealthUI();
    }
   
    public static void Pay25()
    {
        Debug.Log("player has paid the 25hp!");
        healthValue -= 25;
        UIManager.instance.UpdateHealthUI();
    }

    public void NPCwins()
    {
        npcWinPanel.SetActive(true);
    }
}
