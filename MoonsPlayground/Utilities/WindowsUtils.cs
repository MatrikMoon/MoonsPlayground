using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MoonsPlayground.Utilities
{
    public class WindowsUtils
    {
        public static List<string> ExeSearch(string root, int max = 50, List<string> fileList = null)
        {
            fileList = fileList ?? new List<string>();

            try
            {
                foreach (string file in Directory.GetFiles(root))
                {
                    if (fileList.Count >= max) break;
                    if (file.ToLower().EndsWith(".exe")) fileList.Add(file);
                }

                foreach (var directory in Directory.GetDirectories(root))
                {
                    if (fileList.Count >= max) break;
                    fileList.AddRange(ExeSearch(directory));
                }
            }
            catch (Exception e)
            {
                Plugin.Log?.Error(e.Message);
            }
            return fileList;
        }

        public static string FindNthExe(string root, int n, List<string> fileList = null)
        {
            fileList = fileList ?? new List<string>();

            try
            {
                foreach (string file in Directory.GetFiles(root))
                {
                    if (fileList.Count >= n) break;
                    if (file.ToLower().EndsWith(".exe")) fileList.Add(file);
                }

                foreach (var directory in Directory.GetDirectories(root))
                {
                    if (fileList.Count >= n) break;
                    fileList.AddRange(ExeSearch(directory));
                }
            }
            catch (Exception e)
            {
                Plugin.Log?.Error(e.Message);
            }
            return fileList.ElementAt(n);
        }

        public static void RunProgram(string path) => Process.Start(path);
    }
}
