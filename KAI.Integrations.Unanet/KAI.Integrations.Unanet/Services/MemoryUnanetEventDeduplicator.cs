using System;
using System.Collections.Generic;
using System.Text;
using KAI.Integrations.Unanet.Models;
using Microsoft.Extensions.Caching.Memory;

namespace KAI.Integrations.Unanet.Services
{
    public sealed class MemoryUnanetEventDeduplicator : IUnanetEventDeduplicator
    {
        private static readonly TimeSpan DuplicateWindow = TimeSpan.FromSeconds(3);

        private readonly IMemoryCache _cache;

        public MemoryUnanetEventDeduplicator(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool TryAccept(UnanetRouteEventType routeEventType, UnanetEvent unanetEvent)
        {
            string key = string.Join(":", "Unanet", 
                unanetEvent.Database?.Trim().ToLowerInvariant() ?? "unknown", 
                routeEventType.ToString().ToLowerInvariant(), 
                unanetEvent.KeyId);
 
            if (_cache.TryGetValue(key, out _))
            {
                return false;
            }

            _cache.Set(key, true, DuplicateWindow);
 
            return true;
        }
    }
}
