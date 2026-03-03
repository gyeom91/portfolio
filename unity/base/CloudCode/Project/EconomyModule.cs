using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;

using CloudCode.Response;

namespace CloudCode
{
    public class EconomyModule
    {
        private readonly IGameApiClient _gameApiClient;
        private readonly ILogger<EconomyModule> _logger;

        public EconomyModule(IGameApiClient gameApiClient, ILogger<EconomyModule> logger)
        {
            _gameApiClient = gameApiClient;
            _logger = logger;
        }

        [CloudCodeFunction("GetEconomies")]
        public async Task<List<EconomyResponse>> GetEconomies(IExecutionContext context, IGameApiClient gameApiClient)
        {
            List<EconomyResponse> economies = new List<EconomyResponse>();

            var response = await gameApiClient.EconomyCurrencies.GetPlayerCurrenciesAsync(context, context.AccessToken, context.ProjectId, context.PlayerId);
            foreach (var currency in response.Data.Results)
            {
                economies.Add(new EconomyResponse()
                {
                    ID = currency.CurrencyId,
                    Amount = currency.Balance
                });
            }

            return economies;
        }
    }
}
