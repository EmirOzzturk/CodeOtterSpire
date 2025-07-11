using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    /*────────── GÖRSEL ──────────*/
    [field: Header("Görsel"), SerializeField]
    public Sprite Image { get; private set; }

    /*────────── İSTATİSTİKLER ──────────*/
    [field: Header("İstatistikler"), SerializeField]
    public int Health { get; private set; }

    [field: SerializeField]
    public int AttackPower { get; private set; }

    /*────────── NİYET (INTENT) ──────────*/
    [field: Header("Niyet"), SerializeField]
    public IntentType IntentType { get; private set; }

    /*────────── EFEKTLER ──────────*/
    [field: Header("Efektler"), SerializeReference, SR]
    public List<Effect> EnemyEffect { get; private set; }
}