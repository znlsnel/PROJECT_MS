using UnityEngine;

public class StorageBox : Interactable
{
    [SerializeField] private GameObject storageBoxPrefab;

    private StorageUI storageBoxUI;
    private Storage storage = new Storage();
    void Awake() 
    {
        storageBoxUI = Instantiate(storageBoxPrefab).GetComponent<StorageUI>();
        storageBoxUI.Setup(storage);
        storageBoxUI.Hide(); 
    }

    public override void Interact(GameObject obj)
    {
        if (storageBoxUI.IsOpen)
            storageBoxUI.Hide(); 
        else
            storageBoxUI.Show();
    }
}

