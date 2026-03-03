using Newtonsoft.Json;
using System.Collections.Generic;

public class LocalizeTable : Table<LocalizeData, string>
{
    public override IReadOnlyList<LocalizeData> Datas => _localizeDatas;

    [JsonProperty("LocalizeData")] private List<LocalizeData> _localizeDatas { get; set; }
}
