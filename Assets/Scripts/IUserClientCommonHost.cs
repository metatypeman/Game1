using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IUserClientCommonHost
    {
        float GetAxis(string name);
        bool GetKeyUp(KeyCode key);
        bool GetMouseButtonUp(int button);
        UserClientMode UserClientMode { get; set; }
    }
}
