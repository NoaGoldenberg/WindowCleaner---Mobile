using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WindowAssets : ScriptableObject
{
    public List<Sprite> dirtSprites;
    public List<RuntimeAnimatorController> personAnimatorControllers;
    public List<Sprite> backgroundSprites;
    public List<Sprite> windowFrames;
}
