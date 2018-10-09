using Assets.Scripts;
using System;

public interface ISpellHelperDialog : IBaseDialog
{
    event Action<string> OnSpellMessage;
}
