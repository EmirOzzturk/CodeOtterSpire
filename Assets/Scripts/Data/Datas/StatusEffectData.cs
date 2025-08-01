using UnityEngine;

[CreateAssetMenu(fileName = "Status Effect", menuName = "Data/StatusEffect")]
public class StatusEffectData : ScriptableObject
{
    [field: SerializeField] 
    public StatusEffectType StatusEffectType { get; private set; }
    
    [field: SerializeField, Tooltip("Additive: Gelen etki toplanır. Azalma olmaz.\nCounter: Bir koşul gerçekleşince azalır.\nDuration:Tur geçince azalır.\nNone: Azalmaz.")] 
    public StackingBehaviour StackingBehaviour { get; private set; }
    
    [field: SerializeField] 
    public Sprite StatusEffectSprite { get; private set; }
    
}
