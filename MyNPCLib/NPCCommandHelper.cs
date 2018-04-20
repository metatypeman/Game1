using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public static class NPCCommandHelper
    {
        public static NPCInternalCommand ConvertICommandToInternalCommand(INPCCommand command, IEntityDictionary entityDictionary)
        {
            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if(string.IsNullOrWhiteSpace(command.Name))
            {
                throw new ArgumentNullException("command.Name");
            }

            var result = new NPCInternalCommand();
            result.Key = entityDictionary.GetKey(command.Name);
            result.InitiatingProcessId = command.InitiatingProcessId;
            result.KindOfLinkingToInitiator = command.KindOfLinkingToInitiator;
            result.Priority = command.Priority;

            foreach(var commandParam in command.Params)
            {
                var commandParamName = commandParam.Key;
                var commandParamValue = commandParam.Value;

                var paramKey = entityDictionary.GetKey(commandParamName);

                result.Params[paramKey] = commandParamValue;
            }

            return result;
        }
    }
}
