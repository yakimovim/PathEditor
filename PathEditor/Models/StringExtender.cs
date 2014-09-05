using System;
using System.IO;

namespace PathEditor.Models
{
    public static class StringExtender
    {
        public static bool ExpandedDirectoryExists(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;

            try
            {
                return Directory.Exists(Environment.ExpandEnvironmentVariables(str));
            }
            catch
            {
                return false;
            }
        }
    }
}