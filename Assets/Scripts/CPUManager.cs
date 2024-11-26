using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CPUManager : MonoBehaviour
{
    public Transform[] cpuHandPositions;
    public List<CardData> cpuHand = new List<CardData>();
    
    public GameObject cpuCardPlace;
    public GameObject playerWinPanel;

    public static bool initialied = false;  
    public static bool npcHasPlayed = false;
    public static bool npcNeedsToDrawCard = true;

    public static int npcHealthValue;

    public GameManager gameManager;

    private void Start()
    {
        npcHealthValue = 100;
    }

    public void FixedUpdate()
    {
        if (initialied == false)
        {
            if (npcNeedsToDrawCard == true) 
            {
                //UpdateCPUHandPositions(); //sync positions before we add cards
                AddTransformstoHand();
                npcNeedsToDrawCard = false;
            }
           
            GameObject selectedCard = NPCPicksCard<GameObject>.testMethod(this); //grab card from logic
            
            int npcCostValue = selectedCard.GetComponent<Card>().cardInfo.hpCost; 

            //update cpu list and bools for availability 
            List<GameObject> npcHandList = gameManager.GetNPCHand();

            int cardIndex = npcHandList.IndexOf(selectedCard);
            bool[] npcHandBools = gameManager.GetNPCBools();

            // Validate cardIndex
            if (cardIndex < 0 || cardIndex >= npcHandBools.Length)
            {
                Debug.LogError("Card index out of bounds when removing NPC card.");
                return;
            }

            if (cardIndex >= 0 && cardIndex < npcHandBools.Length)
            {
                npcHandBools[cardIndex] = true; // Mark the slot as empty
                npcHandList.Remove(selectedCard);
            }
            else
            {
                Debug.LogError($"Invalid card index: {cardIndex}");
            }


            //npcHandBools[cardIndex] = true;
            //npcHandList.Remove(selectedCard);

            Debug.Log("CPU MAnager got the NPC card");

            //place card down
            selectedCard.transform.parent = cpuCardPlace.transform;
            selectedCard.transform.position = cpuCardPlace.transform.position;
            selectedCard.transform.rotation = Quaternion.Euler(0, 0, 180);
            npcHealthValue -= npcCostValue;
            Debug.Log("The Npc paid the cost :" + npcCostValue + " hp ");

            npcHasPlayed = true;

            initialied = true;

            // Revalidate positions after card removal
            //UpdateCPUHandPositions();
        }

        if(npcHealthValue == 0)
        {
            Playerwins();
        }
        
    }

    public void AddTransformstoHand()
    {
        foreach (Transform t in cpuHandPositions)
        {
            if (t == null) //continue even if some slots are empty
            {
                Debug.LogWarning("Encountered a null transform in CPU hand positions.");
                continue;
            }

            Card cardComponent = t.GetComponentInChildren<Card>();
            if (cardComponent == null) //continue even if some slots are empty
            {
                Debug.LogWarning($"No Card component found in children of {t.name}.");
                continue;
            }

            if (t != null)
            {
                CardData cardData = t.GetComponentInChildren<Card>().cardInfo;

                if (cardData != null)
                {   
                    cpuHand.Add(cardData);
                }
               
            }
        }
    }

    public void UpdateCPUHandPositions()
    {
        cpuHandPositions = cpuCardPlace.GetComponentsInChildren<Transform>();
        Debug.Log("Updated CPU hand positions. Total positions: " + cpuHandPositions.Length);
    }

    public int GetCurrentNPCHealth()
    {
        return npcHealthValue;
    }

    public void Playerwins()
    {
        playerWinPanel.SetActive(true);
    }

}
