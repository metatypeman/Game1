using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TstNPCBehaviourScript : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        var cube1 = GameObject.Find("Cube_1");

        //var position = cube1.transform.position;

#if DEBUG
        //Debug.Log($"TstNPCBehaviourScript Start position = {position}");
#endif

        //_navMeshAgent.SetDestination(position);

        //        var forvard = transform.forward;

        //#if DEBUG
        //        Debug.Log($"TstNPCBehaviourScript Start forvard = {forvard}");
        //#endif

        //        var normalizedForvard = forvard.normalized;

        //#if DEBUG
        //        Debug.Log($"TstNPCBehaviourScript Start normalizedForvard = {normalizedForvard}");
        //#endif

        //        var distance = 5;

        //        var newLocalPosition = new Vector3(normalizedForvard.x * distance, normalizedForvard.y, normalizedForvard.z);

        //#if DEBUG
        //        Debug.Log($"TstNPCBehaviourScript Start newLocalPosition = {newLocalPosition}");
        //#endif

        //        var newGlobalPosition = transform.TransformVector(newLocalPosition);

        //#if DEBUG
        //        Debug.Log($"TstNPCBehaviourScript Start newGlobalPosition = {newGlobalPosition}");
        //#endif

        //        cube1.transform.position = newGlobalPosition;

        //var angle = 45;
        var angle = 0;
        var distance = 5;

        var radAngle = angle * Mathf.Deg2Rad;
        var x = Mathf.Sin(radAngle);
        var y = Mathf.Cos(radAngle);
        var localDirection = new Vector3(x * distance, 0f, y * distance);

#if DEBUG
        Debug.Log($"TstNPCBehaviourScript Start localDirection = {localDirection}");
#endif

        var globalDirection = transform.TransformDirection(localDirection);

        var newPosition = globalDirection + transform.position;

        //cube1.transform.position = newPosition;
        _navMeshAgent.SetDestination(newPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
