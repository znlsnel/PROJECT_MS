using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUserSlotUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _nicknameText;
    [SerializeField] private TextMeshProUGUI _killText;

    [Header("Image")]
    [SerializeField] private Image _nicknameBackground; 
    [SerializeField] private GameObject _roleText_Mafia;    
    [SerializeField] private GameObject _roleText_Survivor;
    [SerializeField] private GameObject _surviveText_Survive;
    [SerializeField] private GameObject _surviveText_Failed;
    [SerializeField] private GameObject _surviveText_Dead;



    public void Setup(string name, Color color, EPlayerRole role, bool survive, int kill)
    {
        _nicknameText.text = name;
        _nicknameBackground.color = color;

        _roleText_Mafia.SetActive(role == EPlayerRole.Imposter);
        _roleText_Survivor.SetActive(role == EPlayerRole.Survival); 

        _surviveText_Survive.SetActive(survive); 
        _surviveText_Dead.SetActive(!survive); 

        bool isEndTime = Managers.scene.GetComponent<TimeSystem>().IsTimeEnd;
        bool isCompleted = ForestScene.isCompleted;

        if (role == EPlayerRole.Survival && survive && isEndTime)
        {
            _surviveText_Failed.SetActive(true);
            _surviveText_Survive.SetActive(false);  
        }

        if (role == EPlayerRole.Imposter && survive && isCompleted)
        {
            _surviveText_Failed.SetActive(true);
            _surviveText_Survive.SetActive(false);   
        }


        _killText.text = kill.ToString() + "킬"; 

        // 실패 조건 -> 생존자 && isEndTime == true
    }



}
