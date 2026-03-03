using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class CharacterTable : Table<CharacterData, string>
{
    public override IReadOnlyList<CharacterData> Datas => _characterDatas;

    [JsonProperty("CharacterData")] private List<CharacterData> _characterDatas { get; set; }
}
