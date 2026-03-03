using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class HeroTable : Table<HeroData, string>
{
    public override IReadOnlyList<HeroData> Datas => _heroDatas;

    [JsonProperty("HeroData")] private List<HeroData> _heroDatas { get; set; }
}
