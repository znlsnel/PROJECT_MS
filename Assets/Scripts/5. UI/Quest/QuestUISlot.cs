using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUISlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image stateIcon;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    
    public void Setup(Quest quest)
    {
        _icon.sprite = quest.QuestData.Icon;
        _title.text = quest.QuestData.QuestName;
        _description.text = quest.QuestData.QuestDescription;
        // currentCount.text = quest.CurrentCount.ToString();
        // targetCount.text = quest.QuestData.TargetCount.ToString();
    }
}
