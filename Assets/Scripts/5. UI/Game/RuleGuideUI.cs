using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RuleGuideUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    
    private List<string> ruleGuides;


    private void Start()
    {
        ruleGuides = Managers.Data.ruleGuides.GetScript();
    }

    private IEnumerator ShowRuleGuide()
    {
        yield return new WaitForSeconds(1f);
        text.text = ruleGuides[0];
    }

}
