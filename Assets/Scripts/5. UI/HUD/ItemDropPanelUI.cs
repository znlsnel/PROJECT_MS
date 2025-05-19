using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropPanelUI : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ItemDragHandler.DropItem();
    }
}
