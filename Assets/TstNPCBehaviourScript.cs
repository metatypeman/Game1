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

        var forvard = transform.forward;

#if DEBUG
        Debug.Log($"TstNPCBehaviourScript Start forvard = {forvard}");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
