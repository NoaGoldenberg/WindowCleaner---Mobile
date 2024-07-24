using UnityEngine;

public static class PoopTracker
{
    public static int PoopCount { get;  set; }
    private static int windowsSinceLastPoop = 0; //makes it less likely for there to be poop on the first row, intended.
    private static float poopChanceIncreaseModifier = 0.05f;
    //poops are most likely to happen every 3-6 windows
   

    public static void init()
    {
        PoopCount = 0;
        windowsSinceLastPoop = 0;
        
    }

    public static bool ShouldAddPoop()
    {
        // every window calls this function on initalization.
        // meaning if there hasn't been a poop in a while, the next window is more likely to have poop.
        // and if this window gets false(no poop), the next one is more likely etc.
        windowsSinceLastPoop++;
        float randVal = Random.value;
        if(randVal < poopChanceIncreaseModifier * windowsSinceLastPoop)
        {
            windowsSinceLastPoop = 0;
            return true;
        }
        return false;
    }
}
