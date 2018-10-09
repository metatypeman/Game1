using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class NPCBodyResourcesResolution: BaseNPCResourcesResolution
    {
        public override NPCResourceKind Kind => NPCResourceKind.Body;
        public TargetStateOfHumanoidBody TargetState { get; set; }
        public DisagreementByHStateInfo DisagreementByHState { get; set; }
        public DisagreementByTargetPositionInfo DisagreementByTargetPosition { get; set; }
        public DisagreementByVStateInfo DisagreementByVState { get; set; }
        public DisagreementByHandsStateInfo DisagreementByHandsState { get; set; }
        public DisagreementByHandsActionStateInfo DisagreementByHandsActionState { get; set; }
        public DisagreementByHeadStateInfo DisagreementByHeadState { get; set; }
        public DisagreementByTargetHeadPositionInfo DisagreementByTargetHeadPosition { get; set; }

        public override NPCBodyResourcesResolution ToBodyResourcesResulution()
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

            return this;
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));

            if (TargetState == null)
            {
                sb.AppendLine($"{spaces}{nameof(TargetState)} = null");
            }
            else
            {
                sb.Append(TargetState.ToString(nextN));
            }

            if (DisagreementByHState == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByHState)} = null");
            }
            else
            {
                sb.Append(DisagreementByHState.ToString(nextN));
            }

            if (DisagreementByTargetPosition == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByTargetPosition)} = null");
            }
            else
            {
                sb.Append(DisagreementByTargetPosition.ToString(nextN));
            }

            if (DisagreementByVState == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByVState)} = null");
            }
            else
            {
                sb.Append(DisagreementByVState.ToString(nextN));
            }

            if (DisagreementByHandsState == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByHandsState)} = null");
            }
            else
            {
                sb.Append(DisagreementByHandsState.ToString(nextN));
            }

            if (DisagreementByHandsActionState == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByHandsActionState)} = null");
            }
            else
            {
                sb.Append(DisagreementByHandsActionState.ToString(nextN));
            }

            if (DisagreementByHeadState == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByHeadState)} = null");
            }
            else
            {
                sb.Append(DisagreementByHeadState.ToString(nextN));
            }

            if (DisagreementByTargetHeadPosition == null)
            {
                sb.AppendLine($"{spaces}{nameof(DisagreementByTargetHeadPosition)} = null");
            }
            else
            {
                sb.Append(DisagreementByTargetHeadPosition.ToString(nextN));
            }

            return sb.ToString();
        }
    }
}
