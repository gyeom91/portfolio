using Newtonsoft.Json;
using UnityEngine;

public struct CharacterData : IKey<string>
{
    [JsonProperty("Key")] public string Key { get; private set; }
    [JsonProperty("Health")] public float Health { get; private set; }
    [JsonProperty("Damage")] public float Damage { get; private set; }
    [JsonProperty("Speed")] public float Speed { get; private set; }

    public bool TryGetPropertyValue(string name, out float value)
    {
        switch (name)
        {
            case "Health": value = Health; return true;
            case "Damage": value = Damage; return true;
            case "Speed": value = Speed; return true;
            default: value = 0; return false;
        }
    }
}
