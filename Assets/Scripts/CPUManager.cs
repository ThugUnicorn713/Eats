using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CPUManager : MonoBehaviour
{
    public Transform[] cpuHandPositions;
    public List<CardData> cpuHand = new List<CardData>();

    private bool initialied = false;    

    public void Update()
    {
        if (initialied == false)
        {
            AddTransformstoHand();
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

}
