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

    private NPCProcessesContext mNPCProcessesContext;

    PlayerShooting _gun;

    private InputKeyHelper mInputKeyHelper;

    public Transform Head;
    public Animator mAnim;

    // Use this for initialization
    void Start()
    {
        mAnim = GetComponent<Animator>();

        mEnemyController = GetComponent<EnemyController>();

        mEnemyRayScaner = GetComponent<EnemyRayScaner>();

        mNPCProcessesContext = new NPCProcessesContext(mEnemyController);
        mNPCProcessesContext.RegisterInstance<INPCRayScaner>(mEnemyRayScaner);

        _gun = GetComponentInChildren<PlayerShooting>();
        _gun.UseDebugLine = true;
        //_gun.FireMode = FireMode.Single;

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
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("EnemyNPC Update");
        mInputKeyHelper.Update();

        //if (TargetAngle.HasValue)
        //{
        //    if (InitAngle.HasValue)
        //    {
        //        //TargetAngle = TargetAngle + AngleSpeed;

        //        transform.rotation = Quaternion.Euler(0, AngleSpeed, 0) * transform.rotation;

        //        //var diff = Vector3.Angle(transform.rotation.eulerAngles, InitRotation.Value.eulerAngles);

        //        var currY = transform.rotation.eulerAngles.y;

        //        Debug.Log($"EnemyNPC Update currY = {currY} InitAngle = {InitAngle}");

        //        var diff = System.Math.Abs(currY - InitAngle.Value);

        //        Debug.Log($"EnemyNPC Update diff = {diff}");

        //        if (System.Math.Abs(TargetAngle.Value) <= diff)
        //        {
        //            TargetAngle = null;
        //        }
        //    }
        //    else
        //    {
        //        InitAngle = transform.rotation.eulerAngles.y;
        //    }
        //}
    }

    private float? TargetAngle = null;
    private float AngleSpeed = 0.5f;
    private float? InitAngle = null;

    public bool isIkActive;

    private void OnFPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnFPressAction key = {key}");

        isIkActive = false;
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
        _gun.TurnState = TurnState.On;
    }

    private void OnHPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnHPressAction key = {key}");
        _gun.TurnState = TurnState.Off;
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
        Debug.Log("OnDestroy");
        mNPCProcessesContext?.Dispose();
    }
}
