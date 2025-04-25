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
    }

    private void Move()
    {
        stateMachine.Player.CharacterController.Move(stateMachine.ReusableData.VerticalVelocity * Time.deltaTime);

        if(stateMachine.ReusableData.MovementInput == Vector2.zero || stateMachine.ReusableData.MovementSpeedModifier == 0f)
        {
            return;
        }

        Vector3 movementDirection = GetMovementInputDirection();

        float targetRotationYAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;

        stateMachine.Player.transform.rotation = Quaternion.Slerp(stateMachine.Player.transform.rotation, Quaternion.Euler(0f, targetRotationYAngle, 0f), stateMachine.Player.AlivePlayerSO.RotationSpeed * Time.deltaTime);

        stateMachine.Player.CharacterController.Move(movementDirection * stateMachine.ReusableData.MovementSpeedModifier * Time.deltaTime);
    }
    #endregion

    #region Reusable Methods
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
    }

    protected virtual void AddInputActionCallbacks()
    {
        Managers.Input.GetInput(EPlayerInput.Move).canceled += OnMovementCanceled;
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        Managers.Input.GetInput(EPlayerInput.Move).canceled -= OnMovementCanceled;
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
            return nextInfo.normalizedTime;
        else if (!animator.IsInTransition(0) && nextInfo.IsTag(tag))
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
