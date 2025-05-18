using UnityEngine;

public class BuildingItemController : ItemController
{
    public PlacementCheck PlacementCheck {get; private set;}

    public override void Setup(AlivePlayer owner, ItemSlot itemSlot)
    {
        base.Setup(owner, itemSlot);
        PlacementCheck = new PlacementCheck(itemData);
    }

    public override void OnAction()
    {
        // 건축 모드 시작
        
    }

    public void OnPlacementComplete()
    {
        // 건축 모드 종료
        itemSlot.AddStack(itemData, -1);
    }
}
