using UnityEngine;

public class CraftingTable : Interactable
{
    [SerializeField] private GameObject craftingTableUIPrefab;
    private static CraftingTableUI craftingTableUI;
    void Awake()
    {
        if (craftingTableUI == null)
        {
            craftingTableUI = Instantiate(craftingTableUIPrefab).GetComponent<CraftingTableUI>();
            craftingTableUI.Hide();
        }
    } 

    public override void Interact(GameObject obj)
    {
        if (craftingTableUI.IsOpen)
            return;
        Managers.UI.ShowPopupUI<CraftingTableUI>(craftingTableUI);
    }
}
