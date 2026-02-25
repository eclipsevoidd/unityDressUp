using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private static Dictionary<string, DraggableItem> equippedItems = new Dictionary<string, DraggableItem>();

    [Header("Kategorijas iestatījumi")]
    public string categoryID;

    [Header("Skaņas efekti")]
    [SerializeField] private AudioClip dragSound;
    [SerializeField] private AudioClip dropSound;
    private AudioSource audioSource;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 startPosition;
    private Transform startParent;
    private Canvas mainCanvas;
    private Vector3 originalItemScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        mainCanvas = GetComponentInParent<Canvas>();
        audioSource = GetComponent<AudioSource>();

        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        startPosition = rectTransform.localPosition;
        startParent = transform.parent;
        // Saglabājam oriģinālo mērogu no Inspector loga
        originalItemScale = transform.localScale;
    }

    public void ApplyRescale(float widthMultiplier, float heightMultiplier)
    {
        transform.localScale = new Vector3(
            originalItemScale.x * widthMultiplier,
            originalItemScale.y * heightMultiplier,
            originalItemScale.z
        );
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragSound) audioSource.PlayOneShot(dragSound);

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(mainCanvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (IsOverCharacter(eventData))
        {
            EquipItem();
            if (dropSound) audioSource.PlayOneShot(dropSound);
        }
        else
        {
            ReturnToCloset();
        }
    }

    private void EquipItem()
    {
        // 1. Ja kategorijā jau kaut kas ir, atgriežam to skapī
        if (equippedItems.ContainsKey(categoryID) && equippedItems[categoryID] != this)
        {
            equippedItems[categoryID].ReturnToCloset();
        }

        equippedItems[categoryID] = this;

        // 2. Atrodam tēlu un piesaistām drēbi tam kā bērnu (Parenting)
        GameObject character = GameObject.FindWithTag("Character");
        if (character != null)
        {
            transform.SetParent(character.transform);

            // 3. TUZREIZ piemērojam slaidera vērtību, lai drēbe nepaliktu maza
            ClothingResizer resizer = Object.FindAnyObjectByType<ClothingResizer>();
            if (resizer != null)
            {
                resizer.ApplyCurrentScaleToItem(this);
            }
        }
    }

    public void ReturnToCloset()
    {
        if (equippedItems.ContainsKey(categoryID) && equippedItems[categoryID] == this)
        {
            equippedItems.Remove(categoryID);
        }

        transform.SetParent(startParent);
        rectTransform.localPosition = startPosition;

        // Atgriežam oriģinālo izmēru, kad drēbe ir skapī
        transform.localScale = originalItemScale;
    }

    private bool IsOverCharacter(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("Character")) return true;
        }
        return false;
    }
}