using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour {

	// Use this for initialization
	public int index = 0;
	Animator animator;

	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
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
			if(index <= 0)
            {
				return;
            }
			index -= 1;
		}
		if (index == 0)
        {
			animator.SetBool("Play", true);
			animator.SetBool("Options", false);
            if (Input.GetKeyDown(KeyCode.Return))
            {
				index = 0;
				SceneManager.LoadScene("Controls");
            }
		}
		if(index == 1)
        {
			animator.SetBool("Play", false);
			animator.SetBool("Options", true);
			animator.SetBool("Quit", false);
			if (Input.GetKeyDown(KeyCode.Return))
			{
				index = 0;
				SceneManager.LoadScene("Controls");
			}
		}
		if (index == 2)
		{
			animator.SetBool("Options", false);
			animator.SetBool("Quit", true);
			if (Input.GetKeyDown(KeyCode.Return))
			{
				index = 0;
				Application.Quit();
			}
		}
	}

}
