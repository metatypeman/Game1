using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficBarrierRedInfo : BasePassiveLogicalGameObject
{
    protected override void OnInitFacts()
    {
#if UNITY_EDITOR
        Debug.Log($"TrafficBarrierRedInfo OnInitFacts EntityId = {EntityId}");
#endif

        base.OnInitFacts();

        this["color"] = "red";
    }
}
