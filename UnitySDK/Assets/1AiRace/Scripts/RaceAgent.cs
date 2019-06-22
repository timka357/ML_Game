using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class RaceAgent : Agent
{
    public float Height = 0.2f;
    private Rigidbody _rb;
    private RayPerception3D _rayPer;
    private SimpleCarController _controller;
    public LayerMask layerMask;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    float rayDistance = 50f;
    float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
    string[] detectableObjects = { "Border" };

    void Start()
    {
        _rb = transform.GetComponent<Rigidbody>();
        _rayPer = GetComponent<RayPerception3D>();
        _controller = GetComponent<SimpleCarController>();

        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    public override void CollectObservations()
    { 
        AddVectorObs(_rayPer.Perceive(rayDistance, rayAngles, detectableObjects, Height, Height)); //21
        AddVectorObs(_rb.velocity.magnitude); //1
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        MoveAgent(vectorAction);
    }

    private void MoveAgent(float[] vectorAction)
    {
        _controller.SetInput(Mathf.Clamp(vectorAction[0]*10, -1f, 1f));
        //RaycastHit hit, hitLeft;

        //var lft = -transform.right;
        //var rgt = transform.right;

        //if (Physics.Raycast(transform.position + new Vector3(0, Height, 0), rgt, out hit, 100, layerMask) && Physics.Raycast(transform.position + new Vector3(0, Height, 0), lft, out hitLeft, 100, layerMask))
        //{
        //    if(Mathf.Abs(hit.distance - hitLeft.distance) < 1)
        //    {
        //        AddReward(0.5f * _rb.velocity.magnitude);
        //    }
        //}
        //else
        //{
        //    Debug.Log("error");
        //}
        AddReward(0.005f * _rb.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Border"))
        {
            SetReward(-10);
            Done();
        }
    }

    public override void AgentReset()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        _rb.velocity = Vector3.zero;
        _rb.isKinematic = true;
        _rb.isKinematic = false;
    }
}
