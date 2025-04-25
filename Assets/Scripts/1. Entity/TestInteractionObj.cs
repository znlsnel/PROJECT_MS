using UnityEngine;

public class TestInteractionObj : MonoBehaviour, IInteractable
{
    public AnimationClip interactAnimation;

    public GameObject Interact(GameObject gameObject)
    {
        Managers.Quest.ReceiveReport(ETaskCategory.Interact, 1000);
        return gameObject;
    } 

}
