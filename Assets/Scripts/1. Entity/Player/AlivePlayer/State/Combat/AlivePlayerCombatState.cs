using Cysharp.Threading.Tasks;
using UnityEngine;

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

    private void OnDead()
    {
        StartAnimation(stateMachine.Player.AnimationData.DeadParameterHash);
    }
    #endregion

    #region Reusable Methods
    public void SetAttackAnimation(AnimationClip animationClip, float speed = 1f)
    {
        stateMachine.Player.overrideController["Attack"] = animationClip;
        stateMachine.Player.Animator.SetFloat("AttackSpeed", speed);
    }
    #endregion

    async UniTaskVoid CheckAnimation()
    {
        while (true)
        {
            float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Damage", 2);

            if(normalizedTime >= 0.8f)
            {
                StopAnimation(stateMachine.Player.AnimationData.DamagedParameterHash);
                break;
            }

            await UniTask.Yield();
        }
    }
}
