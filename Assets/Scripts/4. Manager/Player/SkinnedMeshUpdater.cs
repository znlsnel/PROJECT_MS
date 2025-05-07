using System;
using UnityEngine;

public static class SkinnedMeshUpdater
{

	public static void UpdateMeshRenderer (Transform bone, SkinnedMeshRenderer meshRenderer)
	{
        Transform[] rig = bone.GetComponentsInChildren<Transform>();
		Transform[] bones = new Transform[meshRenderer.bones.Length];
		for (int i = 0; i < meshRenderer.bones.Length; i++) {
			bones [i] = Array.Find<Transform> (rig, c => c.name == meshRenderer.bones [i].name);
		}
 
		meshRenderer.bones = bones;
	}
}

