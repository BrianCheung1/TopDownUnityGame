using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour
{
    public int index = 0;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        Pause();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (index >= 1)
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
            animator.SetBool("Menu", true);
            animator.SetBool("Quit", false);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("MainMenu");
                Time.timeScale = 1f;
            }

        }
        if (index == 1)
        {
            animator.SetBool("Menu", false);
            animator.SetBool("Quit", true);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                QuitGame();
            }

        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
