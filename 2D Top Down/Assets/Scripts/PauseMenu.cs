using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    // Use this for initialization
    public int index = 0;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                index = 0;
                Resume();
            }
            else
            {
                index = 0;
                Pause();
            }
        }

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (index >= 2)
                {
                    return;
                }
                index += 1;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (index <= 0)
                {
                    return;
                }
                index -= 1;
            }

            if (index == 0)
            {
                Debug.LogError("testing");
                animator.SetBool("Resume", true);
                animator.SetBool("Menu", false);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Resume();
                }
            }
            if (index == 1)
            {
                animator.SetBool("Resume", false);
                animator.SetBool("Menu", true);
                animator.SetBool("Quit", false);

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Resume();
                    LoadMenu();
                }
            }
            if (index == 2)
            {
                animator.SetBool("Menu", false);
                animator.SetBool("Quit", true);

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    QuitGame();
                }
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
