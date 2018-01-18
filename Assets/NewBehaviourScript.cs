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
        //Debug.Log("NewBehaviourScript Update");
    }

    void OnBecameVisible()
    {
        //Debug.Log("NewBehaviourScript OnBecameVisible");
    }

    void OnBecameInvisible()
    {
        //Debug.Log("NewBehaviourScript OnBecameInvisible");
    }

    void OnWillRenderObject()
    {
        //var currentCamera = Camera.current;
        //Debug.Log($"NewBehaviourScript OnWillRenderObject currentCamera.name = '{currentCamera.name}' currentCamera.GetInstanceID() = {currentCamera.GetInstanceID()} Time.deltaTime = {Time.deltaTime}");
    }

    private void FixedUpdate()
    {
        //Debug.Log("NewBehaviourScript FixedUpdate");
    }

    private void LateUpdate()
    {
        //Debug.Log("NewBehaviourScript LateUpdate");
    }
}
