using UnityEngine;

[CreateAssetMenu(menuName = "Item/Potion Data")]
public class PotionData : ScriptableObject
{
    public float healAmount = 50.0f;
    public AudioClip useSound;
}