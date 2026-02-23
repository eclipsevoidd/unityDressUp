using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    public bool IsTransitioning { get; private set; }
    // seko līdzi pārejas stāvoklim

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // pārliecinamies, ka netiek iznīcināts sākot scenam
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
        if (IsTransitioning) return;

        IsTransitioning = true;
        fadeImage.raycastTarget = true;

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
        fadeImage.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            fadeImage.raycastTarget = false;
            IsTransitioning = false;
        });
    }
}