using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public abstract class BaseCommonNPCProcessWithEvents : INPCProcess
    {
        protected BaseCommonNPCProcessWithEvents()
        {
        }

        protected BaseCommonNPCProcessWithEvents(IEntityLogger entityLogger)
        {
            mEntityLogger = entityLogger;
        }

        private readonly object mEntityLoggerLockObj = new object();
        private IEntityLogger mEntityLogger;

        public IEntityLogger EntityLogger
        {
            get
            {
                lock (mEntityLoggerLockObj)
                {
                    return mEntityLogger;
                }
            }

            set
            {
                lock (mEntityLoggerLockObj)
                {
                    if(mEntityLogger == value)
                    {
                        return;
                    }

                    mEntityLogger = value;
                }

                OnSetEntityLogger();
            }
        }

        protected virtual void OnSetEntityLogger()
        {
        }

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            lock(mEntityLoggerLockObj)
            {
                mEntityLogger?.Log(message);
            }         
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            lock (mEntityLoggerLockObj)
            {
                mEntityLogger?.Error(message);
            }            
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            lock (mEntityLoggerLockObj)
            {
                mEntityLogger?.Warning(message);
            }           
        }

        public NPCProcessStartupMode StartupMode { get; set; }

        protected readonly object StateLockObj = new object();
        protected StateOfNPCProcess mState = StateOfNPCProcess.Created;

        public abstract StateOfNPCProcess State { get; set; }

        protected bool StateTransitionChecker(StateOfNPCProcess currentState, StateOfNPCProcess newState)
        {
            if (currentState == newState)
            {
                return false;
            }

            if (currentState == StateOfNPCProcess.RanToCompletion)
            {
                return false;
            }

            if (currentState == StateOfNPCProcess.Canceled)
            {
                return false;
            }

            if (currentState == StateOfNPCProcess.Faulted)
            {
                return false;
            }

            if (currentState == StateOfNPCProcess.Destroyed)
            {
                return false;
            }

            return true;
        }

        protected void EmitChangingOfState(StateOfNPCProcess state)
        {
            EmitOnStateChanged(state);

            switch (state)
            {
                case StateOfNPCProcess.Created:
                    break;

                case StateOfNPCProcess.Running:
                    EmitOnRunningChanged();
                    break;

                case StateOfNPCProcess.RanToCompletion:
                    EmitOnRanToCompletionChanged();
                    break;

                case StateOfNPCProcess.Canceled:
                    EmitOnCanceledChanged();
                    break;

                case StateOfNPCProcess.Faulted:
                    EmitOnFaultedChanged();
                    break;

                case StateOfNPCProcess.Destroyed:
                    EmitOnDestroyedChanged();
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private event NPCProcessStateChanged mOnStateChanged;
        public event NPCProcessStateChanged OnStateChanged
        {
            add
            {
                mOnStateChanged += value;
                if (State != StateOfNPCProcess.Created)
                {
                    Task.Run(() => {
                        try
                        {
                            value(this, State);
                        }
                        catch (Exception e)
                        {
#if DEBUG
                            Error($"e = {e}");
#endif
                        }
                    });
                }
            }

            remove
            {
                mOnStateChanged -= value;
            }
        }

        protected void EmitOnStateChanged(StateOfNPCProcess state)
        {
            Task.Run(() => {
                try
                {
                    mOnStateChanged?.Invoke(this, state);
                }
                catch (Exception e)
                {
#if DEBUG
                    Error($"e = {e}");
#endif
                }
            });
        }

        protected virtual void RunningChanged()
        {
        }

        private NPCProcessConcreteStateChanged mOnRunningChanged;
        public event NPCProcessConcreteStateChanged OnRunningChanged
        {
            add
            {
                mOnRunningChanged += value;

                if (State == StateOfNPCProcess.Running)
                {
                    Task.Run(() => {
                        try
                        {
                            value(this);
                        }
                        catch (Exception e)
                        {
#if DEBUG
                            Error($"e = {e}");
#endif
                        }
                    });
                }
            }

            remove
            {
                mOnRunningChanged -= value;
            }
        }

        protected void EmitOnRunningChanged()
        {
            RunningChanged();

            Task.Run(() => {
                try
                {
                    mOnRunningChanged?.Invoke(this);
                }
                catch (Exception e)
                {
#if DEBUG
                    Error($"e = {e}");
#endif
                }
            });
        }

        protected virtual void EndOfProcessChanged()
        {
        }

        private NPCProcessConcreteStateChanged mOnRanToCompletionChanged;
        public event NPCProcessConcreteStateChanged OnRanToCompletionChanged
        {
            add
            {
                mOnRanToCompletionChanged += value;

                if (State == StateOfNPCProcess.RanToCompletion)
                {
                    Task.Run(() => {
                        try
                        {
                            value(this);
                        }
                        catch (Exception e)
                        {
#if DEBUG
                            Error($"e = {e}");
#endif
                        }
                    });
                }
            }

            remove
            {
                mOnRanToCompletionChanged -= value;
            }
        }

        protected void EmitOnRanToCompletionChanged()
        {
            EndOfProcessChanged();

            Task.Run(() => {
                try
                {
                    mOnRanToCompletionChanged?.Invoke(this);
                }
                catch (Exception e)
                {
#if DEBUG
                    Error($"e = {e}");
#endif
                }
            });
        }

        private NPCProcessConcreteStateChanged mOnCanceledChanged;
        public event NPCProcessConcreteStateChanged OnCanceledChanged
        {
            add
            {
                mOnCanceledChanged += value;

                if (State == StateOfNPCProcess.Canceled)
                {
                    Task.Run(() => {
                        try
                        {
                            value(this);
                        }
                        catch (Exception e)
                        {
#if DEBUG
                            Error($"e = {e}");
#endif
                        }
                    });
                }
            }

            remove
            {
                mOnCanceledChanged -= value;
            }
        }

        protected virtual void CancelOfProcessChanged()
        {
            try
            {
                CancellationToken?.Cancel();
            }
            catch
            {
            }          
        }

        protected void EmitOnCanceledChanged()
        {
            CancelOfProcessChanged();
            EndOfProcessChanged();

            Task.Run(() => {
                try
                {
                    mOnCanceledChanged?.Invoke(this);
                }
                catch (Exception e)
                {
#if DEBUG
                    Error($"e = {e}");
#endif
                }
            });
        }

        private NPCProcessConcreteStateChanged mOnFaultedChanged;
        public event NPCProcessConcreteStateChanged OnFaultedChanged
        {
            add
            {
                mOnFaultedChanged += value;

                if (State == StateOfNPCProcess.Faulted)
                {
                    Task.Run(() => {
                        try
                        {
                            value(this);
                        }
                        catch (Exception e)
                        {
#if DEBUG
                            Error($"e = {e}");
#endif
                        }
                    });
                }
            }

            remove
            {
                mOnFaultedChanged -= value;
            }
        }

        protected void EmitOnFaultedChanged()
        {
            EndOfProcessChanged();

            Task.Run(() => {
                try
                {
                    mOnFaultedChanged?.Invoke(this);
                }
                catch (Exception e)
                {
#if DEBUG
                    Error($"e = {e}");
#endif
                }
            });
        }

        private NPCProcessConcreteStateChanged mOnDestroyedChanged;
        public event NPCProcessConcreteStateChanged OnDestroyedChanged
        {
            add
            {
                mOnDestroyedChanged += value;

                if (State == StateOfNPCProcess.Destroyed)
                {
                    Task.Run(() => {
                        try
                        {
                            value(this);
                        }
                        catch (Exception e)
                        {
#if DEBUG
                            Error($"e = {e}");
#endif
                        }
                    });
                }
            }

            remove
            {
                mOnDestroyedChanged -= value;
            }
        }

        protected void EmitOnDestroyedChanged()
        {
            CancelOfProcessChanged();
            EndOfProcessChanged();

            Task.Run(() => {
                try
                {
                    mOnDestroyedChanged?.Invoke(this);
                }
                catch (Exception e)
                {
#if DEBUG
                    Error($"e = {e}");
#endif
                }
            });
        }

        public abstract KindOfNPCProcess Kind { get; }

        public abstract void Dispose();

        public abstract ulong Id { get; set; }
        public Task Task { get; set; }
        public CancellationTokenSource CancellationToken { get; set; }
        public abstract float LocalPriority { get; set; }
        public abstract float GlobalPriority { get; }
    }
}
