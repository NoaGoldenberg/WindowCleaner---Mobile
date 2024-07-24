using Unity.Collections;
using UnityEngine;


[CreateAssetMenu]
public class WindowPlaceParameters : ScriptableObject
{
    [Tooltip(
        "how much of the width of the screen each window block takes, i.e. 0.5 means 2 window blocks take the whole width of the screen.")]
    public float windowBlockWidthPercentage = 0.2f;
    
     //this was a variable at the beggining, but we settled on 3 and now changing this will break some stuff.
    public readonly int windowBlocksPerRow = 3;
}