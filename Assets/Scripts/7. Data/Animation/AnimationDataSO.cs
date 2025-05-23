using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationDataSO", menuName = "Data/Animation Data SO")]
public class AnimationDataSO : ScriptableObject
{
    public List<AnimationClip> animationClips = new List<AnimationClip>();

    // 모든 애니메이션 클립 목록 가져오기
    public IReadOnlyList<AnimationClip> GetAnimationClips()
    {
        return animationClips;
    }

}
