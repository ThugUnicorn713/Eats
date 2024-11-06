using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>();
    public Transform[] playerSlots;
    public Transform[] cpuSlots;
    public bool[] playerEmptySlots;
    public bool[] cpuEmptySlots;

    public PlayerManager playerManager;


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

    public void DrawCard(bool isCPU) //bool checks if it goes to CPU hand or players hand 
    {
       if (deck.Count >= 1)
       {
            GameObject ranCard = deck[Random.Range(0, deck.Count)];

            Transform[] slots = isCPU ? cpuSlots : playerSlots;
            bool[] emptySlots = isCPU ? cpuEmptySlots : playerEmptySlots;

            for (int i = 0; i < emptySlots.Length; i++)
            {
                if (emptySlots[i] == true)
                {
                    ranCard.SetActive(true);
                    ranCard.transform.SetParent(slots[i]);
                    ranCard.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    ranCard.transform.position = slots[i].position;
                    emptySlots[i] = false;
                    deck.Remove(ranCard);
                    return;
                }
            }
       }
    }
}


