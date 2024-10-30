using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string CardName;
    public int value;
    public bool hasSpecial;
   
}
