using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //singleton
    private static SceneLoader _instance;
    public static SceneLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SceneLoader>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    //end singleton

    public Animator animator;

    private void Start()
    {
        
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadTargetScene(scene));
    }

    IEnumerator LoadTargetScene(string scene)
    {
        if (animator == null)
        {
            animator = GameObject.Find("BlackScreen").GetComponent<Animator>();
            animator.SetTrigger("screenToBlack");//animator trigger
            yield return new WaitForSeconds(1f);

        }

        if(animator != null)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }


            if (asyncLoad.isDone)
            {
                animator = GameObject.Find("BlackScreen").GetComponent<Animator>();
                animator.SetTrigger("screenToShow");//animator trigger
            }
        }


    }

    public void ScreenToBlack()
    {
        StartCoroutine(FadeScreen());
    }
    IEnumerator FadeScreen()
    {
        yield return new WaitForSeconds(1f);
        animator = GameObject.Find("BlackScreen").GetComponent<Animator>();
        animator.SetTrigger("screenToBlack");//animator trigger
    }

    public void ScreenToShow()
    {
        StartCoroutine(ShowScreen());
    }
    IEnumerator ShowScreen()
    {
        yield return new WaitForSeconds(1f);
        animator = GameObject.Find("BlackScreen").GetComponent<Animator>();
        animator.SetTrigger("screenToShow");//animator trigger
    }

    public void ScreenTransition()
    {
        StartCoroutine(FadeScreen());
    }
    IEnumerator TransitionScreen()
    {
        yield return new WaitForSeconds(1f);
        animator = GameObject.Find("BlackScreen").GetComponent<Animator>();
        animator.SetTrigger("screenToBlack");//animator trigger
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("screenToShow");//animator trigger
    }

}
