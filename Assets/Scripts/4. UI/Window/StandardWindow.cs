using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandardWindow : UIBase
{
    [SerializeField] private Button closeButton;

    public void Start()
    {
        if(closeButton != null)
            closeButton.onClick.AddListener(() => Hide());
    }

    public override void Show() => base.Show();

    public override void Hide() => base.Hide();
}
