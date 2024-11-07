using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CPUManager : MonoBehaviour
{
    public Transform[] cpuHandPositions;
    public List<CardData> cpuHand = new List<CardData>();
    public GameObject cpuCardPlace;

    private bool initialied = false;    

    public void Update()
    {
        if (initialied == false)
        {
            AddTransformstoHand();
            initialied = true;
        }

        GameObject selectedCard = NPCPicksCard<GameObject>.testMethod(this);

        Debug.Log("CPU MAnager got the NPC card");

        selectedCard.transform.position = cpuCardPlace.transform.position;
        selectedCard.transform.rotation = Quaternion.Euler(0, 0, 0);
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

}
