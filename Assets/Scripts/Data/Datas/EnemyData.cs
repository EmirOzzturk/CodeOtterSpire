using System;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    /*──────────────── INTENT → EFFECT ────────────────*/
    [Serializable]
    public sealed class IntentEffectPair
    {
        [Tooltip("Gösterilecek Enemy Intent’i")]
        public IntentType intentType;

        [Tooltip("Bu turda çalıştırılacak efekt(ler)")]
        [field: SerializeReference, SR]
        public List<EnemyEffect> enemyEffects { get; private set; } = new();
    }

    /*────────────────── GÖRSEL ───────────────────────*/
    [field: Header("Görsel"), SerializeField]
    public Sprite Image { get; private set; }

    /*──────────────── İSTATİSTİKLER ─────────────────*/
    [field: Header("İstatistikler"), SerializeField, Min(1)]
    public int Health { get; private set; } = 1;

    [field: SerializeField, Min(0)]
    public int AttackPower { get; private set; }

    /*────────── INTENT & EFFECT HARİTASI ────────────*/
    [Header("Intent → Effect Eşlemesi")]
    [field: SerializeReference, SR]
    [Tooltip("Tur sırasına göre intent + efekt listesi")]
    public List<IntentEffectPair> IntentEffectMap { get; private set; }

    /*────────── BAŞLANGIÇ STATUS EFFECT ────────────*/
    [Header("Başlangıçta Eklenecek Status Effect")]
    [field: SerializeReference, SR]
    public List<EnemyEffect> InitialEffects { get; private set; }
    
/*────────────────── API ─────────────────────────*/
    /// <summary>IntentEffectMap içindeki TÜM intent’leri sıralı olarak verir.</summary>
    public List<IntentType> GetAllIntents() =>
        IntentEffectMap?.Select(pair => pair.intentType).ToList()
        ?? new List<IntentType>();

    /// <summary>
    /// IntentEffectMap’teki her Effect listesini sıralı şekilde döner.
    /// Dış liste -> intent sırası
    /// İç liste  -> o intent’e ait effect’ler
    /// </summary>
    public List<List<EnemyEffect>> GetEffectGroups()
    {
        // Null veya boş harita durumunda: boş dış liste
        if (IntentEffectMap == null || IntentEffectMap.Count == 0)
            return new List<List<EnemyEffect>>();

        /*
           LINQ ile:
           - Her pair’in enemyEffects listesini al
           - Null ise boş liste yarat
           - Kendi kopyasını oluştur (dışarıdan değiştirilse bile özgün veri bozulmaz)
           - Hepsini ToList() ile dış listeye doldur
        */
        return IntentEffectMap
            .Select(pair => new List<EnemyEffect>(
                pair.enemyEffects ?? Enumerable.Empty<EnemyEffect>()))
            .ToList();
    }

    /*──────────────── HELPERS ───────────────────────*/
    private bool IsValid(int idx) => idx >= 0 && idx < IntentEffectMap.Count;
}
