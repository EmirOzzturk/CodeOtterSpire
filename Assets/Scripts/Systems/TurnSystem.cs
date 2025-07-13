using System;
using UnityEngine;

/// <summary>Aktif turun kimde olduğunu tutar ve değiştiğinde olayı tetikler.</summary>
public class TurnSystem : Singleton<TurnSystem>
{
    public enum Turn { Player, Enemy }
    public Turn CurrentTurn { get; private set; } = Turn.Player;   // Oyun oyuncuyla başlasın
    public int TurnNumber { get; private set; } = 1;
    
    public event Action<Turn> OnTurnChanged;
    

    /// <summary>Bir aksiyon tamamlandığında çağır; turu karşı tarafa geçirsin.</summary>
    public void NextTurn()
    {
        CurrentTurn = (CurrentTurn == Turn.Player) ? Turn.Enemy : Turn.Player;
        if (CurrentTurn == Turn.Player) TurnNumber++;
        OnTurnChanged?.Invoke(CurrentTurn);
    }
    
    public int GetPostTurnRemainder(int mod)
    {
        // Hatalı parametre: mod ≤ 0 ise –1 döndür (veya kendi hata yönetiminiz)
        if (mod <= 0)
        {
            Debug.LogError($"GetTurnRemainder → mod ({mod}) pozitif olmalı!");
            return -1;
        }

        int remainder = ((TurnNumber - 1) % mod + mod) % mod;
        return remainder;
    }

}