using Unity.Services.RemoteConfig;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleStayAction", menuName = PATH + "Battle/BattleStayAction")]
public class BattleStayAction : FSMAction
{
    public override async Awaitable Stay()
    {
        var authenticationData = Get<AuthenticationData>();
        var heartbeatTime = RemoteConfigService.Instance.appConfig.GetFloat("HeartbeatTime");
        await LobbyManager.Instance.Heartbeat(authenticationData.PlayerID, heartbeatTime);
    }
}
