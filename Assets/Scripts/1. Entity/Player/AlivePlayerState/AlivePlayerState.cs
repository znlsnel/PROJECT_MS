using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AlivePlayerState : IState
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
        ReadMovementInput();
        Move();
    }
    #endregion

    #region Main Methods
    private void ReadMovementInput()
    {
        stateMachine.ReusableData.MovementInput = Managers.Input.GetInput(EPlayerInput.Move).ReadValue<Vector2>();
        stateMachine.ReusableData.ShouldSprint = Managers.Input.GetInput(EPlayerInput.Sprint).IsPressed();
    }

    private void Move()
    {
        stateMachine.Player.CharacterController.Move(stateMachine.ReusableData.VerticalVelocity * Time.deltaTime);

        if(stateMachine.ReusableData.MovementInput == Vector2.zero || stateMachine.ReusableData.MovementSpeedModifier == 0f)
        {
            return;
        }

        Vector3 movementDirection = GetMovementInputDirection();

        RotationPlayer(movementDirection);

        stateMachine.Player.CharacterController.Move(movementDirection * stateMachine.ReusableData.MovementSpeedModifier * Time.deltaTime);
    }
    #endregion

    #region Reusable Methods
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }

    public void SetInteractAnimation(AnimationClip animationClip, float speed = 1f)
    {
        stateMachine.Player.overrideController["Interaction"] = animationClip;
        stateMachine.Player.Animator.SetFloat("InteractingSpeed", speed);
    }

    public void SetAttackAnimation(AnimationClip animationClip, float speed = 1f)
    {
        stateMachine.Player.overrideController["Attack"] = animationClip;
        stateMachine.Player.Animator.SetFloat("AttackSpeed", speed);
    }

    protected virtual void AddInputActionCallbacks()
    {
        Managers.Input.GetInput(EPlayerInput.Move).canceled += OnMovementCanceled;
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        Managers.Input.GetInput(EPlayerInput.Move).canceled -= OnMovementCanceled;
    }

    protected void RotationPlayer(Vector3 direction)
    {
        float targetRotationYAngle = GetRotationAngle(direction);

        stateMachine.Player.transform.rotation = Quaternion.Slerp(stateMachine.Player.transform.rotation, Quaternion.Euler(0f, targetRotationYAngle, 0f), stateMachine.Player.AlivePlayerSO.RotationSpeed * Time.deltaTime);
    }

    protected float GetRotationAngle(Vector3 direction)
    {
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
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

    #region Input Methods
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }
    #endregion
}
