using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public enum Team
{
    Orange = 0,
    Green = 1
}

public class RollerAgentVs : Agent
{
    BehaviorParameters m_BehaviorParameters;
    Team team;

    float m_Existential = 0.002f;

    Rigidbody rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        m_BehaviorParameters = GetComponent<BehaviorParameters>();
        if (m_BehaviorParameters.TeamId == (int)Team.Orange)
        {
            team = Team.Orange;
        }
        else
        {
            team = Team.Green;
        }
    }

    public Transform Target;
    public Rigidbody Obstacle;
    public override void OnEpisodeBegin()
    {
        // Move the agent to the beginning
        if (team == Team.Orange)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(2f, 0.5f, -3f);
        }
        else
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0f, 0.5f, 3f);
        }
        

        // Move the target to a new spot
        Target.localPosition = new Vector3(Random.value * 8 - 4,
                                           0.5f,
                                           Random.value * 2 + 2);

        // Move the obstacle to a new spot
        Obstacle.angularVelocity = Vector3.zero;
        Obstacle.velocity = Vector3.zero;
        Obstacle.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        Obstacle.transform.localPosition = new Vector3(Random.value * 8 - 4,
                                                       0.5f,
                                                       Random.value * 2);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        //Obstacle position
        sensor.AddObservation(Obstacle.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);


        if (team == Team.Orange)
        {
            // Reached target
            if (distanceToTarget < 1.42f)
            {
                SetReward(1.0f);
                EndEpisode();
            }
            AddReward(-m_Existential);
        }
        else
        {
            AddReward(m_Existential);
        }
        

        // Fell off platform
        if (this.transform.localPosition.y < 0)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
