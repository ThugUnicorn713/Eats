using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int healthValue;
    public Collider clickableArea;

    private GameObject maybeCard;

    public GameObject playedCard;
    public GameObject payTollPanel;
    public GameObject playerCardPlace;

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI cardNameText;

    public int cardValue;
    public string cardTitle;
    public int cardHPCost;
    
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
            playedCard.transform.position = playerCardPlace.transform.position;
            playedCard.transform.rotation = Quaternion.Euler(0, 0, 0);

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

    public void Pay1()
    {
        Debug.Log("player has paid the 1hp!");
        healthValue -= 1;
    }
   
    public void Pay25()
    {
        Debug.Log("player has paid the 25hp!");
        healthValue -= 25;
    }
}
