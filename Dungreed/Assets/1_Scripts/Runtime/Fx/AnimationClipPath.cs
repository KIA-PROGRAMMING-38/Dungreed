using System.IO;
using UnityEngine;

public static class AnimationClipPath
{
    public static readonly string DefaultFxClipPath= "AnimationClips\\Fx";

    public static AnimationClip GetAnimationClipResource(string filename, string folderPath = null)
    {
        string path = Path.Combine(DefaultFxClipPath, filename);
        var clip = Resources.Load<AnimationClip>(path);
        Debug.Assert(clip != null, "AnimationClipPath was Wrong");
        return clip;
    }
}
