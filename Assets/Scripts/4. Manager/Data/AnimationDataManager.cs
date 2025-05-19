using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AnimationDataManager
{
    private static List<AnimationClip> animationClips = new List<AnimationClip>();
    private static Dictionary<AnimationClip, int> animationClipToIndex = new Dictionary<AnimationClip, int>();

    public static int AddAnimationClip(AnimationClip animationClip)
    {
        if (animationClipToIndex.ContainsKey(animationClip))
        {
            return animationClipToIndex[animationClip];
        }

        animationClips.Add(animationClip);
        animationClipToIndex[animationClip] = animationClips.Count - 1;
        return animationClips.Count - 1;
    }

    public static void ResetData()
    {
        animationClips.Clear();
        animationClipToIndex.Clear();
    }

    public static AnimationClip GetByIndex(int index)
    {
        return animationClips[index];
    }

    public static IReadOnlyList<AnimationClip> GetAnimationClips()
    {
        return animationClips;
    }

    public static int GetIndex(AnimationClip animationClip)
    {
        return animationClipToIndex[animationClip];
    }
}
