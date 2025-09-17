using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MazeAgent : Agent
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float turnSpeed = 200f;

    [Header("References")]
    public MazeArea area;
    public Transform goal;

    Rigidbody rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset agent physics
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Reset maze + agent + goal
        if (area != null)
        {
            area.ResetArea();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Relative position to goal
        Vector3 relativePosition = goal.position - transform.position;

        // Agent’s forward direction (3 values)
        sensor.AddObservation(transform.forward);

        // Agent’s velocity (3 values)
        sensor.AddObservation(rb.linearVelocity);

        // Relative goal position (3 values)
        sensor.AddObservation(relativePosition);
        // ---- Total = 9 observations ----
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = actions.ContinuousActions[0];
        float turn = actions.ContinuousActions[1];

        // Movement
        rb.MovePosition(transform.position + transform.forward * move * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up * turn * turnSpeed * Time.fixedDeltaTime);

        // Step penalty (encourages faster solving)
        AddReward(-0.001f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Vertical");   // Forward / Back
        continuousActions[1] = Input.GetAxis("Horizontal"); // Turn Left / Right
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            SetReward(1f);   // Big reward for reaching goal
            EndEpisode();
        }
        else if (other.CompareTag("Wall"))
        {
            AddReward(-0.5f); // Penalty for hitting wall
            EndEpisode();
        }
    }
}
