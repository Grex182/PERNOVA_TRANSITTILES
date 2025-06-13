using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _cardSlots = new List<GameObject>();
    [SerializeField] private GameObject _cardPrefab;

    public void DrawRandomCard()
    {
        foreach (GameObject slot in _cardSlots)
        {
            if (slot.transform.childCount == 0)
            {
                var newCard = Instantiate(_cardPrefab, slot.transform);
                CardsMovement movementScript = newCard.GetComponent<CardsMovement>();

                movementScript.SetSlot(slot);
                return;
            }
        }
    }
}
