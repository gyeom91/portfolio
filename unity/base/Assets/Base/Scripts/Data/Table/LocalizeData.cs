using Newtonsoft.Json;
using UnityEngine;

public struct LocalizeData : IKey<string>
{
    [JsonProperty("Key")] public string Key { get; private set; }
    [JsonProperty("English(en)")] public string English { get; private set; }
    [JsonProperty("Japanese(ja)")] public string Japanese { get; private set; }
    [JsonProperty("Korean(ko)")] public string Korean { get; private set; }
}
