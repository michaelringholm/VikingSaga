using System;

namespace VikingSagaWpfApp.Code.Battle
{
    internal static class BattleUtil
    {
        public static int FirstNull<T>(T[] array)
        {
            int result;
            if (!TryGetFirstNull(array, out result))
                throw new ArgumentException("Array contains no NULL");

            return result;
        }

        public static bool TryGetFirstNull<T>(T[] array, out int idx)
        {
            idx = 0;

            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i] == null)
                {
                    idx = i;
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetFirstNotNull<T>(T[] array, out int idx)
        {
            idx = 0;

            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i] != null)
                {
                    idx = i;
                    return true;
                }
            }

            return false;
        }
    }
}
