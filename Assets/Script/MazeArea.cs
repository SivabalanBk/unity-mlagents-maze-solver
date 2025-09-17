using System.Collections.Generic;
using UnityEngine;

public class MazeArea : MonoBehaviour
{
    [Header("Scene References")]
    public MazeAgent agent;
    public Transform goal;

    [Header("Spawn Points")]
    public List<Transform> agentSpawnPoints;
    public List<Transform> goalSpawnPoints;

    public void ResetArea()
    {
        // Reset Agent
        if (agentSpawnPoints.Count > 0)
        {
            int i = Random.Range(0, agentSpawnPoints.Count);
            agent.transform.position = agentSpawnPoints[i].position;
            agent.transform.rotation = agentSpawnPoints[i].rotation;
        }

        // Reset Goal
        if (goalSpawnPoints.Count > 0)
        {
            int j = Random.Range(0, goalSpawnPoints.Count);
            goal.position = goalSpawnPoints[j].position;
            goal.rotation = goalSpawnPoints[j].rotation;
        }
    }
}
