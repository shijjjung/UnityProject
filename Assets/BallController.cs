using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : Photon.PunBehaviour
{
    public Transform BallTrans;    
    private bool HaveBall  =false;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine)
        {
            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    photonView.RPC("OnHit", PhotonTargets.All);
               
            //}
        }
    }
    

    private void OnCollisionEnter(Collision col)
    {       
        if (photonView.isMine)
        {
            if (col.collider.tag == "Ball")
            {
                print("碰到球");
              // col.collider.GetComponent<Rigidbody>().velocity = -col.collider.GetComponent<Rigidbody>().velocity*10;

                
                //ball.photonView.RPC("OnThrow", PhotonTargets.All,shootdir);
                //BallTrans.GetComponentInChildren<Ball>().Throw(shootdir);

                //        col.gameObject.GetComponent<Ball>().SetParent(BallTrans);
                //        col.gameObject.GetComponent<Rigidbody>().useGravity = false;
                //        HaveBall = true;
            }
        }
    }
}
