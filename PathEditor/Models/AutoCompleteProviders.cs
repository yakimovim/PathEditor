using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PathEditor.Models
{
    internal class AutoCompleteProviderFactory
    {
        private static readonly IAutoCompleteProvider Empty = new EmptyAutoCompleteProvider();

        public static IAutoCompleteProvider GetAutoCompleteProvider(string text)
        {
            if(string.IsNullOrEmpty(text))
            {return Empty;}

            var indexOfSeparator = text.LastIndexOf(Path.DirectorySeparatorChar);

            string prefix = text.Substring(0, indexOfSeparator + 1);
            string textToAppend = text.Substring(indexOfSeparator + 1);

            if (textToAppend.StartsWith("%"))
                return new EnvironmentVariableAutoCompleteProvider(textToAppend);
            if (!string.IsNullOrEmpty(prefix))
                return new DirectoryAutoCompleteProvider(prefix, textToAppend);

            return Empty;
        }
    }

    internal interface IAutoCompleteProvider
    {
        string GetAutoCompleteText();
    }

    internal class EmptyAutoCompleteProvider : IAutoCompleteProvider
    {
        public string GetAutoCompleteText()
        {
            return string.Empty;
        }
    }

    internal class EnvironmentVariableAutoCompleteProvider : IAutoCompleteProvider
    {
        private static readonly string[] EnvironmentVariables = Environment.GetEnvironmentVariables().Keys.OfType<string>().ToArray();

        private readonly string _variableNamePrefix;

        public EnvironmentVariableAutoCompleteProvider(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend)) throw new ArgumentNullException("textToAppend");

            // remove leading '%'
            _variableNamePrefix = textToAppend.Substring(1);
        }

        public string GetAutoCompleteText()
        {
            if (string.IsNullOrEmpty(_variableNamePrefix))
                return string.Empty;

            var variant = EnvironmentVariables
                .FirstOrDefault(v => v.StartsWith(_variableNamePrefix, StringComparison.OrdinalIgnoreCase)) ?? string.Empty;

            if (variant == string.Empty)
                return string.Empty;

            return variant.Substring(_variableNamePrefix.Length) + "%";

        }
    }

    internal class DirectoryAutoCompleteProvider : IAutoCompleteProvider
    {
        private static string _currentDirectory;
        private static string[] _currentSubDirectories;

        private readonly bool _directoryExists;
        private readonly string _textToAppend;

        public DirectoryAutoCompleteProvider(string prefix, string textToAppend)
        {
            _textToAppend = textToAppend;

            _directoryExists = prefix.ExpandedDirectoryExists();

            if (_directoryExists)
            {
                RefreshSubdirectories(prefix);
            }
        }

        private void RefreshSubdirectories(string prefix)
        {
            string directory = Environment.ExpandEnvironmentVariables(prefix);
            if (directory.Equals(_currentDirectory, StringComparison.OrdinalIgnoreCase))
                return;

            _currentDirectory = directory;
            _currentSubDirectories = Directory.GetDirectories(directory)
                .Select(Path.GetFileName).ToArray();
        }

        public string GetAutoCompleteText()
        {
            if (_directoryExists == false || string.IsNullOrEmpty(_textToAppend))
                return string.Empty;

            var variant = _currentSubDirectories
                .FirstOrDefault(d => d.StartsWith(_textToAppend, StringComparison.OrdinalIgnoreCase)) ?? string.Empty;

            if (variant == string.Empty)
                return string.Empty;

            return variant.Substring(_textToAppend.Length) + Path.DirectorySeparatorChar;
        }
    }
}