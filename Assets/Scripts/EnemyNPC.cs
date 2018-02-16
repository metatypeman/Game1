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

    // Use this for initialization
    void Start()
    {
        mEnemyController = GetComponent<EnemyController>();

        mEnemyRayScaner = GetComponent<EnemyRayScaner>();

        mNPCProcessesContext = new NPCProcessesContext(mEnemyController);
        mNPCProcessesContext.RegisterInstance<INPCRayScaner>(mEnemyRayScaner);

        _gun = GetComponentInChildren<PlayerShooting>();
        _gun.UseDebugLine = true;
        //_gun.FireMode = FireMode.Single;

        mInputKeyHelper = new InputKeyHelper();
        mInputKeyHelper.AddListener(KeyCode.K, OnKPressAction);
        mInputKeyHelper.AddListener(KeyCode.N, OnNPressAction);
        mInputKeyHelper.AddListener(KeyCode.H, OnHPressAction);
        mInputKeyHelper.AddListener(KeyCode. ,);
        mInputKeyHelper.AddListener(KeyCode. ,);
        mInputKeyHelper.AddListener(KeyCode. ,);
        mInputKeyHelper.AddListener(KeyCode. ,);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("EnemyNPC Update");
        mInputKeyHelper.Update();
    }

    private void OnKPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnKPressAction key = {key}");
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

    private void OnPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC  key = {key}");
    }

    private void OnPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC  key = {key}");
    }

    private void OnPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC  key = {key}");
    }
    
    private void OnPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC  key = {key}");
    }
    
    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        //Debug.Log("EnemyController FixedUpdate");

        var isL = Input.GetKey(KeyCode.L);

        if (isL)
        {
            if(!mIsL)
            {
                mIsL = true;

                var tmpSimpleAimProcess = new TstSimpleAimProcess(mNPCProcessesContext);
                tmpSimpleAimProcess.RunAsync();
            }
        }

        var isI = Input.GetKey(KeyCode.I);

        if (isI)
        {
            if(!mIsI)
            {
                mIsI = true;

                var _target = GameObject.Find("Ethan");
                var tmpTSTFireToEthanProcess = new TSTFireToEthanProcess(mNPCProcessesContext, _target.transform.position);
                tmpTSTFireToEthanProcess.RunAsync();
            }
        }
        
        {
            var goToTarget = Input.GetKey(KeyCode.P);

            if (goToTarget)
            {
                if (!mIsPPressed)
                {
                    mIsPPressed = true;

                    var tmpProcess = new TstRunAtOurBaseProcess(mNPCProcessesContext);
                    tmpProcess.RunAsync();
                }
            }
            else
            {

                    var isQKey = Input.GetKey(KeyCode.Q);

                    if (isQKey)
                    {
                    }
                    else
                    {
                        var isUPressed = Input.GetKey(KeyCode.U);

                        if (isUPressed)
                        {
                            if (!mIsUPressed)
                            {
                                mIsUPressed = true;

                                var tmpProcess = new TstRootProcess(mNPCProcessesContext);
                                tmpProcess.RunAsync();
                            }
                        }
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy");
        mNPCProcessesContext?.Dispose();
    }
}
