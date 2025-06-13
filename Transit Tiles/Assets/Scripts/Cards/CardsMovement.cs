using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardsMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Movement Settings")]
    [SerializeField] private float returnSpeed = 10f;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private bool isDragging;

    private void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale *= 1.1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        StartCoroutine(ReturnToOriginalPosition());
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = originalPosition;
        transform.localScale = originalScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CardSlot"))
        {
            originalPosition = collision.gameObject.transform.position;
        }
    }
}
