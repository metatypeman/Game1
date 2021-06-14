//using MyNPCLib;
//using MyNPCLib.CGStorage;
//using MyNPCLib.Logical;
//using MyNPCLib.LogicalHostEnvironment;
//using MyNPCLib.LogicalSoundModeling;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.Scripts
//{
//    public interface IInternalBodyHumanoidHost: IInvokingInMainThread
//    {
//        IEntityLogger EntityLogger { get; }
//        void Execute(InternalTargetStateOfHumanoidController targetState);
//        InternalStatesOfHumanoidController States { get; }
//        event HumanoidStatesChangedAction OnHumanoidStatesChanged;
//        void Die();
//        event Action OnDie;
//        void SetAimCorrector(IAimCorrector corrector);
//        void SetInternalHumanoidHostContext(IInternalHumanoidHostContext intenalHostContext);
//        void CallInMainUI(Action function);
//        TResult CallInMainUI<TResult>(Func<TResult> function);
//        ICGStorage SelfHostStorage { get; }
//        IBusOfCGStorages BusOfCGStorages { get; }
//        ulong SelfEntityId { get; }
//        bool IsReady { get; }
//        event Action OnReady;
//        IList<IHostVisionObject> VisibleObjects { get; }
//        Vector3 GlobalPosition { get; }

//        event OnLogicalSoundAction OnLogicalSound;

//        INavigationRegistry NavigationRegistry { get; }
//    }
//}
