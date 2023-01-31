using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Audio still needs to be paused?

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    //[SerializeField] Transform cameraRotation;
/*    [SerializeField] Transform camera1;
    [SerializeField] Transform camera2;
    [SerializeField] Transform player;*/

    bool pauseActive;

    void Start()
    {
        //sets pause menu to invis
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseActive = false;
    }

    void Update()
    {
        //if E is pressed and pause menu is invis put up pause menu
            //if pause menu is vis then put down pause menu
        if (Input.GetKeyDown(KeyCode.E) && pauseActive == false)
        {
            //pauses time
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            pauseActive = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(Input.GetKeyDown(KeyCode.E) && pauseActive == true)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            pauseActive = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToQuit()
    {
        Debug.Log("Quit Active");
        Application.Quit();
    }

    public void ToRetry()
    {
        /*        camera1.localRotation = cameraRotation.localRotation;
                camera2.localRotation = cameraRotation.localRotation;
                player.localRotation = cameraRotation.localRotation;*/
        Time.timeScale = 1;

/*        player.rotation = Quaternion.identity;
        camera1.localRotation = Quaternion.identity;
        camera2.localRotation = Quaternion.identity;*/

        //camera rotation does not reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        

    }
}
