using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicRatingManager : MonoBehaviour
{
    [Header("Public Rating")]
    [SerializeField] private float maxPublicRating = 5.0f;
    [SerializeField] private float startingPublicRating = 2.5f;
    [SerializeField] private float currentPublicRating;

    [SerializeField] private GameObject failObject;

    private void Start()
    {
        GameManager.instance.PublicRatingManager = this;

        currentPublicRating = startingPublicRating;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentPublicRating -= 2.5f;
            ReducePublicRating();
        }
    }

    public void AddPublicRating()
    {
        //Angry Standard: +0.5 PR | Angry Priority: +1 PR
        if (currentPublicRating >= maxPublicRating)
        {
            Debug.Log("Okay no more PR for you");
        }
        else
        {
            currentPublicRating += 0.5f;
            Debug.Log("Public Rating Increased! CurrentPublicRating: " + currentPublicRating);
        }
    }

    public void ReducePublicRating()
    {
        currentPublicRating -= 0.5f;
        Debug.Log("Public Rating Decreased. CurrentPublicRating: " + currentPublicRating);

        //Angry Standard: -0.5 PR | Angry Priority: -1 PR
        if (currentPublicRating <= 0)
        {
            Debug.Log("You Dead");
            failObject.SetActive(true);
            currentPublicRating = 0;
        }
    }
}
