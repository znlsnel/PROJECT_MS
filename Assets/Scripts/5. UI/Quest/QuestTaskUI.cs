using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestTaskUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _currentCount;
    [SerializeField] private TextMeshProUGUI _targetCount;

    private CanvasGroup _canvasGroup;
    private QuestTask _questTask;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Setup(QuestTask questTask)
    {
        this._questTask = questTask;
        _title.text = questTask.taskData.TastTarget;
        _currentCount.text = questTask.progress.ToString();
        _targetCount.text = questTask.taskData.SuccessCount.ToString();

        questTask.onProgressChanged += UpdateProgress;
    }

    public void UpdateProgress(QuestTask questTask, int currentCount, int targetCount)
    {
        _currentCount.text = currentCount.ToString();
        _targetCount.text = targetCount.ToString();

        if (currentCount >= targetCount)
        {
            // TODO 색 정리
            _canvasGroup.alpha = 0.5f;
        }
    }

    private void OnDestroy()
    { 
        _questTask.onProgressChanged -= UpdateProgress;
    }
}
