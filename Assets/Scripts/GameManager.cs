using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> deck = new List<GameObject>();
    public Transform[] playerSlots;
    public bool[] emptySlots;
}
