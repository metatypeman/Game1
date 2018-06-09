using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        //LogInstance.Log("Begin");
    }

    void OnBecameVisible()
    {
        //LogInstance.Log("Begin");
    }

    void OnBecameInvisible()
    {
        //LogInstance.Log("Begin");
    }

    void OnWillRenderObject()
    {
        //var currentCamera = Camera.current;
        //LogInstance.Log($"currentCamera.name = '{currentCamera.name}' currentCamera.GetInstanceID() = {currentCamera.GetInstanceID()} Time.deltaTime = {Time.deltaTime}");
    }

    private void FixedUpdate()
    {
        //LogInstance.Log("Begin");
    }

    private void LateUpdate()
    {
        //LogInstance.Log("Begin");
    }
}
