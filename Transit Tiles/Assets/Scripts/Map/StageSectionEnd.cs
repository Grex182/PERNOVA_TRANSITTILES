using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SectionEndPosition
{
    Left,
    Right,
}

public class StageSectionEnd : MonoBehaviour
{
    [SerializeField] private SectionEndPosition sectionEndPosition;

    [SerializeField] Transform stageSectionSpawnPoint;

    public Transform GetNextSpawnPoint()
    {
        return stageSectionSpawnPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("TrainCollider"))
        {
            switch(sectionEndPosition)
            {
                case SectionEndPosition.Left:
                    if (!GameManager.instance.StationManager.isMovingRight)
                    {
                        GameManager.instance.StageSpawner.SpawnStageSection();
                        StartCoroutine(GameManager.instance.StageSpawner.DestroyStageSection(transform.parent.gameObject));
                    }
                    break;
                case SectionEndPosition.Right:
                    if (GameManager.instance.StationManager.isMovingRight)
                    {
                        GameManager.instance.StageSpawner.SpawnStageSection();
                        StartCoroutine(GameManager.instance.StageSpawner.DestroyStageSection(transform.parent.gameObject));
                    }
                    break;
            }

            // Spawn the next stage section at the spawn point of the current section
        }
    }
}
