
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VInspector.Libs;

public class DynamicScrollRect : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform slotRoot;
    [SerializeField] private GameObject slotPrefab;
    
    private GridLayoutGroup gridLayoutGroup;

    private float cellSize;
    private float spacing;
    private int slotBackIndex = 0;

    private float ContentMaxY;
    private float ContentMinY;
    private float secondStartPos;
    private int slotVerticalCnt;


    private LinkedList<GameObject> slots = new LinkedList<GameObject>();

    private void Awake()
    {
        gridLayoutGroup = slotRoot.GetComponent<GridLayoutGroup>();

        cellSize = gridLayoutGroup.cellSize.y;

        spacing = gridLayoutGroup.spacing.y;

        slotBackIndex = slotRoot.childCount - 1;

        InitScrollRect();
    }

    private void InitScrollRect()
    {
        ContentMaxY = scrollRect.content.position.y + scrollRect.content.rect.height / 2;
        ContentMinY = scrollRect.content.position.y - scrollRect.content.rect.height / 2; 

        slotVerticalCnt = (int)((scrollRect.content.rect.height) / cellSize) + 2; 
        // if ((ContentMinY - ContentMaxY) % cellSize != 0)
        //     slotVerticalCnt++;

        for (int i = 1; i <= slotVerticalCnt * gridLayoutGroup.constraintCount; i++)
        {
            var slot = Instantiate(slotPrefab, slotRoot);
            slot.GetComponentInChildren<TextMeshProUGUI>().text = $"{i}";
            slots.AddLast(slot);
        }

        scrollRect.onValueChanged.AddListener(OnScroll);

        secondStartPos = 1f - ((cellSize + spacing / 2) * slotVerticalCnt) / (cellSize + spacing / 2);
    }

    float prevScrollPos = 1;
    float interval = 0;
    private void OnScroll(Vector2 value)
    {
        

        var outCnt = CheckSlotPosition();

        if (outCnt > 0)
        {
            int backNum = int.Parse(slots.Last.Value.GetComponentInChildren<TextMeshProUGUI>().text);
            for (int i = 0; i < outCnt; i++)
            {
                var slot = slots.First;
                slots.RemoveFirst();
                slots.AddLast(slot.Value);

                // 데이터 초기화
                slot.Value.GetComponentInChildren<TextMeshProUGUI>().text = $"{++backNum}";

                // 위치 뒤로 바꾸기
                slot.Value.transform.SetSiblingIndex(slotBackIndex);
            }

            float diff = Math.Abs(value.y - prevScrollPos);
            if (diff >= 0.5f)
                diff -= 0.5f;
            scrollRect.verticalNormalizedPosition += 0.5f;
    
        }
        else if (value.y > 1)
        {
            int frontNum = int.Parse(slots.First.Value.GetComponentInChildren<TextMeshProUGUI>().text);
            outCnt = gridLayoutGroup.constraintCount;

            for (int i = 0; i < outCnt; i++)
            {
                var slot = slots.Last;
                slots.RemoveLast();
                slots.AddFirst(slot.Value); 
                
                // 데이터 초기화
                slot.Value.GetComponentInChildren<TextMeshProUGUI>().text = $"{--frontNum}";

                // 위치 앞으로 바꾸기
                slot.Value.transform.SetSiblingIndex(0);
            }

            
                
            scrollRect.verticalNormalizedPosition -= 0.5f + Math.Abs(value.y - prevScrollPos);  

        }
        

        prevScrollPos = scrollRect.verticalNormalizedPosition;
        
    }

    private int CheckSlotPosition()
    {
        int outCnt = 0;

        foreach (var slot in slots)
        {
            float y = GetSlotHeight(slot.transform.position.y, true);

            if (y > ContentMaxY)
                outCnt++;
            else
                break;
        }

        return outCnt;
    }

    private float GetSlotHeight(float posY, bool down = false)
    {
        if (down)
            return posY - (cellSize / 2 + spacing);
        return posY + (cellSize / 2 + spacing / 2);

    }

}
