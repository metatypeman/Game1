﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class TstBlackBoard
    {
        public void Tst()
        {
#if UNITY_EDITOR
            Debug.Log("Begin TstBlackBoard Tst");
#endif
        }
    }
}
