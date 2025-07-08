namespace Utils
{
// Wait.cs
// -----------------------------------------------------------------------------
// • GC dostu: Sadece ilk istekte nesne oluşturur, tekrar kullanır.
// • Thread-safe (C# static class initialization garantisi).
// -----------------------------------------------------------------------------
    using System.Collections.Generic;
    using UnityEngine;

    public static class Wait
    {
        // Sık kullanılan sabitler
        public static readonly WaitForSeconds Quarter = new(0.25f);
        public static readonly WaitForSeconds Half    = new(0.5f);
        public static readonly WaitForSeconds One     = new(1f);
        public static readonly WaitForSeconds Two     = new(2f);

        // İhtiyaca göre dinamik süreler
        private static readonly Dictionary<float, WaitForSeconds> _cache = new();

        /// <summary>
        /// Verilen saniye süresi için önbelleğe alınmış WaitForSeconds döndürür.
        /// </summary>
        public static WaitForSeconds Seconds(float seconds)
        {
            // Not: 0 veya negatif değerler için doğrudan null dönmek isteyebilirsiniz.
            if (!_cache.TryGetValue(seconds, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                _cache[seconds] = wait;
            }
            return wait;
        }

        /// <summary>
        /// Zaman ölçeğinden bağımsız bekleme (TimeScale == 0 iken de akar).
        /// </summary>
        private static readonly Dictionary<float, WaitForSecondsRealtime> _rtCache = new();

        public static WaitForSecondsRealtime Realtime(float seconds)
        {
            if (!_rtCache.TryGetValue(seconds, out var wait))
            {
                wait = new WaitForSecondsRealtime(seconds);
                _rtCache[seconds] = wait;
            }
            return wait;
        }
    }

}