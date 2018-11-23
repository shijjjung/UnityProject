using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMove : Photon.PunBehaviour
{ 
    private Animator anim;
    [SerializeField]
    Transform cameraPoint;   
    public float speed = 3.0f;    
    Rigidbody _rigidbody;
    public float JumpRate = 1.5f;
    public bool isJump = false;
    public float timer = 0.0f;
   
    [SerializeField]
    public GameObject Pos;
    public new Rigidbody rigidbody
    {
        get
        {
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
            return _rigidbody;
        }
    }
    
    private void Awake()
    {
        Camera.main?.gameObject.SetActive(false);
    }
    
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ////額外資訊同步更新
        //if (stream.isWriting)
        //{
        //   stream.SendNext(point);//1                      
        //}
        //else
        //{
        //point = (int)stream.ReceiveNext();   
        //    GameMaster._instance.SetScore(point, (int)stream.ReceiveNext());            
        //}
    }
    void Start () {    
        if (photonView.isMine)
        {
            timer = JumpRate;            
              anim = GetComponent<Animator>();
            cameraPoint.GetComponent<FollowTarget>().SetPlayerTrans(gameObject.transform);

        }
        else
        {
            cameraPoint.gameObject.SetActive(false);            
            this.enabled = false;
        }
    }
    //[PunRPC]
    //void Gain(string pos)
    //{
    //    if (photonView.isMine)
    //    {
    //        print(Pos.name);
    //        if (Pos.name == pos)
    //        {
    //            print("皮卡丘分數++");
    //            point++;
    //        }
    //    }
    //}
    // Update is called once per frame
    void Update() {               
        if (photonView.isMine)
        {
            isJump = false;
             timer += Time.deltaTime;
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if(v!=0 || h != 0)
            {
                anim.SetBool("Walk", true);
                anim.SetFloat("MoveSpeed", 0.084f);
                if (h > 0)
                {
                    anim.SetBool("Right", true);
                    anim.SetBool("Left", false);
                }
                else if (h < 0)
                {
                    anim.SetBool("Left", true);
                    anim.SetBool("Right", false);
                }
                else
                {
                    anim.SetBool("Left", false);
                    anim.SetBool("Right", false);
                }
                anim.SetBool("Front", false);
                if (v < 0)
                {
                    anim.SetBool("WalkBack", true);                   
                }                
                else
                {
                    anim.SetBool("WalkBack", false);                 
                }
                if (v > 0)
                {
                    anim.SetBool("Front", true);
                }
            }
            else
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Left", false);
                anim.SetBool("Right", false);
                anim.SetBool("WalkBack", false);
            }
            transform.localPosition += transform.forward * v * Time.deltaTime * 3.0f;
            transform.localPosition += transform.right * h * Time.deltaTime * 3.0f;
            
            if (timer >= JumpRate && Input.GetButtonDown("Jump"))
            {
                rigidbody.velocity = transform.up * speed * 3.56f * Time.deltaTime;
                anim.SetTrigger("Jump");
                isJump = true;
                 timer = 0;
            }                                        
        }      
        #region Rigid Body +Collider的移動
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        //if (h != 0)
        //{
        //    anim.SetFloat("MoveSpeed", Mathf.Abs(h));
        //}
        //if (v != 0)
        //{
        //    anim.SetFloat("MoveSpeed", Mathf.Abs(v));
        //}
        //if (h != 0 && v != 0)
        //{
        //    anim.SetFloat("MoveSpeed", 1);
        //}
        //if (h < 0)
        //{
        //    transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        //    if (v < 0)
        //    {
        //        //左下
        //        transform.rotation = Quaternion.Euler(0f, -135f, 0f);
        //    }
        //    else if (v > 0)
        //    {
        //        //左上
        //        transform.rotation = Quaternion.Euler(0f, -45f, 0f);
        //    }
        //}
        //else if (h > 0)
        //{
        //    transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        //    if (v < 0)
        //    {
        //        //右下
        //        transform.rotation = Quaternion.Euler(0f, 135f, 0f);
        //    }
        //    else if (v > 0)
        //    {
        //        //右上
        //        transform.rotation = Quaternion.Euler(0f, 45f, 0f);
        //    }
        //}
        //else if (v < 0)
        //{
        //    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        //}
        //else if (v > 0)
        //{
        //    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //}

        //transform.position += new Vector3(h, 0, v) * speed * Time.deltaTime;
        #endregion
    }

    public void OnCollisionEnter(Collision ball)
    {      
        if (ball.collider.tag == "Ball" && isJump)
        {
            isJump = false;
            print(anim.GetCurrentAnimatorStateInfo(0).IsName("jump-up"));
            print(anim.GetCurrentAnimatorStateInfo(0).IsName("jump-down"));
            ball.gameObject.GetComponent<Ball>().photonView.RPC("AddSpeed", PhotonTargets.All);
        }
    }
}
