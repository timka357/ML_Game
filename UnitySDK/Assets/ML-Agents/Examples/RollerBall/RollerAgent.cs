using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAgent : Agent
{
    public Transform Target;

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




}
