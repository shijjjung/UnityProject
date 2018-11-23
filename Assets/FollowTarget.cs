using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    [SerializeField]
    Transform _camera;
    public Vector3 bornPos;
    float _cameraDistance;
    bool _sniperMode = false;
    public Transform playerTrans;
    public float sensitivity = 15f;
    public Vector2 cameraDistanceRange;
    public float cameraDistance
    {
        get { return _cameraDistance; }
        set
        {
            _cameraDistance = value;
            _camera.localPosition = new Vector3(_camera.localPosition.x, _camera.localPosition.y, -_cameraDistance);
        }
    }
    void Start()
    {
        originalRotation = transform.rotation;
        _cameraDistance = -_camera.localPosition.z;
        bornPos = transform.position;
    }
    Quaternion originalRotation;
    // Update is called once per frame
    void Update () {

        transform.position = bornPos;
        //  Vector3 target = player.position + new Vector3(0.048f, 3.88068f, -7.4287f);
        // transform.localPosition = Vector3.Lerp(transform.localPosition, target, smoothing*Time.deltaTime);                             
        //Quaternion rotate = Quaternion.Euler(20, player.rotation.y, 0);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotate, smoothing * Time.deltaTime);
        if (playerTrans != null)
        {
            
        }
    }   
    public void SetPlayerTrans(Transform playerTrans)
    {
        this.playerTrans = playerTrans;
    }
}
