using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Statiska vārdnīca, lai izsekotu, kas pašlaik ir uzvilkts katrā kategorijā
    private static Dictionary<string, DraggableItem> equippedItems = new Dictionary<string, DraggableItem>();

    [Header("Kategorijas iestatījumi")]
    public string categoryID; // Piemēram: "Uzvalks", "Bikses", "Cepure"

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 startPosition;
    private Transform startParent;
    private Canvas mainCanvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        mainCanvas = GetComponentInParent<Canvas>();

        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Saglabājam sākuma pozīciju un vecāku (skapi)
        startPosition = rectTransform.localPosition;
        startParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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
        }
        else
        {
            ReturnToCloset();
        }
    }

    private void EquipItem()
    {
        if (equippedItems.ContainsKey(categoryID) && equippedItems[categoryID] != this)
        {
            equippedItems[categoryID].ReturnToCloset();
        }

        equippedItems[categoryID] = this;

        Debug.Log($"Uzvilkts: {gameObject.name} kategorijā {categoryID}");
    }

    public void ReturnToCloset()
    {
        if (equippedItems.ContainsKey(categoryID) && equippedItems[categoryID] == this)
        {
            equippedItems.Remove(categoryID);
        }

        transform.SetParent(startParent);
        rectTransform.localPosition = startPosition;
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