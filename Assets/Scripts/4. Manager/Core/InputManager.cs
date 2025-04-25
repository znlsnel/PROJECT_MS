
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EPlayerInput
{
	Move,
	Look,
	Interact,
	Fire,
	Sprint,
	Aim,
	Test,
}

[System.Serializable]
public class InputManager : IManager
{
	[SerializeField] private InputActionAsset inputActionAsset; 
	private Dictionary<EPlayerInput, InputAction> playerInputs = new Dictionary<EPlayerInput, InputAction>();

	// Input Action Map   
	private InputActionMap _playerActionMap;
	 

	// === Input Properties ===
	public InputAction Move => playerInputs[EPlayerInput.Move];
	public InputAction Look => playerInputs[EPlayerInput.Look]; 
	public InputAction Interact => playerInputs[EPlayerInput.Interact]; 
	public InputAction Aim => playerInputs[EPlayerInput.Aim];
	public InputAction Fire => playerInputs[EPlayerInput.Fire]; 
	public InputAction Run => playerInputs[EPlayerInput.Sprint];   
	public InputAction Test => playerInputs[EPlayerInput.Test];   

	// === Input Actions ===
	public InputAction GetInput(EPlayerInput type) => playerInputs[type];   

    public void Init()
    {
		if (inputActionAsset == null)
			return;

        _playerActionMap = BindAction(typeof(EPlayerInput));
		inputActionAsset.Enable(); 
    }

    public void Clear()
    {
         
    }
 
	public void SetActive(bool active)
	{ 
		if (active)
			inputActionAsset.Enable();
		
		else 
			inputActionAsset.Disable(); 
	}
 
	private InputActionMap BindAction(Type type)
	{
		if (inputActionAsset == null)
			return null; 

		string mapName = type.Name;
		if (mapName[0] == 'E')
			mapName = mapName.Substring(1);

		var actionMap = inputActionAsset.FindActionMap(mapName);
		foreach (EPlayerInput t in Enum.GetValues(type)){ 
			string name = t.ToString(); 
			playerInputs[t] = actionMap.FindAction(name);
		}
 
		return actionMap;
	} 
}
