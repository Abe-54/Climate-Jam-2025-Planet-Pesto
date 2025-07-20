using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPauseControllerPP : MonoBehaviour
{
  
    private PauseMenu pauseMenu;
   
    void Start()
    {
        if (!pauseMenu)
        {
            pauseMenu = FindAnyObjectByType<PauseMenu>();
        }
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            pauseMenu.PauseToggle();
        }
    }

 
}
