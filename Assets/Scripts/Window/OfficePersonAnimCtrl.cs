using UnityEngine;

public class OfficePersonAnimCtrl : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Window parentWindow;
    
    public void InitializeAnimation(RuntimeAnimatorController controller, float animationStartPoint)
    {
        anim.runtimeAnimatorController = controller;
        anim.Play(anim.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, animationStartPoint);
    }

    public void StartLooking()
    {
        parentWindow.StartLooking();
    }
    
    public void StopLooking()
    {
        parentWindow.StopLooking();
    }
   
}
