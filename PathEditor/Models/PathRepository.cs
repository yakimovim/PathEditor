using System;
using System.Linq;
using Microsoft.Win32;

namespace PathEditor.Models
{
    internal class PathRepository
    {
        private const string PathVariableName = "Path";
        private const char PathPartsSeparator = ';';
        private const string PathKeyName = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment\";

        public string[] GetPathParts()
        {
            return ((string)Registry.LocalMachine
                .OpenSubKey(PathKeyName)
                .GetValue(PathVariableName, string.Empty, RegistryValueOptions.DoNotExpandEnvironmentNames))
                .Split(new[] { PathPartsSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
        }

        public void SetPathFromParts(string[] pathParts)
        {
            var paths = pathParts
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct(StringComparer.OrdinalIgnoreCase);

            var path = string.Join(Convert.ToString(PathPartsSeparator), paths);

            Environment.SetEnvironmentVariable(PathVariableName, path, EnvironmentVariableTarget.Machine);
        }
    }
}
