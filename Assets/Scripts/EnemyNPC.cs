using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyRayScaner))]
public class EnemyNPC : MonoBehaviour
{
    private EnemyController mEnemyController;
    private EnemyRayScaner mEnemyRayScaner;
    //GameObject mTargetCube;
    //private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    //private Vector3 m_CamForward;             // The current forward direction of the camera
    //private Vector3 m_Move;
    public float surfaceOffset = 1.5f;
    public GameObject setTargetOn;
    private int n = 0;
    private int maxN = 3;
    private NPCProcessesContext mNPCProcessesContext;

    PlayerShooting _gun;

    // Use this for initialization
    void Start()
    {
        mEnemyController = GetComponent<EnemyController>();
        //mEnemyController.OnAchieveDestinationOfMoving += () => {
        //if(mIsQPressed)
        //{
        //    mIsQPressed = false;
        //    return;
        //}

        //if(n < maxN)
        //{
        //    Debug.Log($"EnemyController Start goal {n} had achieved!!!!");
        //    RunToTarget();
        //}
        //};

        mEnemyRayScaner = GetComponent<EnemyRayScaner>();
        //m_Cam = Camera.main.transform;
        //mTargetCube = GameObject.Find("Cube_1");

        mNPCProcessesContext = new NPCProcessesContext(mEnemyController);
        mNPCProcessesContext.RegisterInstance<INPCRayScaner>(mEnemyRayScaner);

        _gun = GetComponentInChildren<PlayerShooting>();
        _gun.FireMode = FireMode.Single;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("EnemyNPC Update");
    }

    private bool mIsPPressed;
    private bool mIsTPressed;
    private bool mIsQPressed;
    private bool mIsUPressed;
    private bool mIsEnter;
    private bool mIsH;
    private bool mIsL;

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

        var isEnter = Input.GetKey(KeyCode.N);

        if (isEnter)
        {
            if (!mIsEnter)
            {
                mIsEnter = true;
                mIsH = false;
                _gun.TurnState = TurnState.On;
            }
        }

        var isH = Input.GetKey(KeyCode.H);

        if(isH)
        {
            if(!mIsH)
            {
                mIsH = true;
                mIsEnter = false;
                _gun.TurnState = TurnState.Off;
            }
        }

        var isL = Input.GetKey(KeyCode.L);

        if (isL)
        {
            if(!mIsL)
            {
                mIsL = true;
                mEnemyController.TmpAim();
            }
        }

        var goAhead = Input.GetKey(KeyCode.W);

        if (goAhead)
        {
            //mEnemyController.HState = HumanoidHState.Walk;
        }
        else
        {
            var goToTarget = Input.GetKey(KeyCode.P);

            if (goToTarget)
            {
                //mEnemyController.HState = HumanoidHState.Walk;
                //mEnemyController.Move(mTargetCube.transform.position);

                if (!mIsPPressed)
                {
                    mIsPPressed = true;

                    var tmpProcess = new TstRunAtOurBaseProcess(mNPCProcessesContext);
                    tmpProcess.RunAsync();

                    //n = 0;
                    //RunToTarget();

                    //var moveCommand = new HumanoidHStateCommand();
                    //moveCommand.State = HumanoidHState.Walk;
                    //moveCommand.TargetPosition = mTargetCube.transform.position;

                    //mEnemyController.Execute(moveCommand);
                }
            }
            else
            {
                var gth = Input.GetKey(KeyCode.T);

                if (gth)
                {
                    if (!mIsTPressed)
                    {
                        mIsTPressed = true;

                        DisplayVisibleItems();
                    }

                    //mEnemyController.Stop();
                }
                else
                {
                    var isQKey = Input.GetKey(KeyCode.Q);

                    if (isQKey)
                    {
                        if (!mIsQPressed)
                        {
                            mIsQPressed = true;

                            GoToFarWaypoint();
                        }
                    }
                    else
                    {
                        var isUPressed = Input.GetKey(KeyCode.U);

                        if (isUPressed)
                        {
                            if (!mIsUPressed)
                            {
                                mIsUPressed = true;

                                //var tmpNPCThreadSafeMeshController = new NPCThreadSafeMeshController(mEnemyController);

                                //var target = GameObject.Find("Cube_1");

                                //var targetPoint = target.transform.position;

                                var tmpProcess = new TstRootProcess(mNPCProcessesContext);
                                tmpProcess.RunAsync();

                                //                                var moveCommand = new HumanoidHStateCommand()
                                //                                {
                                //                                    TaskId = 12,
                                //                                    State = HumanoidHState.Walk,
                                //                                    //TargetPosition = targetWayPoint.Position
                                //                                    TargetPosition = targetPoint
                                //                                };

                                //#if UNITY_EDITOR
                                //                                Debug.Log($"TstGoToEnemyBaseProcess moveCommand = {moveCommand}");
                                //#endif
                                //                                Task.Run(() =>
                                //                                {
                                //                                    var tmpTask = tmpNPCThreadSafeMeshController.Execute(moveCommand);

                                //#if UNITY_EDITOR
                                //                                    Debug.Log($"TstGoToEnemyBaseProcess tmpTask = {tmpTask}");
                                //#endif
                                //                                });


                                //var targetWayPoint = WaypointsBus.GetByTag("enemy military base");



                                //                                if (targetWayPoint != null)
                                //                                {
                                //                                    var moveCommand = new HumanoidHStateCommand()
                                //                                    {
                                //                                        TaskId = 12,
                                //                                        State = HumanoidHState.Walk,
                                //                                        TargetPosition = targetWayPoint.Position
                                //                                    };

                                //#if UNITY_EDITOR
                                //                                    Debug.Log($"TstGoToEnemyBaseProcess moveCommand = {moveCommand}");
                                //#endif
                                //                                    Task.Run(() => {
                                //                                        var tmpTask = tmpNPCThreadSafeMeshController.Execute(moveCommand);

                                //#if UNITY_EDITOR
                                //                                        Debug.Log($"TstGoToEnemyBaseProcess tmpTask = {tmpTask}");
                                //#endif
                                //                                    });
                                //                                }

                                //var tmpNPCProcessesContext = new NPCProcessesContext();
                                //var tmpTstRootProcess = new TstRootProcess(mEnemyController, tmpNPCProcessesContext);
                                //tmpTstRootProcess.RunAsync();
                            }
                        }
                    }
                    //mEnemyController.HState = HumanoidHState.Stop;
                }
            }
        }
    }

    private void GoToFarWaypoint()
    {
        var targetWayPoint = WaypointsBus.GetByTag("enemy military base");

        if (targetWayPoint != null)
        {
            var moveCommand = new HumanoidHStateCommand();
            moveCommand.State = HumanoidHState.Walk;
            moveCommand.TargetPosition = targetWayPoint.Position;

            //mEnemyController.Execute(moveCommand);
        }
    }

    private void DisplayVisibleItems()
    {
        ////var visibleItemsList = mEnemyRayScaner.VisibleItems;

        //if (visibleItemsList.Count == 0)
        //{
        //    Debug.Log("EnemyNPC DisplayVisibleItems visibleItemsList.Count == 0");
        //    mIsTPressed = false;
        //    return;
        //}

        //foreach (var visibleItem in visibleItemsList)
        //{
        //    Debug.Log($"EnemyNPC DisplayVisibleItems visibleItem = {visibleItem}");

        //    var gameInfo = MyGameObjectsBus.GetObject(visibleItem.InstanceID);

        //    Debug.Log($"EnemyNPC DisplayVisibleItems gameInfo = {gameInfo}");
        //}

        //mIsTPressed = false;
    }

    private void RunToTarget()
    {
        if (n >= maxN)
        {
            mIsPPressed = false;
            n = 0;
            return;
        }

        n++;

        var targetName = $"Cube_{n}";

        Debug.Log($"EnemyNPC RunToTarget targetName = {targetName}");

        var target = GameObject.Find(targetName);

        var moveCommand = new HumanoidHStateCommand();
        moveCommand.State = HumanoidHState.Walk;
        moveCommand.TargetPosition = target.transform.position;

        //mEnemyController.Execute(moveCommand);
    }

    void OnAnimatorIK(int layerIndex)
    {
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy");
        mNPCProcessesContext?.Dispose();
    }
}
