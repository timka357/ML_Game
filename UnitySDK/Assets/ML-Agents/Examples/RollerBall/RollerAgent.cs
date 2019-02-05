using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAgent : Agent
{
    public Transform Target;
    public float speed = 10f;

    private Rigidbody _rBody;


    void Start()
    {
        _rBody = GetComponent<Rigidbody>();
    }

    public override void AgentReset()
    {
        if (this.transform.position.y < 0)
        {
            _rBody.angularVelocity = Vector3.zero;
            _rBody.velocity = Vector3.zero;
            transform.position = new Vector3(0, 0.5f, 0);
        }

        Target.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations()
    {
        AddVectorObs(Target.position);
        AddVectorObs(transform.position);
        AddVectorObs(_rBody.velocity.x);
        AddVectorObs(_rBody.velocity.z);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Vector3 ControlSignal = Vector3.zero;
        ControlSignal[0] = vectorAction[0];
        ControlSignal[1] = vectorAction[1];
        _rBody.AddForce(ControlSignal * speed);

        //rewards
        float distanceToTarget = Vector3.Distance(transform.position, Target.position);

        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        if (transform.position.y < 0)
        {
            Done();
        }
    }



}
