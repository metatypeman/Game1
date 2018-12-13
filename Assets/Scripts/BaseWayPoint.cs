using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class BaseWayPoint: BasePassiveLogicalGameObject
    {
        protected BaseWayPoint()
            : base (new PassiveLogicalGameObjectOptions() {
                ShowGlobalPosition = true
            })
        {
        }

        protected override void OnInitFacts()
        {
            base.OnInitFacts();

            //this["class"] = "waypoint";
            this["class"] = "place";
        }
    }
}
