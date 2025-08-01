using UnityEngine;

[CreateAssetMenu(fileName = "Cocktail", menuName = "Cocktails/Cocktail")]
public class Cocktail : ScriptableObject
{
    public Sprite sprite;
    public Resource[] Recipe;
}
