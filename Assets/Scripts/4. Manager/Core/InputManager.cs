using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EPlayerInput
{
	Move,
	Look,
	Interact,
	LeftMouse,
	RightMouse,
	Sprint,
	Aim,
	Test,
	Inventory,
	TurnOffPopup
}

public enum EQuickSlot
{
	Select1,
	Select2,
	Select3,
	Select4,
	Select5,
	Select6,
}

[System.Serializable]
public class InputManager : IManager
{
	[SerializeField] private InputActionAsset inputActionAsset; 

	private Dictionary<EPlayerInput, InputAction> playerInputs = new Dictionary<EPlayerInput, InputAction>();
	private Dictionary<EQuickSlot, InputAction> quickSlotInputs = new Dictionary<EQuickSlot, InputAction>();


	// Input Action Map   
	private InputActionMap _playerActionMap;
	private InputActionMap _quickSlotActionMap;


	// === Input Properties ===
	public InputAction Move => playerInputs[EPlayerInput.Move];
	public InputAction Look => playerInputs[EPlayerInput.Look]; 
	public InputAction Interact => playerInputs[EPlayerInput.Interact]; 
	public InputAction Aim => playerInputs[EPlayerInput.Aim];
	public InputAction LeftMouse => playerInputs[EPlayerInput.LeftMouse]; 
	public InputAction RightMouse => playerInputs[EPlayerInput.RightMouse];
	public InputAction Run => playerInputs[EPlayerInput.Sprint];   
	public InputAction Test => playerInputs[EPlayerInput.Test];   


	// === Input Actions ===
	public InputAction GetInput(EPlayerInput type) => playerInputs[type];   
	public InputAction GetInput(EQuickSlot type) => quickSlotInputs[type]; 

    public void Init()
    {
		if (inputActionAsset == null)
			return;

        _playerActionMap = BindAction<EPlayerInput>(typeof(EPlayerInput), out playerInputs);
		_quickSlotActionMap = BindAction<EQuickSlot>(typeof(EQuickSlot), out quickSlotInputs); 
		inputActionAsset.Enable(); 
    } 

    public void Clear()
    { 
         
    }
 
	private InputActionMap BindAction<T>(Type type, out Dictionary<T, InputAction> playerInputs) where T : Enum
	{
		playerInputs = new Dictionary<T, InputAction>();
		if (inputActionAsset == null)
			return null; 

		string mapName = type.Name;
		if (mapName[0] == 'E')
			mapName = mapName.Substring(1);

		var actionMap = inputActionAsset.FindActionMap(mapName);
		foreach (var t in Enum.GetValues(typeof(T)))
		{ 
			string name = t.ToString(); 
			playerInputs[(T)t] = actionMap.FindAction(name);  
		} 
 
		return actionMap;
	} 
}
