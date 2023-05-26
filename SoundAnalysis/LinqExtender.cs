using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAnalysisLib
{
    public static class LinqExtender
    {
        class KeyValueComparer<K> : IComparer<KeyValuePair<K, double>>
        {
            public int Compare(KeyValuePair<K, double> x, KeyValuePair<K, double> y)
            {
                return Math.Sign(x.Value - y.Value);
            }
        }

        public static double Mode(this IEnumerable<double> seq)
        {
            Dictionary<double, double> count = new Dictionary<double, double>();
            foreach (var d in seq)
            {
                if (count.ContainsKey(d))
                    count[d]++;
                else
                    count[d] = 1;
            }
            return count.Max(new KeyValueComparer<double>()).Key;
        }

        public static double StandardDeviation(this IEnumerable<double> seq)
        {
            (double sum, double sumSquares, int count) = seq
                .Select(d => (d, d * d, 1))
                .Aggregate((0, 0, 0), ((double, double, int) a, (double, double, int) b) => (a.Item1 + b.Item1, a.Item2 + b.Item2, a.Item3 + b.Item3));

            return Math.Sqrt(sumSquares / count - sum * sum / (count * count));
        }

        public static double Median(this IEnumerable<double> seq)
        {
            var list = seq.ToList();
            if (list.Count == 0) return 0;
            list.Sort();
            if (list.Count % 2 == 0)
            {
                return (list[list.Count / 2 - 1] + list[list.Count / 2]) / 2;
            }
            else
            {
                return list[list.Count / 2];
            }
        }

        public static double Product(this IEnumerable<double> seq)
        {
            double prod = 1;
            foreach (var item in seq)
            {
                prod *= item;
            }
            return prod;
        }

        public static double GeometricAverage(this IEnumerable<double> seq)
        {
            return Math.Exp(seq.Select(d => Math.Log(d)).Average());
        }
    }
}
