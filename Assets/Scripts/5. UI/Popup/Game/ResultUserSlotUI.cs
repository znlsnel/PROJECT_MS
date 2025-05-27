using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUserSlotUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _nicknameText;
    [SerializeField] private TextMeshProUGUI _killText;

    [Header("Image")]
    [SerializeField] private GameObject _roleText_Mafia;    
    [SerializeField] private GameObject _roleText_Survivor;
    [SerializeField] private GameObject _surviveText_Survive;
    [SerializeField] private GameObject _surviveText_Dead;



    public void Setup(string name, PlayerRole role, bool survive, int kill)
    {
        _nicknameText.text = name;

        _roleText_Mafia.SetActive(role == PlayerRole.Imposter);
        _roleText_Survivor.SetActive(role == PlayerRole.Survival); 

        _surviveText_Survive.SetActive(survive);
        _surviveText_Dead.SetActive(!survive);

        _killText.text = kill.ToString() + "킬"; 
    }



}
