using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CPUManager : MonoBehaviour
{
    public Transform[] cpuHandPositions;
    public List<CardData> cpuHand = new List<CardData>();
    public GameObject cpuCardPlace;

    public static bool initialied = false;  
    public static bool npcHasPlayed = false;
    public static bool npcNeedsToDrawCard = true;

    public static int npcHealthValue;

    public GameManager gameManager;

    private void Start()
    {
        npcHealthValue = 100;
    }

    public void Update()
    {
        if (initialied == false)
        {
            if (npcNeedsToDrawCard == true) 
            {
                AddTransformstoHand();
                npcNeedsToDrawCard = false;
            }
           
            GameObject selectedCard = NPCPicksCard<GameObject>.testMethod(this);
            
            int npcCostValue = selectedCard.GetComponent<Card>().cardInfo.hpCost;

            List<GameObject> npcHandList = gameManager.GetNPCHand();

            int cardIndex = npcHandList.IndexOf(selectedCard);
            bool[] npcHandBools = gameManager.GetNPCBools();

            npcHandBools[cardIndex] = true;
            npcHandList.Remove(selectedCard);

            Debug.Log("CPU MAnager got the NPC card");

            selectedCard.transform.parent = cpuCardPlace.transform;
            selectedCard.transform.position = cpuCardPlace.transform.position;
            selectedCard.transform.rotation = Quaternion.Euler(0, 0, 0);
            npcHealthValue -= npcCostValue;

            Debug.Log("The Npc paid the cost :" + npcCostValue + " hp ");

            npcHasPlayed = true;

            initialied = true;
        }

        
    }

    public void AddTransformstoHand()
    {
        foreach (Transform t in cpuHandPositions)
        {
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

    public int GetCurrentNPCHealth()
    {
        return npcHealthValue;
    }

}
