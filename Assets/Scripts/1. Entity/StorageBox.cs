using UnityEngine;

public class StorageBox : Interactable
{
    [SerializeField] private GameObject storageBoxPrefab;

    private static StorageUI storageBoxUI;
    private Storage storage = new Storage();
    void Awake() 
    {
        if (storageBoxUI == null)
        {
            storageBoxUI = Instantiate(storageBoxPrefab).GetComponent<StorageUI>();
            storageBoxUI.Hide();  
        } 
    }

    public override void Interact(GameObject obj)
    {
        if (!storageBoxUI.IsOpen)
        {
            storageBoxUI.Setup(storage); 
            Managers.UI.ShowPopupUI<StorageUI>(storageBoxUI);
        } 
    }
}

