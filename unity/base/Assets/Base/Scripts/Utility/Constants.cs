using UnityEngine;

public class Constants
{
    #region :   Network

    public const string RELAY_CODE = "RELAY_CODE";

    #region :   Header

    public enum EHeader
    {
        NONE,
        LOAD_COMPLETED,
        EXP_INCREASE,
        LEVEL_UP_ENTER,
        LEVEL_UP_EXIT,
        ABILITY_SELECT,
    }

    #endregion

    #region :   Message

    public const string MESSAGE_DAMAGE = "MESSAGE_DAMAGE";

    #endregion

    #endregion

    #region :   State

    public const string SCENE_LOAD = "SCENE_LOAD";

    #region :   Network

    public const string NETWORK_HOST_OFFLINE = "NETWORK_HOST_OFFLINE";
    public const string NETWORK_HOST_ONLINE = "NETWORK_HOST_ONLINE";
    public const string NETWORK_CLIENT = "NETWORK_CLIENT";
    public const string NETWORK_LOBBY = "NETWORK_LOBBY";

    #endregion

    #region :   Login

    public const string LOGIN_LOADING = "LOGIN_LOADING";
    public const string LOGIN_SOCIAL_SIGN_IN = "LOGIN_SOCIAL_SIGN_IN";
    public const string LOGIN_GUEST_SIGN_IN = "LOGIN_GUEST_SIGN_IN";
    public const string LOGIN_UPDATE_PLAYER_NAME = "LOGIN_UPDATE_PLAYER_NAME";

    #endregion

    #region :   Lobby

    public const string LOBBY_MAIN = "LOBBY_MAIN";
    public const string LOBBY_SEARCH = "LOBBY_SEARCH";
    public const string LOBBY_STAY = "LOBBY_STAY";

    #endregion

    #region :   Session

    public const string SESSION_MAIN = "SESSION_MAIN";
    public const string SESSION_SEARCH = "SESSION_SEARCH";
    public const string SESSION_JOIN = "SESSION_JOIN";
    public const string SESSION_CREATE = "SESSION_CREATE";
    public const string SESSION_STAY = "SESSION_STAY";
    public const string SESSION_LEAVE = "SESSION_LEAVE";
    public const string SESSION_DELETE = "SESSION_DELETE";

    #endregion

    #region :   Battle

    public const string BATTLE_MAIN = "BATTLE_MAIN";
    public const string BATTLE_OPTION = "BATTLE_OPTION";

    #endregion

    #endregion

    #region :   Asset

    public const string DEFAULT_PATH = "ScriptableObjects/";
    public const string GAMEPLAY_ABILITY_SYSTEM_PATH = DEFAULT_PATH + "GameplayAbilitySystem/";
    //public const string GAMEPLAY_EFFECT_PATH = GAMEPLAY_ABILITY_SYSTEM_PATH + "GameplayEffect/";
    public const string FSM_PATH = DEFAULT_PATH + "FSM/";
    public const string BEHAVIOUR_TREE_PATH = DEFAULT_PATH + "Behaviour Tree/";
    public const string SKILL_EXECUTOR_PATH = DEFAULT_PATH + "Skill Executor/";
    public const string SKILL_SPAWN_EXECUTOR_PATH = SKILL_EXECUTOR_PATH + "Spawn/";
    public const string SKILL_MOVEMENT_EXECUTOR_PATH = SKILL_EXECUTOR_PATH + "Movement/";
    public const string SKILL_OFFENSIVE_EXECUTOR_PATH = SKILL_EXECUTOR_PATH + "Offensive/";
    public const string SKILL_DEFENSIVE_EXECUTOR_PATH = SKILL_EXECUTOR_PATH + "Defensive/";
    public const string SKILL_DESPAWN_EXECUTOR_PATH = SKILL_EXECUTOR_PATH + "Despawn/";
    public const string SKILL_DEATH_EXECUTOR_PATH = SKILL_EXECUTOR_PATH + "Death/";

    #endregion

    #region :   Battle

    public const int BATTLE_ABILITY_MAX_SLOT = 4;

    #endregion

    #region :   Event

    public enum EEvent
    {
        OnSpawn,
        OnMovement,
        OnOffensive,
        OnDefensive,
        OnDespawn,
        OnDeath,
    }

    #endregion

    #region :   Ability

    public enum EAbility
    {
        Status,
        Skill
    }

    public enum EType
    {
        Add,
        Multiple,
    }

    public enum ERank
    {
        None,
        Normal,
        Rare,
        Unique,
        Legend,
    }

    public enum EProperty
    {
        None,
        Speed,
        Damage,
        CoolTime,
    }

    #endregion
}
