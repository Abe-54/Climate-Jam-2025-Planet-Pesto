using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour {
   
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    void PauseToggle(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

public void Resume() {
    pauseMenuUI.SetActive(false);
    Time.timeScale = 1f;
    GameIsPaused = false;
}

void Pause() {
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f;
    GameIsPaused = true;
}

    public void LoadMenu()
    {
        Debug.Log("Loading Menu...");
    }

    public void QuitGame()
    {
        Debug.Log("Qutting game...");
    }
}
