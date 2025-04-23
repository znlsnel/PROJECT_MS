using UnityEngine;

public abstract class SceneInitializer : MonoBehaviour
{
    private void Start()
    {
        Initialize();
    }

    public abstract void Initialize();
}
