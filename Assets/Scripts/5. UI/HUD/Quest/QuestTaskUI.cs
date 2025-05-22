using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestTaskUI : MonoBehaviour
{
    [SerializeField] private Image _checkImage;
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
        _title.text = questTask.taskData.TaskTitle;
        _currentCount.text = questTask.progress.ToString();
        _targetCount.text = questTask.taskData.SuccessCount.ToString();

        questTask.onProgressChanged += UpdateProgress;
        questTask.onMoveToNextTask += OnMoveToNextTask;

        UpdateProgress(questTask, questTask.progress, questTask.taskData.SuccessCount);
        
    }

    public void UpdateProgress(QuestTask questTask, int currentCount, int targetCount)
    {
        _currentCount.text = currentCount.ToString();
        _targetCount.text = targetCount.ToString();

        _checkImage.gameObject.SetActive(currentCount >= targetCount);
        if (currentCount >= targetCount)
        {
            // TODO 색 정리
            _canvasGroup.alpha = 0.5f;
            _title.fontStyle = FontStyles.Strikethrough; 
        }
    }

    private void OnDestroy()
    { 
        _questTask.onProgressChanged -= UpdateProgress;
    }

    private void OnMoveToNextTask(QuestTask questTask)
    {
        _questTask = questTask;
        _title.text = questTask.taskData.TaskTitle;

        _title.transform.localScale = Vector3.zero;
        _title.transform.DOScale(1f, 1f).SetEase(Ease.OutCubic);
    }
}
