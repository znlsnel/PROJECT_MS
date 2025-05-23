using Cysharp.Threading.Tasks;
using UnityEngine;


public class GhostPlayerState : IState
{
    protected GhostPlayerStateMachine stateMachine;
    public GhostPlayerState(GhostPlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    #region IState Methods
    public virtual void Enter()
    {
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
    protected float GetNormalizedTime(Animator animator, string tag, int layerIndex = 0)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(layerIndex);
        if (animator.IsInTransition(layerIndex) && nextInfo.IsTag(tag))
            return nextInfo.normalizedTime;
        else if (!animator.IsInTransition(layerIndex) && currentInfo.IsTag(tag))
            return currentInfo.normalizedTime;
        else
            return 0f;
    }
    #endregion
}

