using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class YardPlane : MonoBehaviour, IPlane
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        var scaleX = transform.localScale.x / 2;

        var rightPoint = transform.position + transform.right.normalized * scaleX;

        var rX = rightPoint.x;

        var leftPoint = transform.position + transform.right.normalized * scaleX * -1;

        var lX = leftPoint.x;

        //Gizmos.DrawLine(transform.position, rightPoint);

        var scaleZ = transform.localScale.z / 2;

        var forvardPoint = transform.position + transform.forward.normalized * scaleZ;

        var fZ = forvardPoint.z;

        var backPoint = transform.position + transform.forward.normalized * scaleZ * -1;

        var bZ = backPoint.z;

        //Gizmos.DrawLine(transform.position, forvardPoint);

        var scaleY = transform.localScale.y / 2;

        var upPoint = transform.position + transform.up.normalized * scaleY;

        var uY = upPoint.y;

        var downPoint = transform.position + transform.up.normalized * scaleY * -1;

        var dY = downPoint.y;

        //Gizmos.DrawLine(transform.position, upPoint);

        var ufrPoint = new Vector3(rX, uY, fZ);
        var ubrPoint = new Vector3(rX, uY, bZ);
        var uflPoint = new Vector3(lX, uY, fZ);
        var ublPoint = new Vector3(lX, uY, bZ);

        Gizmos.DrawLine(ufrPoint, ubrPoint);
        Gizmos.DrawLine(ufrPoint, uflPoint);
        Gizmos.DrawLine(ublPoint, uflPoint);
        Gizmos.DrawLine(ublPoint, ubrPoint);

        var dfrPoint = new Vector3(rX, dY, fZ);
        var dbrPoint = new Vector3(rX, dY, bZ);
        var dflPoint = new Vector3(lX, dY, fZ);
        var dblPoint = new Vector3(lX, dY, bZ);

        Gizmos.DrawLine(dfrPoint, dbrPoint);
        Gizmos.DrawLine(dfrPoint, dflPoint);
        Gizmos.DrawLine(dblPoint, dflPoint);
        Gizmos.DrawLine(dblPoint, dbrPoint);

        Gizmos.DrawLine(dfrPoint, ufrPoint);
        Gizmos.DrawLine(dbrPoint, ubrPoint);
        Gizmos.DrawLine(dflPoint, uflPoint);
        Gizmos.DrawLine(dblPoint, ublPoint);
    }

    public void Awake()
    {
        //Debug.Log("Awake");

        mName = name;

        //var commonLevelHost = LevelCommonHostFactory.Get();

        CalculateZeroPoints();

        //commonLevelHost.HostNavigationRegistry.RegPlane(this);

        mCollider = GetComponent<Collider>();
    }

    public void CalculatePoints()
    {
        mPointsCoordsList = PointsList.Select(p => VectorsConvertor.NumericToUnity(p.Position)).Distinct().ToList();
    }

    // Use this for initialization
    //void Start ()
    //{
    //    Debug.Log("Start");
    //}

    private void CalculateZeroPoints()
    {
        var scaleX = transform.localScale.x / 2;

        var rightPoint = transform.position + transform.right.normalized * scaleX;

        var rX = rightPoint.x;

        var leftPoint = transform.position + transform.right.normalized * scaleX * -1;

        var lX = leftPoint.x;

        var scaleZ = transform.localScale.z / 2;

        var forvardPoint = transform.position + transform.forward.normalized * scaleZ;

        var fZ = forvardPoint.z;

        var backPoint = transform.position + transform.forward.normalized * scaleZ * -1;

        var bZ = backPoint.z;

        //var scaleY = transform.localScale.y / 2;

        var upPoint = transform.position;

        var uY = upPoint.y;
        //uY = 1;

        var frPoint = new Vector3(rX, uY, fZ);
        var brPoint = new Vector3(rX, uY, bZ);
        var flPoint = new Vector3(lX, uY, fZ);
        var blPoint = new Vector3(lX, uY, bZ);

        FRPoint = frPoint;
        BRPoint = brPoint;
        FLPoint = flPoint;
        BLPoint = blPoint;

#if DEBUG
        //Debug.Log($"frPoint = {frPoint}");
        //Debug.Log($"brPoint = {brPoint}");
        //Debug.Log($"flPoint = {flPoint}");
        //Debug.Log($"blPoint = {blPoint}");

        //Debug.DrawLine(mFRPoint, mBRPoint, Color.black);
        //Debug.DrawLine(mFRPoint, mFLPoint, Color.black);
        //Debug.DrawLine(mBLPoint, mFLPoint, Color.black);
        //Debug.DrawLine(mBLPoint, mBRPoint, Color.black);
#endif
    }

    // Update is called once per frame
    void Update ()
    {
#if DEBUG
        Debug.DrawLine(FRPoint, BRPoint, Color.black);
        Debug.DrawLine(FRPoint, FLPoint, Color.black);
        Debug.DrawLine(BLPoint, FLPoint, Color.black);
        Debug.DrawLine(BLPoint, BRPoint, Color.black);
#endif
    }

    public Vector3 FRPoint { get; private set; }
    public Vector3 BRPoint { get; private set; }
    public Vector3 FLPoint { get; private set; }
    public Vector3 BLPoint { get; private set; }
    public IList<IWayPoint> PointsList { get; set; } = new List<IWayPoint>();
    private List<Vector3> mPointsCoordsList;

    private Collider mCollider;

    public bool Contains(Vector3 position)
    {
#if DEBUG
        //Debug.Log($"position = {position}");
#endif

        //var localPosition = transform.InverseTransformPoint(position);

#if DEBUG
        //Debug.Log($"name = {name}");
        //Debug.Log($"mPointsCoordsList.Count = {mPointsCoordsList.Count}");
        //Debug.Log($"transform.localScale = {transform.localScale}");
#endif

        if(mPointsCoordsList.Any(p => p == position))
        {
            return true;
        }

        var result = mCollider.bounds.Contains(position);

#if DEBUG
        //Debug.Log($"result = {result}");
#endif

        return result;
    }

    private string mName;
    public string Name => mName;
}
