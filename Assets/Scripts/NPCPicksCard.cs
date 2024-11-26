using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public class NPCPicksCard<T> : MonoBehaviour {

    private static Random RANDOM = new Random();

    public static GameObject testMethod(CPUManager manager) {


        //List<GameObject> weightList = new List<GameObject>();
        List<GameObject> cardList = new List<GameObject>();

       

        foreach (var item in manager.cpuHandPositions) {
          
            if(item.childCount >= 2)
            {
                cardList.Add(item.GetChild(1).gameObject);
                //                        ^^^ If you have a sphere, its 1, if you delete the object, make this zero
               Debug.Log("DEBUG YOU ARE LOOKING FOR!!!" + item.GetChild(1).gameObject.GetComponent<Card>().cardInfo.hpCost);
            }

        }

        return NPCPicksCard<GameObject>.GetWeightedRandom(cardList);
    }

    public static GameObject GetWeightedRandom(List<GameObject> inputs) 
    {
        inputs.OrderBy(key => key.GetComponent<Card>().cardInfo.hpCost);
        // Finished Building Dictionary

        int totalWeight = inputs.Sum(x => x.GetComponent<Card>().cardInfo.hpCost); // get the sum of the weights

        int selectedWeight = RANDOM.Next(totalWeight-1)+1; // makes counting start at 1, and get random number

        int temporaryVal = 0; // temporary value to keep track of the incerement

        for (int i = 0; i < inputs.Count; i++) {
            int testValue = inputs[i].GetComponent<Card>().cardInfo.hpCost; // this is the current value being checked in the list

            temporaryVal += testValue; // this is the current increment of the list


            if (selectedWeight <= temporaryVal) // if the selected weight, is less or equal to the current increment
            {
                //Debug.Log("NPC picked a card");
                return inputs[i]; // return the current element that is being looked at
                
                
            }
        }

        return inputs.First(); // returns the first element in the list
    }
}
