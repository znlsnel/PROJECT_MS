using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText; 
    [SerializeField] private TextMeshProUGUI hourText;

    void Start()
    {
        Managers.scene.TimeSystem.onChangedTime += OnChangedTime;
    }



    private void OnChangedTime(int day, int hour, int minute)
    {
        dayText.text = $"{day}일차";
        hourText.text = $"{hour:D2} : {minute:D2}"; 
    }
}
