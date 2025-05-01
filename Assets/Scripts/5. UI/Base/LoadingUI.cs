using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Image progressBar;

    private void Update()
    {
      //  progressBar.fillAmount = Managers.Network.LoadingProgress / 0.9f;
    }
}
