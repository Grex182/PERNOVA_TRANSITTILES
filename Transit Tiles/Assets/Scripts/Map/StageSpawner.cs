using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] Transform initialSpawnPoint;
    [SerializeField] Transform spawnerStageSectionSpawnPoint;

    [Header("Prefabs")]
    [SerializeField] GameObject _stageSectionPrefab;

    [Header("Floats")]
    [SerializeField] public float destroySectionDelay = 3f;
    //[SerializeField] float stagePositionX = -2.595f;

    [Header("Vectors")]
    [SerializeField] private Vector3 stagePosition = new Vector3(-29.2f, -2.2f, -0.7f);

    private void Start()
    {
        GameManager.instance.StageSpawner = this;

        if (!spawnerStageSectionSpawnPoint)
            spawnerStageSectionSpawnPoint = initialSpawnPoint;

        StartStage();
    }

    public IEnumerator DestroyStageSection(GameObject stageObj)
    {
        Debug.Log("Destroying: " + stageObj.name);

        yield return new WaitForSeconds(destroySectionDelay);

        Destroy(stageObj);
        Debug.Log("Stage has been destroyed");
    }

    public void SpawnStageSection()
    {
        // Spawn the next stage section at the current stage's spawn point
        GameObject newStageSection = Instantiate(_stageSectionPrefab, 
                                                 new Vector3(spawnerStageSectionSpawnPoint.position.x, spawnerStageSectionSpawnPoint.position.y, spawnerStageSectionSpawnPoint.position.z), 
                                                 Quaternion.Euler(-89.98f, spawnerStageSectionSpawnPoint.rotation.eulerAngles.y, spawnerStageSectionSpawnPoint.rotation.eulerAngles.z));

        // Update the spawn point to the new stage section's spawn point
        spawnerStageSectionSpawnPoint = newStageSection.GetComponentInChildren<StageSectionEnd>().GetNextSpawnPoint();

        Debug.Log("Spawned Next Stage Section");
    }

    public void StartStage()
    {
        GameObject startingStageSection = Instantiate(_stageSectionPrefab, new Vector3(stagePosition.x, stagePosition.y, stagePosition.z), Quaternion.Euler(-89.98f, spawnerStageSectionSpawnPoint.rotation.eulerAngles.y, spawnerStageSectionSpawnPoint.rotation.eulerAngles.z));

        spawnerStageSectionSpawnPoint = startingStageSection.GetComponentInChildren<StageSectionEnd>().GetNextSpawnPoint();

        Debug.Log("Spawned Starting Stage");
    }
}
