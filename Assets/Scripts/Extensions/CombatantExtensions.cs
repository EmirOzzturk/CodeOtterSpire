// CombatantExtensions.cs
using System.Collections.Generic;
using System.Linq;

public static class CombatantExtensions
{
    /// <summary>
    /// IEnumerable üzerinde tahsissiz (GC-siz) up-cast sağlar.
    /// </summary>
    /// <remarks>
    /// Covariance sayesinde <c>IEnumerable&lt;TDerived&gt;</c> doğrudan
    /// <c>IEnumerable&lt;TBase&gt;</c> olarak kullanılabilir; <c>Select</c> veya
    /// <c>Cast&lt;T&gt;</c> tahsisi gerekmez.
    /// </remarks>
    public static IEnumerable<CombatantView> AsCombatants<T>(this IEnumerable<T> src)
        where T : CombatantView
        => src;                      // sıfır kopya, sıfır GC

    /// <summary>
    /// Listeye ihtiyaç varsa: tek kopya, tek tahsis.
    /// </summary>
    public static List<CombatantView> ToCombatantList<T>(this IEnumerable<T> src)
        where T : CombatantView
        => src is List<CombatantView> ready
            ? ready                    // zaten List<CombatantView> ise yeniden yapmaya gerek yok
            : src.Select(c => (CombatantView)c).ToList();
}