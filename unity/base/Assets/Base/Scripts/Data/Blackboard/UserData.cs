using System.Collections.Generic;
using Unity.Services.CloudCode.GeneratedBindings.CloudCode.Response;
using UnityEngine;

public class UserData
{
    public IReadOnlyList<EconomyResponse> Economies { get; private set; }

    public void SetEconomies(List<EconomyResponse> economies)
    {
        Economies = economies;
    }
}
