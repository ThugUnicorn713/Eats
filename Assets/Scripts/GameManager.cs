using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>();
    public List<GameObject> playerHand = new List<GameObject>();
    public List<GameObject> npcHand = new List<GameObject>();

    public Transform[] playerSlots;
    public Transform[] cpuSlots;
    public bool[] playerEmptySlots;
    public bool[] cpuEmptySlots;

    public GameObject npcPlaySpot;
    public GameObject playerPlaySpot;
    public GameObject discardPile;
    public GameObject flyCard;
    public GameObject playerCardPlace;
    public GameObject npcCardPlace;

    private Transform npcDiscard;
    private Transform playerDiscard;

    public int npcPlayedValue;
    public int playerPlayedValue;
    public int npcCost;
    public int playerCost;
    public int playerHealth;
    public int npcHealth;

   //public CPUManager cpuManager;

    public bool[] GetPlayerBools()
    {
        return playerEmptySlots;
    }
    public bool[] GetNPCBools()
    {
        return cpuEmptySlots;
    }
    public List<GameObject> GetPlayerHand()
    {
        return playerHand;
    }
    public List<GameObject> GetNPCHand()
    {
        return npcHand;
    }
    public void Start() 
    { 
        for (int i = 0; i < playerEmptySlots.Length; ++i)
        {
            if (playerEmptySlots[i] && deck.Count > 0)
            {
                DrawCard(false);
            }
        }
       
        for (int i = 0; i < cpuEmptySlots.Length; ++i)
        {
            if (cpuEmptySlots[i] && deck.Count > 0)
            {
                DrawCard(true);
            }
        }
    }

    public void Update()
    {
        bool npcFinal = CPUManager.npcHasPlayed;
        bool playerFinal = PlayerManager.playerHasPlayed;

        if (npcFinal == true && playerFinal == true)
        {
           // Debug.Log("Both players have placed there cards");
            CompareCards();
            DiscardCards();

            CPUManager.npcHasPlayed = false;
            PlayerManager.playerHasPlayed = false;
        }

        GrabFlyCardForPlayer(flyCard);
        GrabFlyCardForNPC(flyCard);
    }


    public void DrawCard(bool isCPU) //bool checks if it goes to CPU hand or players hand 
    {
       if (deck.Count >= 1)
       {
            //Debug.Log($"Deck contains {deck.Count} cards.");

            GameObject ranCard = deck[Random.Range(0, deck.Count)];
            //Debug.Log($"Randomly selected card: {ranCard.name}");

            Transform[] slots = isCPU ? cpuSlots : playerSlots;
            bool[] emptySlots = isCPU ? cpuEmptySlots : playerEmptySlots;
            List<GameObject> hand = isCPU ? npcHand : playerHand;

            for (int i = 0; i < emptySlots.Length; i++)
            {
                if (emptySlots[i] == true)
                {
                    //Debug.Log($"Found empty slot: {slots[i].name}");

                    ranCard.SetActive(true);
                    ranCard.transform.SetParent(slots[i]);
                    ranCard.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    ranCard.transform.position = slots[i].position;
                    emptySlots[i] = false;
                  
                    hand.Add(ranCard);
                    //Debug.Log($"{ranCard.name} added to {(isCPU ? "NPC" : "Player")} hand. Hand size: {hand.Count}");

                    deck.Remove(ranCard);

                    //Debug.Log($"Deck size after removal: {deck.Count}");
                    return;
                }
            }
       }
       else
       {
            Debug.Log("The deck is empty. No card can be drawn.");
       }
    }

    public void CompareCards()
    {
        playerHealth = PlayerManager.healthValue;
        npcHealth = CPUManager.npcHealthValue;

        if (npcPlaySpot != null)
        {
            int value = npcPlaySpot.GetComponentInChildren<Card>().cardInfo.value;
            npcPlayedValue = value;

            int cost = npcPlaySpot.GetComponentInChildren<Card>().cardInfo.hpCost;
            npcCost = cost;
        }

        if (playerPlaySpot != null)
        {
            int value = playerPlaySpot.GetComponentInChildren<Card>().cardInfo.value;
            playerPlayedValue = value;

            int cost = playerPlaySpot.GetComponentInChildren<Card>().cardInfo.hpCost;
            playerCost = cost;
        }

        if (playerPlayedValue > npcPlayedValue)
        {
            Debug.Log("Player Wins round!");
            PlayerManager.healthValue += npcCost;
            UIManager.instance.UpdateHealthUI();
            UIManager.instance.UpdateNPCHealthUI();
            UIManager.instance.GetPlayerWon();
            UIManager.instance.TurnOffPlayerWon();
            Debug.Log("Player health :" +  playerHealth);
            DrawCardForNPC();
        }
        else if (playerPlayedValue < npcPlayedValue)
        {
            Debug.Log("NPC Wins Round!");
            CPUManager.npcHealthValue += playerCost;
            UIManager.instance.UpdateHealthUI();
            UIManager.instance.UpdateNPCHealthUI();
            UIManager.instance.GetNPCWon();
            UIManager.instance.TurnOffNPCWon();
            Debug.Log("npc health :" + npcHealth);
            DrawCardForPlayer();
        }
        else
        {
            Debug.Log("We have a tie");
            Debug.Log("Player health :" + playerHealth);
            Debug.Log("npc health :" + npcHealth);
            UIManager.instance.UpdateNPCHealthUI();
            UIManager.instance.UpdateHealthUI();
            UIManager.instance.GetTIE();
            UIManager.instance.TurnOffTIE();
        }

        CPUManager.initialied = false;
    }

    public void DiscardCards()
    {
        playerDiscard = playerPlaySpot.transform.GetChild(0);
        npcDiscard = npcPlaySpot.transform.GetChild(0);

        playerDiscard.transform.parent = discardPile.transform;
        playerDiscard.transform.position = discardPile.transform.position;
        playerDiscard.transform.rotation = Quaternion.Euler(0, 0, 180);

        npcDiscard.transform.parent = discardPile.transform;
        npcDiscard.transform.position = discardPile.transform.position;
        npcDiscard.transform.rotation = Quaternion.Euler(0, 0, 180);


    }

    public void DrawCardForPlayer()
    {
        DrawCard(false);
        Debug.Log("I drew a card for the player");
    }

    public void DrawCardForNPC()
    {
        CPUManager.npcNeedsToDrawCard = true;
        DrawCard(true);

        // After drawing a card, ensure positions are updated
        //cpuManager.UpdateCPUHandPositions();

        Debug.Log("I drew a card for the NPC and updated hand positions.");
        
    }

    public void GrabFlyCardForPlayer(GameObject flyCard)
    {
        if (playerEmptySlots.All(isEmpty => isEmpty))
        {
            Debug.Log("All player slots are empty.");
            GrabFlyCard(flyCard);
        }
        else
        {
            Debug.Log("Not all player slots are empty.");
        }
    }

    private void GrabFlyCard(GameObject flyCard)
    {
        if (flyCard != null)
        {
            Debug.Log($"Grabbing object: {flyCard.name}");
            
            flyCard.SetActive(true);
            flyCard.transform.parent = playerCardPlace.transform;
            flyCard.transform.position = playerCardPlace.transform.position;
            flyCard.transform.rotation = Quaternion.Euler(0, 0, 0);
            PlayerManager.playerHasPlayed = true;
        }
        else
        {
            Debug.Log("Target object is null!");
        }
    }

    public void GrabFlyCardForNPC(GameObject flyCard)
    {
        if (cpuEmptySlots.All(isEmpty => isEmpty))
        {
            Debug.Log("All player slots are empty.");
            GrabFlyCardNPC(flyCard);
        }
        else
        {
            Debug.Log("Not all npc slots are empty.");
        }
    }

    private void GrabFlyCardNPC(GameObject flyCard)
    {
        if (flyCard != null)
        {
            Debug.Log($"Grabbing object: {flyCard.name}");

            flyCard.SetActive(true);
            flyCard.transform.parent = npcCardPlace.transform;
            flyCard.transform.position = npcCardPlace.transform.position;
            flyCard.transform.rotation = Quaternion.Euler(0, 0, 0);
            CPUManager.npcHasPlayed = true;
        }
        else
        {
            Debug.Log("Target object is null!");
        }
    }
}


