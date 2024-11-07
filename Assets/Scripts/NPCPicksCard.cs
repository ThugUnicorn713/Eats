using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

using Random = System.Random;

public class NPCPicksCard<T> : MonoBehaviour {

    private static Random RANDOM = new Random();

    public static GameObject testMethod(CPUManager manager) {

        List<int> weightList = new List<int>();
        List<GameObject> cardList = new List<GameObject>();

        foreach (var item in manager.cpuHand) {
            weightList.Add(item.hpCost);
        }

        foreach (var item in manager.cpuHandPositions) {
            cardList.Add(item.GetChild(1).gameObject);
            //                        ^^^ If you have a sphere, its 1, if you delete the object, make this zero

        }

        return NPCPicksCard<GameObject>.GetWeightedRandom(cardList, weightList);
    }

    public static T GetWeightedRandom(List<T> inputs, List<int> weights) {
        // Create Dictionary
        SortedDictionary<int, T> test = new SortedDictionary<int, T>();
        
        for (int i = 0; i < inputs.Count; i++) {
            test.Add(weights[i], inputs[i]);
        }

        test.OrderBy(key => key);
        // Finished Building Dictionary

        int totalWeight = weights.Sum(x => x); // get the sum of the weights

        int selectedWeight = RANDOM.Next(totalWeight-1)+1; // makes counting start at 1, and get random number

        int temporaryVal = 0; // temporary value to keep track of the incerement

        for (int i = 0; i < inputs.Count; i++) {
            int testValue = weights[i]; // this is the current value being checked in the list

            temporaryVal += testValue; // this is the current increment of the list


            if (selectedWeight <= temporaryVal) // if the selected weight, is less or equal to the current increment
            {
                return inputs[i]; // return the current element that is being looked at
                
                Debug.Log("NPC picked a card");
            }
        }

        return inputs.First(); // returns the first element in the list
    }
}
