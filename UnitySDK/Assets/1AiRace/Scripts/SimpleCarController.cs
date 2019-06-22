using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{

    public void SetInput(float horizontal)
    {
        
        m_horizontalInput = horizontal;//Input.GetAxis("Horizontal");
        m_verticalInput = vertical; //Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;
    }

    private void Accelerate()
    {
        /*float velocity = rigidBody.velocity.x;
        float brake = 0;

        if ((velocity > 0.01 && m_verticalInput < 0) || (velocity < -0.01 && m_verticalInput > 0))
        {
            brake = brakeForce;
            if (Mathf.Abs(velocity) < 0.05)
            {
                rigidBody.velocity = Vector3.zero;
                velocity = 0;
            }
        }
        frontDriverW.brakeTorque = Mathf.Abs(m_verticalInput) * brake;
        frontPassengerW.brakeTorque = Mathf.Abs(m_verticalInput) * brake;
        rearDriverW.brakeTorque = Mathf.Abs(m_verticalInput) * brake;
        rearPassengerW.brakeTorque = Mathf.Abs(m_verticalInput) * brake; //Тормоз


        if (m_verticalInput != 0 && Mathf.Abs(velocity) < 0.05 && !isStartCourutine)
        {
            StartCoroutine(BackPause());
            isStartCourutine = true;
            isForward = false;
            Debug.Log("сщгкгешту");
        } //Изменить направление после полной остановки

        if (isForward)
        {
            frontDriverW.motorTorque = m_verticalInput * motorForce;
            frontPassengerW.motorTorque = m_verticalInput * motorForce;
        }*/
        float brake = 0;
    
        if ( m_verticalInput < 0) 
        {
            brake = brakeForce;
        }

        frontDriverW.brakeTorque = brake;
        frontPassengerW.brakeTorque = brake;
        rearDriverW.brakeTorque = brake;
        rearPassengerW.brakeTorque = brake;

        rearDriverW.motorTorque = m_verticalInput * motorForce;
        rearPassengerW.motorTorque = m_verticalInput * motorForce;
        frontDriverW.motorTorque = m_verticalInput * motorForce;
        frontPassengerW.motorTorque = m_verticalInput * motorForce;
    }

    //private IEnumerator BackPause()
    //{
    //    yield return new WaitForSeconds(1f);
    //    isForward = true;
    //    yield return new WaitForSeconds(0.1f);
    //    isStartCourutine = false;
    //}

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    public float m_horizontalInput;
    public float m_verticalInput;
    private float m_steeringAngle;

    private bool isForward = false, isStartCourutine = false;

    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public float maxSteerAngle = 30;
    public float motorForce = 50;
    public float brakeForce;
    public float vertical=0.5f;
    public Rigidbody rigidBody;
}
