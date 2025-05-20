using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationDataSO", menuName = "Data/Animation Data SO")]
public class AnimationDataSO : ScriptableObject
{
    public List<AnimationClip> animationClips = new List<AnimationClip>();

    public Dictionary<AnimationClip, int> animationClipToIndex = new Dictionary<AnimationClip, int>();

    // 애니메이션 클립 추가 - 정적 인터페이스
    public int AddAnimationClip(AnimationClip animationClip)
    {
        if (animationClip == null)
            return -1;
        
        // 이미 캐시에 있는지 확인
        if (animationClipToIndex.TryGetValue(animationClip, out int index))
            return index;
        
        // 실제 저장 및 인덱스 얻기
        int newIndex = animationClips.Count;
        
        // 캐시 업데이트
        if (newIndex >= 0)
        {
            animationClips.Add(animationClip);
            animationClipToIndex[animationClip] = newIndex;
            
#if UNITY_EDITOR
            // 에디터에서는 변경 내용 저장
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
        
        return newIndex;
    }
    
    // 데이터 초기화
    public void ResetData()
    {
        animationClips.Clear();
        animationClipToIndex.Clear();
        
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

    // 모든 애니메이션 클립 목록 가져오기
    public IReadOnlyList<AnimationClip> GetAnimationClips()
    {
        return animationClips;
    }

}
