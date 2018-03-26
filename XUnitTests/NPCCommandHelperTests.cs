using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTests
{
    public class NPCCommandHelperTests
    {
        [Fact]
        public void ConvertICommandToInternalCommand_PutNull_ArgumentNullException()
        {
            var globalEntityDictionary = new EntityDictionary();

            var e = Assert.Throws<ArgumentNullException>(() => {
                var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(null, globalEntityDictionary);
            });

            Assert.Equal("command", e.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ConvertICommandToInternalCommand_PutCommandWithNullOrEmptyName_ArgumentNullException(string name)
        {
            var globalEntityDictionary = new EntityDictionary();

            var command = new NPCCommand();
            command.Name = name;

            var e = Assert.Throws<ArgumentNullException>(() => {
                var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);
            });

            Assert.Equal("command.Name", e.ParamName);
        }

        [Fact]
        public void ConvertICommandToInternalCommand_PutCommandWithoutParameters_GotInternalCommandWithoutParameters()
        {
            var globalEntityDictionary = new EntityDictionary();

            var command = new NPCCommand();
            command.Name = "someName";

            var key = globalEntityDictionary.GetKey("someName");

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            Assert.NotEqual(null, internalCommand);
            Assert.Equal(key, internalCommand.Key);
            Assert.Equal(command.InitiatingProcessId, internalCommand.InitiatingProcessId);
            Assert.Equal(command.KindOfLinkingToInitiator, internalCommand.KindOfLinkingToInitiator);

            Assert.NotEqual(null, internalCommand.Params);
            Assert.Equal(0, internalCommand.Params.Count);
        }

        [Fact]
        public void ConvertICommandToInternalCommand_PutCommandWithParameters_GotInternalCommandWithParameters()
        {
            var globalEntityDictionary = new EntityDictionary();

            var command = new NPCCommand();
            command.Name = "someName";
            command.Params["param1"] = 12;

            var key = globalEntityDictionary.GetKey("someName");

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            Assert.NotEqual(null, internalCommand);
            Assert.Equal(key, internalCommand.Key);
            Assert.Equal(command.InitiatingProcessId, internalCommand.InitiatingProcessId);
            Assert.Equal(command.KindOfLinkingToInitiator, internalCommand.KindOfLinkingToInitiator);

            Assert.NotEqual(null, internalCommand.Params);
            Assert.Equal(command.Params.Count, internalCommand.Params.Count);

            foreach(var commandParam in command.Params)
            {
                var commandParamName = commandParam.Key;
                var commandParamValue = commandParam.Value;

                var paramKey = globalEntityDictionary.GetKey(commandParamName);

                var internalCommandParamValue = internalCommand.Params[paramKey];

                Assert.Equal(commandParamValue, internalCommandParamValue);
            }
        }
    }
}
