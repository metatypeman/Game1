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

    public float surfaceOffset = 1.5f;
    public GameObject setTargetOn;

    private TstConcreteNPCProcessesContextWithBlackBoard mNPCProcessesContext;

    //RapidFireGun _gun;

    private InputKeyHelper mInputKeyHelper;

    public Transform Head;
    public Animator mAnim;

    public Transform GunEnd;
    private GameObject mGunBody;

    // Use this for initialization
    void Start()
    {
        mAnim = GetComponent<Animator>();

        mEnemyController = GetComponent<EnemyController>();
        

        mEnemyRayScaner = GetComponent<EnemyRayScaner>();

        mNPCProcessesContext = new TstConcreteNPCProcessesContextWithBlackBoard(mEnemyController);
        mNPCProcessesContext.RegisterInstance<INPCRayScaner>(mEnemyRayScaner);

        //_gun = GetComponentInChildren<RapidFireGun>();

        //if(_gun != null)
        //{
        //    mEnemyController.SetAimCorrector(_gun);
        //    _gun.UseDebugLine = true;
            //_gun.FireMode = FireMode.Single;
        //}

        mInputKeyHelper = new InputKeyHelper();
        mInputKeyHelper.AddListener(KeyCode.F, OnFPressAction);
        mInputKeyHelper.AddListener(KeyCode.G, OnGPressAction);
        mInputKeyHelper.AddListener(KeyCode.K, OnKPressAction);
        mInputKeyHelper.AddListener(KeyCode.N, OnNPressAction);
        mInputKeyHelper.AddListener(KeyCode.H, OnHPressAction);
        mInputKeyHelper.AddListener(KeyCode.L, OnLPressAction);
        mInputKeyHelper.AddListener(KeyCode.I, OnIPressAction);
        mInputKeyHelper.AddListener(KeyCode.P, OnPPressAction);
        mInputKeyHelper.AddListener(KeyCode.U, OnUPressAction);
        mInputKeyHelper.AddListener(KeyCode.M, OnMPressAction);
        mInputKeyHelper.AddListener(KeyCode.B, OnBPressAction);
        mInputKeyHelper.AddListener(KeyCode.J, OnJPressAction);

        mGunBody = GameObject.Find("M4A1 Sopmod");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("EnemyNPC Update");
        mInputKeyHelper.Update();
        //mGunBody.SetActive(false);
    }

    private void OnJPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnJPressAction key = {key}");

        //var render = mGunBody.GetComponentInChildren<MeshRenderer>();

        //render.enabled = false;

        mGunBody.SetActive(false);

        //Debug.Log($"EnemyNPC OnJPressAction GunEnd.forward = {GunEnd.forward}");
        //Debug.Log($"EnemyNPC OnJPressAction transform.forward = {transform.forward}");

        //var diff = Vector3.Angle(GunEnd.forward, transform.forward);

        //Debug.Log($"EnemyNPC OnJPressAction diff = {diff}");

        //var targetObj = GameObject.Find("Ethan");
        //var target = targetObj.transform;

        //var targetDir = target.position - GunEnd.position;
        //Debug.Log($"EnemyNPC OnBPressAction targetDir = {targetDir}");

        //var forward = GunEnd.forward;

        //var angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

        //Debug.Log($"EnemyNPC OnJPressAction angle = {angle}");

        //if(Mathf.Abs(angle) > 8)
        //{
        //    transform.rotation = Quaternion.Euler(0, -1 * angle * 0.9f , 0) * transform.rotation;
        //}      
    }

    private void OnBPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnBPressAction key = {key}");

        var targetGun = FindObjectOfType<RapidFireGun>();

        Debug.Log($"EnemyNPC OnBPressAction (targetGun == null) = {targetGun == null}");

        if(targetGun != null)
        {
            var instanceId = targetGun.GetInstanceID();

            Debug.Log($"EnemyNPC OnBPressAction instanceId = {instanceId}");

            var tmpProcess = new TSTTakeFromSurfaceProcess(mNPCProcessesContext, instanceId);
            tmpProcess.RunAsync();

            //mEnemyController.TstTakeRifle(targetGun);

            //_gun = targetGun;
            //mEnemyController.SetAimCorrector(_gun);
            //_gun.UseDebugLine = true;         
        }
        
        //var rightHandWPLocator = GetComponentInChildren<RightHandWPLocator>();

        //Debug.Log($"EnemyNPC OnBPressAction (rightHandWPLocator == null) = {rightHandWPLocator == null}");

        //Debug.Log($"EnemyNPC OnBPressAction GunEnd.forward = {GunEnd.forward}");
        //Debug.Log($"EnemyNPC OnBPressAction transform.forward = {transform.forward}");
        //Debug.Log($"EnemyNPC OnBPressAction Vector3.Angle(GunEnd.forward, transform.forward) = {Vector3.Angle(GunEnd.forward, transform.forward)}");

        //var targetObj = GameObject.Find("Ethan");
        //var target = targetObj.transform;

        //var targetDir = target.position - GunEnd.position;
        //Debug.Log($"EnemyNPC OnBPressAction targetDir = {targetDir}");

        //var forward = GunEnd.forward;

        //var angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

        //Debug.Log($"EnemyNPC OnBPressAction angle = {angle}");
    }

    private void OnMPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnMPressAction key = {key}");

        var tmpProcess = new TSTMoveProcess(mNPCProcessesContext);
        tmpProcess.RunAsync();
    }

    private float? TargetAngle = null;
    private float AngleSpeed = 0.5f;
    private float? InitAngle = null;

    public bool isIkActive;

    private void OnFPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnFPressAction key = {key}");

        isIkActive = false;

        var tmpProcess = new TSTHeadToForvardProcess(mNPCProcessesContext);
        tmpProcess.RunAsync();
    }

    private void OnGPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnGPressAction key = {key}");

        var currHeadRotation = Head.rotation;
        //isIkActive = true;
        //Head.rotation = Quaternion.Euler(0, 12f, 0) * currHeadRotation;

        var tmpProcess = new TSTRotateHeadProcess(mNPCProcessesContext, 12f);
        tmpProcess.RunAsync();
    }

    private void OnKPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnKPressAction key = {key}");

        TargetAngle = 30f;

        var tmpProcess = new TSTRotateProcess(mNPCProcessesContext, TargetAngle.Value);
        tmpProcess.RunAsync();
    }

    private void OnNPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnNPressAction key = {key}");

        var _gun = mNPCProcessesContext.BlackBoard.RapidFireGunProxy;

        Debug.Log($"EnemyNPC OnNPressAction _gun.IsReady = {_gun.IsReady}");

        if (_gun != null && _gun.IsReady)
        {
            _gun.UseDebugLine = true;
            _gun.TurnState = TurnState.On;
        }      
    }

    private void OnHPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnHPressAction key = {key}");
        var _gun = mNPCProcessesContext.BlackBoard.RapidFireGunProxy;
        if (_gun != null)
        {
            _gun.TurnState = TurnState.Off;
        }      
    }

    private void OnLPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnLPressAction key = {key}");        
        var tmpSimpleAimProcess = new TstSimpleAimProcess(mNPCProcessesContext);
        tmpSimpleAimProcess.RunAsync();
    }

    private void OnIPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnIPressAction key = {key}");        
        var _target = GameObject.Find("Ethan");
        var tmpTSTFireToEthanProcess = new TSTFireToEthanProcess(mNPCProcessesContext, _target.transform.position);
        tmpTSTFireToEthanProcess.RunAsync();
    }

    private void OnPPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnPPressAction key = {key}");       
        var tmpProcess = new TstRunAtOurBaseProcess(mNPCProcessesContext);
        tmpProcess.RunAsync();
    }
    
    private void OnUPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnUPressAction key = {key}");
        var tmpProcess = new TstRootProcess(mNPCProcessesContext);
        tmpProcess.RunAsync();
    }

    //void OnAnimatorIK(int layerIndex)
    //{
    //    if (isIkActive)
    //    {
    //        mAnim.SetLookAtWeight(1);
    //        mAnim.SetLookAtPosition(new Vector3(226.15f, 0, 98.96f));
    //        Head.LookAt(new Vector3(226.15f, 0, 98.96f));
    //    }    
    //}

    void OnDestroy()
    {
        //Debug.Log("OnDestroy");
        mNPCProcessesContext?.Dispose();
    }
}
