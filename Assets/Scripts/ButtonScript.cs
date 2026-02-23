using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum ButtonType { Start, Quit, Settings }

    [Header("Button Logic")]
    [SerializeField] private ButtonType typeOfButton;
    [SerializeField] private string sceneToLoad = "Game";

    [Header("Animation Settings")]
    [SerializeField] private float hoverScale = 1.15f;
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private Ease easeType = Ease.OutBack;

    [Header("Audio")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    private AudioSource m_AudioSource;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SceneChanger.Instance != null && SceneChanger.Instance.IsTransitioning) return;

        transform.DOKill();
        transform.DOScale(originalScale * hoverScale, duration).SetEase(easeType);

        if (m_AudioSource && hoverSound)
            m_AudioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originalScale, duration).SetEase(Ease.OutQuad);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SceneChanger.Instance != null && SceneChanger.Instance.IsTransitioning)
            return;

        if (m_AudioSource && clickSound)
            m_AudioSource.PlayOneShot(clickSound);

        switch (typeOfButton)
        {
            case ButtonType.Start:
                StartGame();
                break;
            case ButtonType.Quit:
                QuitGame();
                break;
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }

    private void StartGame()
    {
        if (SceneChanger.Instance != null)
        {
            SceneChanger.Instance.ChangeScene(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void QuitGame()
    {
        Debug.Log("Spēle beigta");
        Application.Quit();
    }
}