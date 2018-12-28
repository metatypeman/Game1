using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlainRect
    {
        public PlainRect(Vector3 leftBottomPoint, Vector3 rightTopPoint)
        {
            mLeftBottomPoint = leftBottomPoint;

#if DEBUG
            Debug.Log($"mLeftBottomPoint = {mLeftBottomPoint}");
#endif

            mLeftTopPoint = new Vector3(leftBottomPoint.x, 0, rightTopPoint.z);

#if DEBUG
            Debug.Log($"mLeftTopPoint = {mLeftTopPoint}");
#endif

            mRightTopPoint = rightTopPoint;

#if DEBUG
            Debug.Log($"mRightTopPoint = {mRightTopPoint}");
#endif

            mRightBottomPoint = new Vector3(rightTopPoint.x, 0, leftBottomPoint.z);

#if DEBUG
            Debug.Log($"mRightBottomPoint = {mRightBottomPoint}");
#endif
        }

        private Vector3 mLeftBottomPoint;
        private Vector3 mLeftTopPoint;
        private Vector3 mRightBottomPoint;
        private Vector3 mRightTopPoint;

        public void DrawDebug()
        {
            Debug.DrawLine(mLeftBottomPoint, mLeftTopPoint, Color.red);
            Debug.DrawLine(mLeftTopPoint, mRightTopPoint, Color.red);
            Debug.DrawLine(mLeftBottomPoint, mRightBottomPoint, Color.red);
            Debug.DrawLine(mRightTopPoint, mRightBottomPoint, Color.red);
        }
    }
}
