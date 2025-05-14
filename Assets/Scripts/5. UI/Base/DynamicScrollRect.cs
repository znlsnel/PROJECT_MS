using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DynamicScrollRect : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform slotRoot;
    [SerializeField] private GameObject slotPrefab;
    
    private GridLayoutGroup gridLayoutGroup;
    private float sellSize;
    private float spacing;
    private int slotBackIndex = 0;

    private float ContextMaxY;
    private float ContextMinY;

    private LinkedList<GameObject> slots = new LinkedList<GameObject>();

    private void Awake()
    {
        gridLayoutGroup = slotRoot.GetComponent<GridLayoutGroup>();
        sellSize = gridLayoutGroup.cellSize.y;
        spacing = gridLayoutGroup.spacing.y;

        slotBackIndex = slotRoot.childCount - 1;

        ContextMaxY = scrollRect.content.position.y + scrollRect.content.rect.height / 2;
        ContextMinY = scrollRect.content.position.y - scrollRect.content.rect.height / 2; 

        for (int i = 1; i <= 30; i++)
        {
            var slot = Instantiate(slotPrefab, slotRoot);
            slot.GetComponentInChildren<TextMeshProUGUI>().text = $"{i}";
            slots.AddLast(slot);
        }

        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    float testTime = 0;
    private void OnScroll(Vector2 value)
    {
        if (Time.time - testTime < 0.5f)
            return;

        float scrollPos = scrollRect.verticalNormalizedPosition;
        var (outCnt_up, outCnt_down) = CheckSlotPosition();

        int backNum = int.Parse(slots.Last.Value.GetComponentInChildren<TextMeshProUGUI>().text);
        for (int i = 0; i < outCnt_up; i++)
        {
            var slot = slots.First;
            slots.RemoveFirst();
            slots.AddLast(slot.Value);

            // 데이터 초기화
            slot.Value.GetComponentInChildren<TextMeshProUGUI>().text = $"{++backNum}";

            // 위치 뒤로 바꾸기
            slot.Value.transform.SetSiblingIndex(slotBackIndex);
        }

        int frontNum = int.Parse(slots.First.Value.GetComponentInChildren<TextMeshProUGUI>().text);
        for (int i = 0; i < outCnt_down; i++)
        {
            var slot = slots.Last;
            slots.RemoveLast();
            slots.AddFirst(slot.Value);
            
            // 데이터 초기화
            slot.Value.GetComponentInChildren<TextMeshProUGUI>().text = $"{--frontNum}";

            // 위치 앞으로 바꾸기
            slot.Value.transform.SetSiblingIndex(0);
        }

        // if (outCnt_up > 0)
        //     scrollRect.verticalNormalizedPosition = 1f;
        // else if (outCnt_down > 0)
        //     scrollRect.verticalNormalizedPosition = 0f; 


        if (outCnt_up > 0 || outCnt_down > 0)
        {
            testTime = Time.time;
            scrollRect.verticalNormalizedPosition = scrollPos; 
        }
    }

    private (int, int) CheckSlotPosition()
    {
        int outCnt_up = 0, outCnt_down = 0;

        foreach (var slot in slots)
        {
            float y = GetSlotHeight(slot.transform.position.y, true);
            if (y > ContextMaxY)
                outCnt_up++;
            else
                break;
        }

        foreach (var slot in slots.Reverse())
        {
            float y = GetSlotHeight(slot.transform.position.y);
            if (y < ContextMinY)
                outCnt_down++;
            else
                break;
        }

        return (outCnt_up, outCnt_down);
        
    }

    private float GetSlotHeight(float posY, bool down = false)
    {
        if (down)
            return posY - (sellSize  );
        return posY + (sellSize );
    }

}
