using Assets.Scripts;
using System;
using System.Collections.Generic;
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

    // Use this for initialization
    void Start () {
        var commonLevelHost = LevelCommonHostFactory.Get();

        CalculateZeroPoints();
    }

    private void CalculateZeroPoints()
    {
        mZeroFRPoint;
        mZeroBRPoint;
        mZeroFLPoint;
        mZeroBLPoint;
    }

	// Update is called once per frame
	void Update () {
		
	}

    private Vector3 mZeroFRPoint;
    private Vector3 mZeroBRPoint;
    private Vector3 mZeroFLPoint;
    private Vector3 mZeroBLPoint;

    public bool Contains(Vector3 position)
    {
#if DEBUG
        Debug.Log($"position = {position}");
#endif

        throw new NotImplementedException();
    }
}
