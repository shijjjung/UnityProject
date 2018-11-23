using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Photon.PunBehaviour {
    public bool inPacket = false;
    public Rigidbody rig;
    public float throwPower = 8.0f;
	// Use this for initialization
	void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        if (inPacket)
        {
            transform.localPosition = Vector3.zero;
        }
	}
    public void SetParent(Transform pos)
    {
        transform.SetParent(pos);
        inPacket = true;
    }
    private void FixedUpdate()
    {        
        if (rig.velocity.magnitude > 8)
        {
            rig.velocity = rig.velocity.normalized * throwPower;
        }
    }
    public void Throw(Vector3 dir)
    {   
        inPacket = false;
        transform.SetParent(null);
        rig.useGravity = true;
        Vector3 normalDir = dir;        
        rig.velocity = normalDir * throwPower;
    }
    [PunRPC]
    void OnThrow(Vector3 dir)
    {
        inPacket = false;
        transform.SetParent(null);
        rig.useGravity = true;
        Vector3 normalDir = dir;
        rig.velocity = normalDir * throwPower;
    }
    [PunRPC]
    void OnGravityClose()
    {
        rig.useGravity = false;
    }

    [PunRPC]
    void OnGravityOpen()
    {
        rig.useGravity = true;
    }

    [PunRPC]
    void OnMove()
    {
        rig.isKinematic = false;
    }
    [PunRPC]
    void AddSpeed()
    {
        print(throwPower + "增加");
        throwPower += 5.0f;
    }
    private void OnCollisionEnter(Collision col)
    {
        throwPower = 8.0f;
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log(col.collider.tag);
            if (col.collider.tag == "Ground")
            {
                rig.velocity = Vector3.zero;
                if (transform.position.z < 0)
                {
                    GameMaster._instance.BallOnGroundB();
                }
                else
                {
                    GameMaster._instance.BallOnGroundA();
                }

            }else if (col.collider.tag == "Player")
            {                
                Vector3 power = (transform.position- col.collider.transform.position + col.collider.transform.forward).normalized * throwPower;
                rig.velocity = power;
            }else if (col.collider.tag == "OutGround")
            {
                rig.velocity = Vector3.zero;
                if (transform.position.z < 0)
                {
                    GameMaster._instance.BallOnGroundA();
                }
                else
                {
                    GameMaster._instance.BallOnGroundB();
                   
                }

            }
        }
    }
}
