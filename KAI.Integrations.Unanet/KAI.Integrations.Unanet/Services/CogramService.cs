using System;
using System.Collections.Generic;
using System.Text;

namespace KAI.Integrations.Unanet.Services
{
    public class CogramService : ICogramService
    {
        public Task<bool> TestConnectionAsync()
        {
            return Task.FromResult(true);
        }
    }
}
