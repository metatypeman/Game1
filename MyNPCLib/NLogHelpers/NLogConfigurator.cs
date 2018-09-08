using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MyNPCLib.NLogHelpers
{
    public static class NLogConfigurator
    {
        public static void Config()
        {
            Config(CreateDefaultOptions());
        }

        private static NLogConfiguratOptions CreateDefaultOptions()
        {
            return new NLogConfiguratOptions()
            {
                UseConsole = true,
                UseFile = true,
                TargetDirectory = NLogConfiguratOptionsTargetDirectory.None,
                SeparateFiles = NLogConfiguratOptionsSeparateFiles.ByDate
            };
        }

        public static void Config(NLogConfiguratOptions options)
        {
            var config = new LoggingConfiguration();

            if (options.UseConsole)
            {
                var consoleTarget = new ColoredConsoleTarget();
                config.AddTarget("console", consoleTarget);

                var rule = new LoggingRule("*", LogLevel.Debug, consoleTarget);
                config.LoggingRules.Add(rule);
            }

            if (options.UseFile)
            {
                var fileTarget = new FileTarget();
                config.AddTarget("file", fileTarget);

                var name = Path.Combine(GetPath(options), GetName(options.SeparateFiles));

                var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
                config.LoggingRules.Add(rule);

                fileTarget.FileName = $"{name}.log";

                if (options.SeparateFiles == NLogConfiguratOptionsSeparateFiles.DeleteOldFile)
                {
                    fileTarget.DeleteOldFileOnStartup = true;
                }
            }

            LogManager.Configuration = config;
        }

        private static string GetName(NLogConfiguratOptionsSeparateFiles separateFiles)
        {
            var name = GetRootNamespace(GetClassFullName());

            switch (separateFiles)
            {
                case NLogConfiguratOptionsSeparateFiles.ByDate:
                    var now = DateTime.Now;
                    return $"{name}_{now.Day}_{now.Month}_{now.Year} {now.Hour}_{now.Minute}_{now.Second}_{now.Millisecond}";

                default:
                    return name;
            }
        }

        private static string GetRootNamespace(string fullName)
        {
            return Regex.Match(fullName, @"^\w+").ToString();
        }

        private static string GetClassFullName()
        {
            var className = string.Empty;
            Type declaringType = null;
            var framesToSkip = 2;

            while (true)
            {
                var frame = new StackFrame(framesToSkip, false);

                var method = frame.GetMethod();

                declaringType = method?.DeclaringType;
                if (declaringType == null)
                {
                    break;
                }

                if (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                framesToSkip++;
                className = declaringType.FullName;
            }

            return className;
        }

        private static string GetPath(NLogConfiguratOptions options)
        {
            var targetDir = options.TargetDirectory;

            switch (targetDir)
            {
                case NLogConfiguratOptionsTargetDirectory.None:
                    return string.Empty;

                case NLogConfiguratOptionsTargetDirectory.USERPROFILE:
                    return Environment.GetEnvironmentVariable("USERPROFILE");

                case NLogConfiguratOptionsTargetDirectory.VisualStudioDir:
                    return Environment.GetEnvironmentVariable("VisualStudioDir");

                case NLogConfiguratOptionsTargetDirectory.APPDATA:
                    return Environment.GetEnvironmentVariable("APPDATA");

                case NLogConfiguratOptionsTargetDirectory.LOCALAPPDATA:
                    return Environment.GetEnvironmentVariable("LOCALAPPDATA");

                case NLogConfiguratOptionsTargetDirectory.Path:
                    return options.TargetPath;

                case NLogConfiguratOptionsTargetDirectory.PathOrUserProfileIfNotExist:
                    var targetPath = options.TargetPath;
                    if (string.IsNullOrWhiteSpace(targetPath))
                    {
                        return Environment.GetEnvironmentVariable("USERPROFILE");
                    }

                    if (Directory.Exists(targetPath))
                    {
                        return targetPath;
                    }

                    return Environment.GetEnvironmentVariable("USERPROFILE");

                default: return string.Empty;
            }
        }
    }
}
