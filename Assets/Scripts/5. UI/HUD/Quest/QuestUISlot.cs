using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUISlot : MonoBehaviour
{
    [SerializeField] private GameObject _taskUI;
    [SerializeField] private Transform _taskRoot;
    [SerializeField] private TextMeshProUGUI _title;
    
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(Quest quest)
    {
        _title.text = quest.QuestData.Title;
        quest.onCompleted += OnCompleted; 

        foreach (var task in quest.tasks)
        {
            GameObject taskUI = Instantiate(_taskUI, _taskRoot);
            taskUI.GetComponent<QuestTaskUI>().Setup(task);
        } 

        var rect = transform as RectTransform;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 80 + (quest.tasks.Count * 15));     
    }

    private void OnCompleted(Quest quest)
    {
        _canvasGroup.alpha = 0.5f;
    }
}
