using System.Collections.Generic;
using UnityEngine;

public class AnimationClipComparer : IEqualityComparer<AnimationClip>
{
    public bool Equals(AnimationClip clipA, AnimationClip clipB)
    {
        if (clipA == null || clipB == null) return false;
        if (clipA == clipB) return true;

        if (clipA.name != clipB.name) return false;
        if (clipA.legacy != clipB.legacy) return false;
        if (!Mathf.Approximately(clipA.length, clipB.length)) return false;
        if (!Mathf.Approximately(clipA.frameRate, clipB.frameRate)) return false;
        if (clipA.wrapMode != clipA.wrapMode) return false;
        if (clipA.isLooping != clipB.isLooping) return false;
        if (!AreEventsEqual(clipA.events, clipB.events)) return false;

        return true;
    }

    public int GetHashCode(AnimationClip clip)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + clip.name.GetHashCode();
            hash = hash * 23 + clip.length.GetHashCode();
            hash = hash * 23 + clip.frameRate.GetHashCode();

            return hash;
        }
    }

    private bool AreEventsEqual(AnimationEvent[] a, AnimationEvent[] b)
    {
        if (a.Length != b.Length) return false;

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i].functionName != b[i].functionName) return false;
            if (!Mathf.Approximately(a[i].time, b[i].time)) return false;
        }
        return true;
    }
}

[System.Serializable]
public class AnimationDataManager
{
    [SerializeField] private AnimationDataSO animationDataSO;
    public AnimationDataSO AnimationDataSO => animationDataSO;
    public static Dictionary<AnimationClip, int> animationClipToIndex = new Dictionary<AnimationClip, int>(new AnimationClipComparer());
    [field: SerializeField] public string REGISTRY_PATH { get; private set; } = "AnimationDataSO";
    
    public void Init()
    {
        LoadRegistry();
    }

    // 저장소 로드
    private void LoadRegistry()
    {
        if (animationDataSO == null)
        {
            animationDataSO = Resources.Load<AnimationDataSO>(REGISTRY_PATH);
        }
        
        RebuildCache();
    }
    
    // 캐시 재구성
    private void RebuildCache()
    {
        animationClipToIndex.Clear();
        
        if (animationDataSO != null)
        {
            var clips = AnimationDataSO.animationClips;
            for (int i = 0; i < clips.Count; i++)
            {
                if (clips[i] != null)
                {
                    animationClipToIndex[clips[i]] = i;
                }
            }
        }
    }

    // 애니메이션 클립의 인덱스 가져오기
    public int GetIndex(AnimationClip animationClip)
    {
        if (animationClip == null)
            return -1;

        // 캐시에서 먼저 찾기
        if(animationClipToIndex.TryGetValue(animationClip, out int index))
            return index;

        return -1;
    }
    
    // 인덱스로 애니메이션 클립 가져오기
    public AnimationClip GetByIndex(int index)
    {
        if(index < 0 || index >= animationClipToIndex.Count)
            return null;

        return AnimationDataSO.animationClips[index];
    }

#if UNITY_EDITOR
    public static int AddAnimationClip(AnimationDataSO animationDataSO, AnimationClip animationClip)
    {
        if (animationClip == null)
            return -1;
        
        // 이미 캐시에 있는지 확인
        if (animationClipToIndex.TryGetValue(animationClip, out int index))
            return index;
        
        // 실제 저장 및 인덱스 얻기
        int newIndex = animationDataSO.animationClips.Count;
        
        // 캐시 업데이트
        if (newIndex >= 0)
        {
            animationDataSO.animationClips.Add(animationClip);
            animationClipToIndex[animationClip] = newIndex;
            // 에디터에서는 변경 내용 저장
            UnityEditor.EditorUtility.SetDirty(animationDataSO);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        return newIndex;
    }
    

    // 데이터 초기화
    public static void ResetData(AnimationDataSO animationDataSO)
    {
        animationDataSO.animationClips.Clear();
        animationClipToIndex.Clear();

        UnityEditor.EditorUtility.SetDirty(animationDataSO);
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
}
