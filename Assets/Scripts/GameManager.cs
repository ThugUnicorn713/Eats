using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>();
    public Transform[] playerSlots;
    public bool[] emptySlots;

    public void Start() 
    { 
        for (int i = 0; i < emptySlots.Length; ++i)
        {
            if (emptySlots[i] && deck.Count > 0)
            {
                DrawCard();
            }
        }
        
    }

    public void DrawCard()
    {
       if (deck.Count >= 1)
       {
            GameObject ranCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < emptySlots.Length; i++)
            {
                if (emptySlots[i] == true)
                {
                    ranCard.SetActive(true);
                    ranCard.transform.SetParent(null);
                    ranCard.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    ranCard.transform.position = playerSlots[i].position;
                    //Debug.Log("Positioning card at: " + playerSlots[i].position);
                    emptySlots[i] = false;
                    deck.Remove(ranCard);
                    return;
                }
            }
       }
    }

}
