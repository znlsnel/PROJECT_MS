using System;
using UnityEngine;

[Serializable]
public class GhostPlayerAnimationData
{
    [Header("State Group Parameter Names")]
    [SerializeField] private string groundParameterHashName = "Ground";
    [SerializeField] private string movingParameterHashName = "Moving";

    [Header("State Moving Parameter Names")]
    [SerializeField] private string idlingParameterHashName = "isIdling";
    [SerializeField] private string runningParameterHashName = "isRunning";
    [SerializeField] private string sprintingParameterHashName = "isSprinting";

    public int GroundParameterHash { get; private set; }
    public int MovingParameterHash { get; private set; }

    public int IdlingParameterHash { get; private set; }
    public int RunningParameterHash { get; private set; }
    public int SprintingParameterHash { get; private set; }

    public GhostPlayerAnimationData()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterHashName);
        MovingParameterHash = Animator.StringToHash(movingParameterHashName);
        
        IdlingParameterHash = Animator.StringToHash(idlingParameterHashName);
        RunningParameterHash = Animator.StringToHash(runningParameterHashName);
        SprintingParameterHash = Animator.StringToHash(sprintingParameterHashName);
    }
}
