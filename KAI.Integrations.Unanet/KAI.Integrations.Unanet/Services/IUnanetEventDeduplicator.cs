using System;
using System.Collections.Generic;
using System.Text;

using KAI.Integrations.Unanet.Models;

namespace KAI.Integrations.Unanet.Services
{
    public interface IUnanetEventDeduplicator
    {
        bool TryAccept(UnanetRouteEventType routeEventType, UnanetEvent unanetEvent);
    }
}
