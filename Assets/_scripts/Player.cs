
using UnityEngine;
using Unity.Netcode;
using BonBon;
using Cinemachine;
using UnityEngine.UIElements;

public class Player : NetworkBehaviour
{
    [SerializeField] private PlayerStaminaBar _staminaBar;
    [SerializeField] private PlayerNickName _nick;
    [SerializeField] private CinemachineVirtualCamera _camera;
    private PlayerConstraintsCfg _constraints;
    [SerializeField] private CharacterMovement _movement;
    private IPlayerAnimatorsStorage _animatorsStorage;

    public void SetConstraints(PlayerConstraintsCfg constraints)
    {
        Debug.Log("Constraints Set");
        _constraints = constraints;
    }
    public void SetAnimator(CharacterAnimator animator, PlayerConstraintsCfg constraintsCfg)
    {
        var constraints = new PlayerConstraints(constraintsCfg);
        transform.localScale = new Vector3(constraints._scaleX, constraints._scaleY, 1);
        _movement.SetUp(animator,_staminaBar, constraints);
        animator.transform.SetParent(transform);
    }
    public void SetAnimator(CharacterAnimator animator, PlayerConstraints constraints)
    {
        transform.localScale = new Vector3(constraints._scaleX, constraints._scaleY, 1);
        _movement.SetUp(animator,_staminaBar, constraints);
        animator.transform.SetParent(transform);
    }
    public void SetAnimatorsStorage(IPlayerAnimatorsStorage animatorsStorage)
    {
        _animatorsStorage = animatorsStorage;
    }

    public PlayerData GetLocalPlayerData()
    {
        return PlayerPrefsManager.GetLocalPlayerData();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner is false)
        {
            Debug.Log($"{OwnerClientId} - OnNetworkSpawn");
            Destroy(_camera.gameObject);
            return;
        }

        Debug.Log($"{OwnerClientId} - OnNetworkSpawn");

        var localplayerData = GetLocalPlayerData();
        string colId = localplayerData._colId;
        string name = localplayerData._name;

        
        SpawnPlayerBodyServerRpc(colId,name, OwnerClientId);
        base.OnNetworkSpawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerBodyServerRpc(string colId,string name, ulong client)
    {
        _nick.SetName(name);

        var animatorObj = _animatorsStorage.GetAnimator(colId);
        var animator = GameObject.Instantiate(animatorObj).GetComponent<CharacterAnimator>();

        animator.name = $"{client} - {colId}";
        animator.GetComponent<NetworkObject>().SpawnWithOwnership(client);
        SetAnimator(animator, _constraints);

        Debug.Log($"{OwnerClientId} - SERVER RPC");
        SpawnPlayerBodyClientRpc(client,name, new PlayerConstraints(_constraints));
    }


    [ClientRpc]
    public void SpawnPlayerBodyClientRpc(ulong client,string name, PlayerConstraints constraints)
    {
        Debug.Log(gameObject.name);
        if (IsServer || IsHost)
            return;

        if (OwnerClientId != client) return;

        _nick.SetName(name);

        Debug.Log($"Scale X: {constraints._scaleX},  MoveSpeed: {constraints._moveSpeed}");

        CharacterAnimator animator = GetComponentInChildren<CharacterAnimator>();

        Debug.Log($"{OwnerClientId} - SpawnPlayerBodyClientRpc");
        SetAnimator(animator, constraints);
    }
}
