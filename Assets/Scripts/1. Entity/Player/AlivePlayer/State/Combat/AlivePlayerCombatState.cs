using System.Xml;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class AlivePlayerCombatState : AlivePlayerState
{
    protected AlivePlayerCombatStateMachine combatStateMachine { get; private set; }

    public AlivePlayerCombatState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
        combatStateMachine = stateMachine.CombatStateMachine;
    }

    #region IState Methods
    public override void Enter()
    {
        Debug.Log($"CombatStateMachine : Enter {GetType().Name} state");

        base.Enter();

        stateMachine.Player.onDamaged += OnDamaged;
        stateMachine.Player.onDead += OnDead;
    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.Player.onDamaged -= OnDamaged;
        stateMachine.Player.onDead -= OnDead;
    }
    
    #endregion

    #region Main Methods
    private void OnDamaged()
    {
        StartAnimation(stateMachine.Player.AnimationData.DamagedParameterHash);

        CheckAnimation().Forget();
    }
    #endregion

    #region Reusable Methods
    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();

       // Managers.Input.GetInput(EPlayerInput.Fire).started += OnInputAttack;
        ItemHandler.onAction += OnInputAttack;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();

      //  Managers.Input.GetInput(EPlayerInput.Fire).started -= OnInputAttack;
        ItemHandler.onAction -= OnInputAttack;
    }

    protected override void OnDead()
    {
        StartAnimation(stateMachine.Player.AnimationData.DeadParameterHash);

        combatStateMachine.ChangeState(combatStateMachine.DeadState);
    }
    #endregion

    #region Input Methods
    protected virtual void OnInputAttack(ItemController itemController)
    {
        if(stateMachine.MovementStateMachine.currentState == stateMachine.MovementStateMachine.InterctingState)
            return;

        if (itemController is WeaponController)
        {
            combatStateMachine.ChangeState(combatStateMachine.AttackingState);
        }
    }
    #endregion

    async UniTaskVoid CheckAnimation()
    {
        while (true)
        {
            float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Damage", 3);

            if(normalizedTime >= 0.8f)
            {
                StopAnimation(stateMachine.Player.AnimationData.DamagedParameterHash);
                break;
            }

            await UniTask.Yield();
        }
    }
}
