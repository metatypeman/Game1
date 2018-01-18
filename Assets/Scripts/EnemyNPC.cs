using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(EnemyController))]
public class EnemyNPC : MonoBehaviour {
    private EnemyController mEnemyController;
    //GameObject mTargetCube;
    //private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    public float surfaceOffset = 1.5f;
    public GameObject setTargetOn;
    private int n = 0;
    private int maxN = 3;

    // Use this for initialization
    void Start () {
        mEnemyController = GetComponent<EnemyController>();
        mEnemyController.OnAchieveDestinationOfMoving += () => {
            if(n < maxN)
            {
                Debug.Log($"EnemyController Start goal {n} had achieved!!!!");
                RunToTarget();
            }
        };
        //m_Cam = Camera.main.transform;
        //mTargetCube = GameObject.Find("Cube_1");
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("EnemyController Update");
    }

    void OnWillRenderObject()
    {
        Debug.Log("EnemyController OnWillRenderObject");
    }

    private bool mIsPPressed;

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        //Debug.Log("EnemyController FixedUpdate");

        //var crouch = Input.GetKey(KeyCode.C);

        //var h = CrossPlatformInputManager.GetAxis("Horizontal");
        //var v = CrossPlatformInputManager.GetAxis("Vertical");

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        transform.position = hit.point + hit.normal * surfaceOffset;
        //        if (setTargetOn != null)
        //        {
        //            setTargetOn.SendMessage("SetTarget", transform);
        //        }
        //    }
        //}

        //if (h > 0.1 || h < -0.1 || v > 0.1 || v < -0.1)
        //{
        //    Debug.Log("EnemyController FixedUpdate h = " + h + " v = " + v);

        //    //var m_Move = v * Vector3.forward + h * Vector3.right;
        //    var m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        //    m_Move = v * m_CamForward + h * m_Cam.right;
        //    //m_Move *= 0.5f;

        //    Debug.Log("EnemyController FixedUpdate m_Move = " + m_Move);

        //    var transformGlobalPosition = transform.position;

        //    Debug.Log("EnemyController FixedUpdate (2) transformGlobalPosition = " + transformGlobalPosition);

        //    var newPosition = transformGlobalPosition + m_Move;

        //    Debug.Log("EnemyController FixedUpdate newPosition = " + newPosition);

        //    mEnemyController.Move(m_Move);
        //}

        //var mMove = transform.TransformDirection(transform.position) + m_Move;

        //Debug.Log("EnemyController FixedUpdate (2) m_Move = " + m_Move);

        //mEnemyController.Move(m_Move);

        var goAhead = Input.GetKey(KeyCode.W);

        if(goAhead)
        {
            //mEnemyController.HState = HumanoidHState.Walk;
        }
        else
        {
            var goToTarget = Input.GetKey(KeyCode.P);

            if(goToTarget)
            {
                //mEnemyController.HState = HumanoidHState.Walk;
                //mEnemyController.Move(mTargetCube.transform.position);

                if(!mIsPPressed)
                {
                    mIsPPressed = true;

                    n = 0;
                    RunToTarget();

                    //var moveCommand = new HumanoidHStateCommand();
                    //moveCommand.State = HumanoidHState.Walk;
                    //moveCommand.TargetPosition = mTargetCube.transform.position;

                    //mEnemyController.Execute(moveCommand);
                }
            }
            else
            {
                var gth = Input.GetKey(KeyCode.T);
               
                if(gth)
                {
                    //mEnemyController.Stop();
                }
                else
                {
                    //mEnemyController.HState = HumanoidHState.Stop;
                }            
            }
        }   
    }

    private void RunToTarget()
    {
        if(n >= maxN)
        {
            return;
        }

        n++;

        var targetName = $"Cube_{n}";

        Debug.Log($"EnemyController RunToTarget targetName = {targetName}");

        var target = GameObject.Find(targetName);

        var moveCommand = new HumanoidHStateCommand();
        moveCommand.State = HumanoidHState.Walk;
        moveCommand.TargetPosition = target.transform.position;

        mEnemyController.Execute(moveCommand);
    }

    void OnAnimatorIK(int layerIndex)
    {
    }
}
