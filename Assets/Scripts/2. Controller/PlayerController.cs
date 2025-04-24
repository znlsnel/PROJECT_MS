using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector2 _moveInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Managers.Input.SubscribeToInit(InitInput);
    }

    private void Update()
    {
        Move(_moveInput);
    }

    private void InitInput()
    {
        Managers.Input.Move.performed += InputMove;
        Managers.Input.Move.canceled += InputMove;
    }

    private void OnDestroy()
    {
        if (Managers.IsNull) 
            return; 

        Managers.Input.Move.performed -= InputMove;
        Managers.Input.Move.canceled -= InputMove;
    }

    public void InputMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>(); 
    }

    public void Move(Vector2 moveInput) 
    {
        _rigidbody.MovePosition(_rigidbody.position + new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime * 10); 
    }
}
