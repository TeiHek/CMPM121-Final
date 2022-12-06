using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager instance { get; private set; }
    [SerializeField] private Transform fade;
    private Animator fadeAnim;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        fadeAnim = fade.GetComponent<Animator>();
    }

    public void changeScene(string sceneName) {
        StartCoroutine(fadeToScene(sceneName));
    }

    public void exitGame() {
        Application.Quit(0);
    }

    private IEnumerator fadeToScene(string sceneName) {
        fade.gameObject.SetActive(true);
        fadeAnim.Play("FadeOut");
        yield return new WaitUntil(() => fade.GetComponent<FadeTransition>().transitionOver());
        SceneManager.LoadScene(sceneName);
    }
}
