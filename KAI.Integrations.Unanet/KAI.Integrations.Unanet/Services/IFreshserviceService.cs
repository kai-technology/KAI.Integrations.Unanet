using System;
using System.Collections.Generic;
using System.Text;

namespace KAI.Integrations.Unanet.Services
{
    public interface IFreshserviceService
    {
        Task<bool> TestConnectionAsync();
    }
}
