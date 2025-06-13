using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSectionEnd : MonoBehaviour
{
    [SerializeField] Transform stageSectionSpawnPoint;

    public Transform GetNextSpawnPoint()
    {
        return stageSectionSpawnPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("TrainCollider"))
        {
            // Spawn the next stage section at the spawn point of the current section
           GameManager.instance.StageSpawner.SpawnStageSection();
           StartCoroutine(GameManager.instance.StageSpawner.DestroyStageSection(this.gameObject));
        }
    }
}
