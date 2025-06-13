using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSectionMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; // Units per second

    private void Update()
    {
        if (GameManager.instance.StationManager.isTrainMoving)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }
}
