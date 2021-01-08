using SymOntoClay.Scriptables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SymOntoClay
{
    [AddComponentMenu("SymOntoClay/HumanoidNPC")]
    public class HumanoidNPC : MonoBehaviour
    {
        public NPCFile NPCFile;
        public HostFile HostFile;
        public string Id;

        void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Id = $"#{Guid.NewGuid().ToString("D").Replace("-", string.Empty)}";
            }
        }

        void Awake()
        {
#if DEBUG
            Debug.Log("HumanoidNPC Awake");
#endif
        }

        void Start()
        {
#if DEBUG
            Debug.Log("HumanoidNPC Start");
#endif
        }

        void Update()
        {

        }

        void Stop()
        {

        }
    }
}
