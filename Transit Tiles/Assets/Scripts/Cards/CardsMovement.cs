using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardsMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Movement Settings")]
    [SerializeField] private float returnSpeed = 10f;
    [SerializeField] private float hoverSpeed = 2f;
    [SerializeField] private float hoverHeight = 1.1f;

    private GameObject designatedSlot;
    private Vector3 originalScale;
    private int originalSiblingIndex;

    [Header("Flags")]
    private bool isDragging = false;
    private bool isSelected = false;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform targetArea;
    [SerializeField] private RectTransform rectTransform;
    private UnityEngine.UI.Image targetImage;

    private void Start()
    {
        originalScale = transform.localScale;
        originalSiblingIndex = transform.GetSiblingIndex();
        isDragging = false;
        isSelected = false;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (targetArea != null)
        {
            targetImage = targetArea.GetComponent<UnityEngine.UI.Image>();
        }
    }

    public void SetSlot(GameObject slot)
    {
        designatedSlot = slot;
    }

    #region POINTER
    public void OnPointerDown(PointerEventData eventData)
    {
        isSelected = true;
        transform.localScale *= 1.1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isSelected = false;
        transform.localScale = originalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging || !IsAtDesignatedSlot() || isSelected) { return; }

        StartCoroutine(DoHover(designatedSlot.transform.position.y * hoverHeight));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging || isSelected) { return; }
        StartCoroutine(ReturnToOriginalPosition());
    }
    #endregion

    #region DRAG
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        transform.SetParent(transform.parent.parent);
        transform.SetAsLastSibling(); // Bring to front while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        StartCoroutine(ReturnToOriginalPosition());

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (RectTransformUtility.RectangleContainsScreenPoint(targetArea, Input.mousePosition, eventData.pressEventCamera))
        {
            // snap the object to the target area's position
            rectTransform.anchoredPosition = targetArea.anchoredPosition;

            Debug.Log($"{gameObject.name} dropped in the target area!");
        }
        else
        {
            Debug.Log($"{gameObject.name} dropped outside the target area.");
        }
    }
    #endregion

    private IEnumerator ReturnToOriginalPosition()
    {
        transform.SetParent(designatedSlot.transform);
        transform.SetSiblingIndex(originalSiblingIndex);

        while (Vector3.Distance(transform.position, designatedSlot.transform.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, designatedSlot.transform.position, returnSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = designatedSlot.transform.position;
        transform.localScale = originalScale;
    }

    IEnumerator DoHover(float targetY)
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = new Vector3(currentPosition.x, targetY, currentPosition.z);

        while (Vector2.Distance(currentPosition, targetPosition) > 0.1f)
        {
            float newY = Mathf.Lerp(currentPosition.y, targetY, hoverSpeed * Time.deltaTime);

            transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
            currentPosition = transform.position;
            yield return null;
        }
    }

    private bool IsAtDesignatedSlot()
    {
        if (designatedSlot == null) return false;
        return Vector3.Distance(transform.position, designatedSlot.transform.position) < 0.1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ActivateCard"))
        {
            Destroy(gameObject);
            Debug.Log("Card Activated");
        }
    }
}
