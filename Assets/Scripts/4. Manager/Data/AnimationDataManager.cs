using UnityEngine;

[System.Serializable]
public class AnimationDataManager
{
    [field: SerializeField] public AnimationDataSO AnimationDataSO { get; private set; }
    [field: SerializeField] public string REGISTRY_PATH { get; private set; } = "AnimationDataSO";
    
    public void Init()
    {
        LoadRegistry();
    }

    // 저장소 로드
    private void LoadRegistry()
    {
        if (AnimationDataSO == null)
        {
            AnimationDataSO = Resources.Load<AnimationDataSO>(REGISTRY_PATH);

            if(AnimationDataSO == null)
                return;
        }
        
        RebuildCache();
    }
    
    // 캐시 재구성
    private void RebuildCache()
    {
        AnimationDataSO.animationClipToIndex.Clear();
        
        if (AnimationDataSO != null)
        {
            var clips = AnimationDataSO.animationClips;
            for (int i = 0; i < clips.Count; i++)
            {
                if (clips[i] != null)
                {
                    AnimationDataSO.animationClipToIndex[clips[i]] = i;
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
        if (AnimationDataSO.animationClipToIndex.TryGetValue(animationClip, out int index))
            return index;

        return -1;
    }
    
    // 인덱스로 애니메이션 클립 가져오기
    public AnimationClip GetByIndex(int index)
    {
        if(index < 0 || index >= AnimationDataSO.animationClips.Count)
            return null;

        return AnimationDataSO.animationClips[index];
    }
}
