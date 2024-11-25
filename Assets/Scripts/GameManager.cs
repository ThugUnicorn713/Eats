using NUnit.Framework;
using System.Collections.Generic;
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

    private Transform npcDiscard;
    private Transform playerDiscard;

    public int npcPlayedValue;
    public int playerPlayedValue;
    public int npcCost;
    public int playerCost;
    public int playerHealth;
    public int npcHealth;

    public PlayerManager playerManager;

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
            Debug.Log("Both players have placed there cards");
            CompareCards();
            DiscardCards();

            CPUManager.npcHasPlayed = false;
            PlayerManager.playerHasPlayed = false;
        }
        
    }


    public void DrawCard(bool isCPU) //bool checks if it goes to CPU hand or players hand 
    {
       if (deck.Count >= 1)
       {
            Debug.Log($"Deck contains {deck.Count} cards.");

            GameObject ranCard = deck[Random.Range(0, deck.Count)];
            //Debug.Log($"Randomly selected card: {ranCard.name}");

            Transform[] slots = isCPU ? cpuSlots : playerSlots;
            bool[] emptySlots = isCPU ? cpuEmptySlots : playerEmptySlots;
            List<GameObject> hand = isCPU ? npcHand : playerHand;

            for (int i = 0; i < emptySlots.Length; i++)
            {
                if (emptySlots[i] == true)
                {
                    Debug.Log($"Found empty slot: {slots[i].name}");

                    ranCard.SetActive(true);
                    ranCard.transform.SetParent(slots[i]);
                    ranCard.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    ranCard.transform.position = slots[i].position;
                    emptySlots[i] = false;
                  
                    hand.Add(ranCard);
                    //Debug.Log($"{ranCard.name} added to {(isCPU ? "NPC" : "Player")} hand. Hand size: {hand.Count}");

                    deck.Remove(ranCard);

                    Debug.Log($"Deck size after removal: {deck.Count}");
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
            playerHealth += npcCost;
            Debug.Log("Player health :" +  playerHealth);
            DrawCardForNPC();
        }
        else if (playerPlayedValue < npcPlayedValue)
        {
            Debug.Log("NPC Wins Round!");
            npcHealth += playerCost;
            Debug.Log("npc health :" + npcHealth);
            DrawCardForPlayer();
        }
        else
        {
            Debug.Log("We have a tie");
            Debug.Log("Player health :" + playerHealth);
            Debug.Log("npc health :" + npcHealth);
        }

        CPUManager.initialied = false;
    }

    public void DiscardCards()
    {
        playerDiscard = playerPlaySpot.transform.GetChild(0);
        npcDiscard = npcPlaySpot.transform.GetChild(0);

        playerDiscard.transform.parent = discardPile.transform;
        playerDiscard.transform.position = discardPile.transform.position;
        playerDiscard.transform.rotation = Quaternion.Euler(0, 0, 0);

        npcDiscard.transform.parent = discardPile.transform;
        npcDiscard.transform.position = discardPile.transform.position;
        npcDiscard.transform.rotation = Quaternion.Euler(0, 0, 0);


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
        Debug.Log("I drew a card for the NPC");
    }
}


