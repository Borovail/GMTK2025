using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Cocktails/CustomerData")]
public class CustomerData : ScriptableObject
{
    public Sprite Sprite;
    public Cocktail Cocktail;
}