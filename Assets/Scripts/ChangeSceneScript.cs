using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance; // lai jebkurš cits skripts var piekļūt šim objektam

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        // nodrošina, ka ir tikai viena šī skripta instance un tā netiek iznīcināta citos scenos
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject.transform.root.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        FadeFromBlack();
    }

    public void ChangeScene(string sceneName)
    {
        fadeImage.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);

            SceneManager.sceneLoaded += OnSceneLoaded;
        });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeFromBlack();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void FadeFromBlack()
    {
        fadeImage.color = new Color(0, 0, 0, 1);
        fadeImage.DOFade(0f, fadeDuration);
    }
}