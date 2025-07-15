using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationControllerPP : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            animator.SetBool("IsMoving", false);
        }
        else
        {
            animator.SetBool("IsMoving", true);
        }
    }
}
