using UnityEngine;

public class AlivePlayerCombatState : AlivePlayerState
{
    public AlivePlayerCombatState(AlivePlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods

    #endregion

    #region Reusable Methods
    public void SetAttackAnimation(AnimationClip animationClip, float speed = 1f)
    {
        stateMachine.Player.overrideController["Attack"] = animationClip;
        stateMachine.Player.Animator.SetFloat("AttackSpeed", speed);
    }
    #endregion
}
