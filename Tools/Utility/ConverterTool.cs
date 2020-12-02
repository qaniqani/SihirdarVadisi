using System.Collections.Generic;

namespace Tools.Utility
{
    public class ConverterTool
    {
        public static int GetLevels(int level)
        {
            return GetLevelConverter[level];
        }

        private static readonly Dictionary<int, int> GetLevelConverter = new Dictionary<int, int>
        {
            {1, 1800},
            {2, 4200},
            {3, 6600},
            {4, 9000}
        };
    }
}