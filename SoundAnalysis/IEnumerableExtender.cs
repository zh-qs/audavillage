using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAnalysisLib
{
    public static class IEnumerableExtender
    {
        // Equivalent to .Reverse().SkipWhile(predicate).Reverse(), but without reversing the sequence
        public static IEnumerable<T> SkipLastWhile<T>(this IEnumerable<T> values, Func<T, bool> predicate)
        {
            Queue<T> queue = new Queue<T>();
            int lastBeforeFalseIdx = -1;
            int i = 0;
            foreach (T item in values)
            {
                queue.Enqueue(item);
                if (!predicate(item))
                {
                    lastBeforeFalseIdx = i;
                }
                ++i;
            }
            for (int j=0;j<=lastBeforeFalseIdx; ++j)
            {
                yield return queue.Dequeue();
            }
        }
    }
}
