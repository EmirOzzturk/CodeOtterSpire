using System;
using UnityEngine;

public class StatusEffect
{
    public readonly StatusEffectData StatusEffectData;

    public StatusEffectType StatusEffectType => StatusEffectData.StatusEffectType;
    public StackingBehaviour StackingBehaviour => StatusEffectData.StackingBehaviour;
    public Sprite Sprite => StatusEffectData.StatusEffectSprite;
    
    public int Stack { get; private set; }

    public StatusEffect(StatusEffectData statusEffectData, int stack)
    {
        StatusEffectData = statusEffectData;
        Stack = stack;
    }
    
    /// <summary>
    /// Aynı türde iki status effect birleştirildiğinde stack’i nasıl hesaplayacağımızı belirler.
    /// </summary>
    /// <param name="incoming">Dışarıdan gelen (eklenmek istenen) etki.</param>
    public void Merge(StatusEffect incoming)
    {
        if (incoming.StatusEffectType != StatusEffectType)
            throw new ArgumentException("StatusEffect türleri uyuşmuyor.");

        switch (StackingBehaviour)
        {
            case StackingBehaviour.Additive:
            case StackingBehaviour.Counter:
            case StackingBehaviour.Duration:
                // Her eklemede yığının üzerine ekle.
                Stack += incoming.Stack;
                break;

            case StackingBehaviour.None:
                Stack = 1;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Var olan etkiye miktar ekler. StackingBehaviour’e göre nasıl davranacağını
    /// merkezi olarak burada tanımlar.
    /// </summary>
    /// <param name="amount">Eklenmek istenen stack miktarı.</param>
    public void AddStack(int amount)
    {
        if (amount <= 0) return; // Negatif ya da sıfır ekleme anlamsız

        switch (StackingBehaviour)
        {
            case StackingBehaviour.Additive:
            case StackingBehaviour.Counter:
            case StackingBehaviour.Duration:
                // Üç durumda da doğrudan toplarız.
                Stack += amount;
                break;
            
            case StackingBehaviour.None:
                Stack = 1;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Var olan stack’i azaltır. Stack 0’ın altına düşmez.
    /// </summary>
    /// <param name="amount">Azaltılmak istenen miktar.</param>
    /// <returns>Etki tamamen bitti mi (stack 0 oldu mu)?</returns>
    public bool ReduceStack(int amount)
    {
        if (amount <= 0) return false;

        switch (StackingBehaviour)
        {
            case StackingBehaviour.Additive:
            case StackingBehaviour.Counter:
            case StackingBehaviour.Duration:
                Stack -= amount;
                if (Stack < 0) Stack = 0;
                break;

            case StackingBehaviour.None:
                Stack = 0;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        return Stack == 0;
    }


    /// <summary>
    /// Örn. her tur sonunda çağrılacak azaltım mantığı.
    /// </summary>
    public void TickEndOfTurn()
    {
        if (StackingBehaviour == StackingBehaviour.Duration && Stack > 0)
            Stack--;
    }
}