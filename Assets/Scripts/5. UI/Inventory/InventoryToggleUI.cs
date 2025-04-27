using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class InventoryToggleUI : MonoBehaviour
{
    [SerializeField] private RectTransform CheckImage;
    [SerializeField] private Toggle allButton;
    [SerializeField] private Toggle weaponButton;
    [SerializeField] private Toggle consumableButton;
    [SerializeField] private Toggle resourceButton;
    [SerializeField] private Toggle equippableButton;
    [SerializeField] private Toggle placeableButton;


    [SerializeField] private InventoryUI inventoryUI;

    private void Awake()
    {
        SetToggle();
    } 
    
    private void SetToggle()
    {
        allButton.onValueChanged.AddListener((isOn) => SetFilter(isOn, EItemType.None, allButton.transform));
        weaponButton.onValueChanged.AddListener((isOn) => SetFilter(isOn, EItemType.Weapon, weaponButton.transform));
        consumableButton.onValueChanged.AddListener((isOn) => SetFilter(isOn, EItemType.Consumable, consumableButton.transform));
        resourceButton.onValueChanged.AddListener((isOn) => SetFilter(isOn, EItemType.Resource, resourceButton.transform));
        equippableButton.onValueChanged.AddListener((isOn) => SetFilter(isOn, EItemType.Equippable, equippableButton.transform));
        placeableButton.onValueChanged.AddListener((isOn) => SetFilter(isOn, EItemType.Placeable, placeableButton.transform));
    }

    private void SetFilter(bool isOn, EItemType itemType, Transform target)
    {
        inventoryUI.FilterInventoryByType(itemType);
        Vector3 position = CheckImage.parent.InverseTransformPoint(target.position);
        CheckImage.DOLocalMove(position, 0.5f); 
    }

    // private IEnumerator MoveToCheckImage(Transform target)
    // {
        
    //     yield return null;
    // }

}
