using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCamHelper : MonoBehaviour {
    public float speed = 3.0F;
    public float rotateSpeed = 3.0F;
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var camera = Camera.current.transform;

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        var yRot = Input.GetAxis("Mouse X") * XSensitivity;
        var xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        var m_CameraTargetRot = camera.localRotation;
        m_CameraTargetRot *= Quaternion.Euler(0f, yRot, 0f);//Quaternion.Euler(-xRot, yRot, 0f);
        camera.localRotation = m_CameraTargetRot;

        Debug.Log($"Update h = {h} v = {v}");

        var newPosition = new Vector3(camera.position.x + h, camera.position.y, camera.position.z + v);
        camera.position = newPosition;
    }

    // Fixed update is called in sync with physics
    //private void FixedUpdate()
    //{
    //    var h = Input.GetAxis("Horizontal");
    //    var v = Input.GetAxis("Vertical");
    //    var yRot = Input.GetAxis("Mouse X") * XSensitivity;
    //    var xRot = Input.GetAxis("Mouse Y") * YSensitivity;

    //    Debug.Log($"FixedUpdate h = {h} v = {v} yRot = {yRot} xRot = {xRot}");

    //    //float horizontal = Input.GetAxis("Horizontal");
    //    //float vertical = Input.GetAxis("Vertical");
    //    //CharacterController controller = GetComponent<CharacterController>();
    //    //Camera.current.transform.Rotate(0, horizontal * rotateSpeed, 0);
    //    //Vector3 forward = transform.TransformDirection(Vector3.forward);
    //    //float curSpeed = speed * vertical;
    //    //controller.SimpleMove(forward * curSpeed);
    //    //Camera.current.transform.position.x = Camera.current.transform.position.x + speed;

    //    //if (Input.GetButtonDown("Fire1"))
    //    //    GetComponent<Animator>().SetBool("Fire", true);
    //    //else if (Input.GetButtonUp("Fire1"))
    //    //    GetComponent<Animator>().SetBool("Fire", false);
    //}
}
