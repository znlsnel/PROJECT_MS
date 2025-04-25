using UnityEngine;

public class TestInteractionObj : MonoBehaviour, IInteractable
{
    public AnimationClip interactAnimation;

    public GameObject Interact(GameObject gameObject)
    {
        Debug.Log("Interact");
        return gameObject;
    } 

}
