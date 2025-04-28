using UnityEngine;

public class AlivePlayerState : IState
{
    protected AlivePlayerStateMachine stateMachine;

    public AlivePlayerState(AlivePlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    #region IState Methods
    public virtual void Enter()
    {
        Debug.Log($"Enter {GetType().Name} state");

        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }

    public virtual void FixedUpdate()
    {
        
    }

    public virtual void Update()
    {

    }
    #endregion

    #region Reusable Methods
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }

    protected virtual void AddInputActionCallbacks()
    {
        
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
            return nextInfo.normalizedTime;
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
            return currentInfo.normalizedTime;
        else
            return 0f;
    }
    #endregion
}
