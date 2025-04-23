using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBillboardable
{
    void SetActive(bool active);
    void UpdatePosition(Vector3 screenPos);
    GameObject gameObject { get; }
}
