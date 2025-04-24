using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameTagUI : MonoBehaviour, IBillboardable
{
    public TextMeshProUGUI nameText;

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void UpdatePosition(Vector3 screenPos)
    {
        transform.position = screenPos;
    }
}
