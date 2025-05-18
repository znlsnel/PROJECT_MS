using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SystemDialogUI : PopupUI
{
    [SerializeField] private TextMeshProUGUI _textField;
    
    private void Start()
    {
        _textField.text = "";
        var texts = Managers.Data.ruleGuides.GetScript();
        StartCoroutine(ShowText(texts));
    }

    private IEnumerator ShowText(List<string> texts)
    {
        StartCoroutine(Fade(1, 0.5f));
        yield return new WaitForSeconds(0.6f);

        for (int i = 0; i < texts.Count; i++)
        {
            string text = texts[i];
            for (int j = 0; j < text.Length; j++)
            {
                _textField.text += text[j];
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(3f);

            if (i < texts.Count - 1)
                _textField.text = "";
            
        }
        StartCoroutine(Fade(0, 1f));
    }

}
