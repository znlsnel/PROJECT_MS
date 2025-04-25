using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestTaskUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _currentCount;
    [SerializeField] private TextMeshProUGUI _targetCount;
    
    private QuestTask _questTask;
    public void Setup(QuestTask questTask)
    {
        this._questTask = questTask;
        _title.text = questTask.taskData.TaskName;
        _currentCount.text = questTask.progress.ToString();
        _targetCount.text = questTask.taskData.SuccessCount.ToString();

        questTask.onSuccessChanged += UpdateProgress;
    }

    public void UpdateProgress(QuestTask questTask, bool isSuccess)
    {
        _currentCount.text = questTask.progress.ToString();
        _targetCount.text = questTask.taskData.SuccessCount.ToString();

        if (isSuccess)
        {
            // TODO 색 정리
        }
    }

    private void OnDestroy()
    {
        _questTask.onSuccessChanged -= UpdateProgress;
    }
}
