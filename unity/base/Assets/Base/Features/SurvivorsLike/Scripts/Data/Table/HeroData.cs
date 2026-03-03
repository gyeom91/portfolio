using Newtonsoft.Json;
using UnityEngine;

public struct HeroData : IKey<string>
{
    [JsonProperty("Key")] public string Key { get; private set; }
    [JsonProperty("ExpSensor")] public float ExpSensor { get; private set; }
    [JsonProperty("Level")] public int Level { get; private set; }
}
