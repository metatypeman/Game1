using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class RTreeNode
    {
        public const int MAX_DEPH = 5;
        public const int MIN_SIZE = 50;

        public RTreeNode(Vector3 leftBottomPoint, Vector3 rightTopPoint)
            : this(leftBottomPoint, rightTopPoint, 1)
        {
        }

        public RTreeNode(Vector3 leftBottomPoint, Vector3 rightTopPoint, int level)
        {
#if DEBUG
            Debug.Log($"level = {level}");
            Debug.Log($"leftBottomPoint = {leftBottomPoint}");
            Debug.Log($"rightTopPoint = {rightTopPoint}");
#endif

            mPlainRect = new PlainRect(leftBottomPoint, rightTopPoint);

            //if(level > MAX_DEPH)
            //{
            //    return;
            //}

            mRect = new Vector3(rightTopPoint.x - leftBottomPoint.x, 0, rightTopPoint.z - leftBottomPoint.z);

#if DEBUG
            Debug.Log($"mRect = {mRect}");
#endif

            if(mRect.x <= MIN_SIZE && mRect.z <= MIN_SIZE)
            {
                return;
            }

            var n = level % 2;

#if DEBUG
            Debug.Log($"n = {n}");
#endif

            if (n == 0)
            {
                BornNewRectsByX(leftBottomPoint, rightTopPoint, level);
            }
            else
            {
                BornNewRectsByZ(leftBottomPoint, rightTopPoint, level);
            }
        }

        private PlainRect mPlainRect;
        private Vector3 mRect;

        private void BornNewRectsByX(Vector3 leftBottomPoint, Vector3 rightTopPoint, int level)
        {
            var halfOfX = mRect.x / 2;

#if DEBUG
            Debug.Log($"halfOfX = {halfOfX}");
#endif

            var leftBottomPoint1 = new Vector3(leftBottomPoint.x, 0, leftBottomPoint.z);
            var rightTopPoint1 = new Vector3(leftBottomPoint.x + halfOfX, 0, rightTopPoint.z);

            mChildRTreeNode1 = new RTreeNode(leftBottomPoint1, rightTopPoint1, level + 1);

            var leftBottomPoint2 = new Vector3(leftBottomPoint.x + halfOfX, 0, leftBottomPoint.z);
            var rightTopPoint2 = new Vector3(rightTopPoint.x, 0, rightTopPoint.z);

            mChildRTreeNode2 = new RTreeNode(leftBottomPoint2, rightTopPoint2, level + 1);
        }

        private void BornNewRectsByZ(Vector3 leftBottomPoint, Vector3 rightTopPoint, int level)
        {
            var halfOfZ = mRect.z / 2;

#if DEBUG
            Debug.Log($"halfOfZ = {halfOfZ}");
#endif

            var leftBottomPoint1 = new Vector3(leftBottomPoint.x, 0, leftBottomPoint.z);
            var rightTopPoint1 = new Vector3(rightTopPoint.x, 0, leftBottomPoint.z + halfOfZ);

            mChildRTreeNode1 = new RTreeNode(leftBottomPoint1, rightTopPoint1, level + 1);

            var leftBottomPoint2 = new Vector3(leftBottomPoint.x, 0, leftBottomPoint.z + halfOfZ);
            var rightTopPoint2 = new Vector3(rightTopPoint.x, 0, rightTopPoint.z);

            mChildRTreeNode2 = new RTreeNode(leftBottomPoint2, rightTopPoint2, level + 1);
        }

        private RTreeNode mChildRTreeNode1;
        private RTreeNode mChildRTreeNode2;

        public void Draw()
        {
            mPlainRect.DrawDebug();
            mChildRTreeNode1?.Draw();
            mChildRTreeNode2?.Draw();
        }
    }
}
