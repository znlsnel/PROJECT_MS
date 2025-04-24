using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListWindow : UIBase
{
    [SerializeField] private Transform listContent;
    [SerializeField] private GameObject listItemPrefab;

    public T AddItem<T>() where T : MonoBehaviour
    {
        GameObject itemObject = Instantiate(listItemPrefab, listContent);
        return itemObject.GetOrAddComponent<T>();
    }

    public void RemoveItem(MonoBehaviour item)
    {
        Destroy(item.gameObject);
    }
}
