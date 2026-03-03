using CycloneGames.GameplayAbilities.Runtime;
using Unity.Netcode;
using UnityEngine;

public class ArrowAbility : GameplayAbility
{
    private string _arrowPrefabName;
    private Vector3 _positionOffset;

    public ArrowAbility(string prefabName, Vector3 positionOffset)
    {
        _arrowPrefabName = prefabName;
        _positionOffset = positionOffset;
    }

    public override GameplayAbility CreatePoolableInstance()
    {
        var ability = new ArrowAbility(_arrowPrefabName, _positionOffset);
        ability.Initialize(
            this.Name,
            this.InstancingPolicy,
            this.NetExecutionPolicy,
            this.CostEffectDefinition,
            this.CooldownEffectDefinition,
            this.AbilityTags,
            this.ActivationBlockedTags,
            this.ActivationRequiredTags,
            this.CancelAbilitiesWithTag,
            this.BlockAbilitiesWithTag
        );



        return ability;
    }

    public override void ActivateAbility(GameplayAbilityActorInfo actorInfo, GameplayAbilitySpec spec, GameplayAbilityActivationInfo activationInfo)
    {
        base.ActivateAbility(actorInfo, spec, activationInfo);

        var sceneController = SceneController.Instance;
        if (sceneController.IsNull())
        {
            EndAbility();
            return;
        }

        if (actorInfo.OwnerActor is BattleCharacter character == false || character.TryGetComponent<NetworkObject>(out var networkObject) == false)
        {
            EndAbility();
            return;
        }

        var networkPoolService = sceneController.GetService<NetworkPoolService>();
        var arrowSyncData = new ArrowSyncData();
        arrowSyncData.NetworkID = networkObject.NetworkObjectId;

        var asc = character.ASC;
        var skillCount = asc.GetAttribute(SurvivorsLikeGameplayTagContainer.Attribute_Skill_Count).CurrentValue;
        var charTransform = character.transform;
        var charForward = charTransform.forward;
        var angleOffset = 360f / skillCount;
        for (var i = 0; i < skillCount; ++i)
        {
            var angle = angleOffset * i;
            var axis = Quaternion.AngleAxis(angle, Vector3.up);
            var direction = axis * charForward;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            var spawnPos = charTransform.position + _positionOffset;
            var arrowNetworkObject = networkPoolService.HandlerSpawn("Arrow", arrowSyncData, spawnPos, rotation);
            if (arrowNetworkObject.TryGetComponent<Arrow>(out var arrow))
            {
                arrow.OnHit += EndAbility;
            }
            else
            {
                networkObject.Despawn();

                EndAbility();
            }
        }
    }
}
