using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace SoundAnalysisLib
{
    public static class FFTExtender
    {
        private static Complex W(int k, int N)
        {
            double exponent = -2.0 * Math.PI * k / N;
            return new Complex(Math.Cos(exponent), Math.Sin(exponent));
        }

        private static int ReverseBits(int x, int bits)
        {
            int res = 0;
            for (int i=0;i<bits;++i)
                res |= ((x & (1 << i)) >> i) << (bits - i - 1);
            return res;
        }

        private static void PerformFFTOn(Complex[] result, bool inverse = false)
        {
            // check if result length is power of two
            int l = result.Length;
            int logl = 0;
            while (l > 1)
            {
                if (l % 2 != 0) throw new Exception();
                l >>= 1;
                ++logl;
            }

            // rearrange items
            for (int i = 0; i < result.Length; i++)
            {
                int iRev = ReverseBits(i, logl);
                if (i < iRev)
                {
                    var s = result[i];
                    result[i] = result[iRev];
                    result[iRev] = s;
                }
            }

            // fft
            int offset = 1;
            while (offset < result.Length)
            {
                for (int i = 0; i < result.Length; i += 2 * offset)
                {
                    for (int j = 0; j < offset; j++)
                    {
                        result[i + j + offset] *= W(inverse ? -j : j, 2 * offset);
                        var next1 = result[i + j] + result[i + j + offset];
                        var next2 = result[i + j] - result[i + j + offset];
                        result[i + j] = next1;
                        result[i + j + offset] = next2;
                    }
                }
                offset <<= 1;
            }

            if (inverse)
            {
                for (int i = 0; i < result.Length; i++)
                    result[i] /= result.Length;
            }
        }

        private static void PerformCepstrumOn(Complex[] result)
        {
            //PerformFFTOn(result);
            //for (int i = 0; i < result.Length; i++)
            //    result[i] = new Complex(Math.Log(result[i].Magnitude), result[i].Phase);
            //PerformFFTOn(result, inverse: true);
            PerformFFTOn(result);
            for (int i = 0; i < result.Length; i++)
                result[i] = Math.Log(result[i].Magnitude);
            //result = result.Take(result.Length / 2).ToArray();
            PerformFFTOn(result, inverse: true);
        }

        public static double[] FFT(this IEnumerable<double> data)
        {
            Complex[] result = data.Select(d => new Complex(d, 0)).ToArray();
            PerformFFTOn(result);
            return result.Select(c => c.Magnitude).Take(result.Length / 2).ToArray();
        }

        public static double[] InverseFFT(this IEnumerable<double> data)
        {
            Complex[] result = data.Select(d => new Complex(d, 0)).ToArray();
            PerformFFTOn(result, inverse: true);
            return result.Select(c => c.Magnitude).Take(result.Length / 2).ToArray();
        }

        public static double[] FFT(this Clip clip, int channel = 0)
        {
            Complex[] result = clip.GetWindowedData(alignToPowerOfTwo: true, channel).Select(d => new Complex(d, 0)).ToArray();

            PerformFFTOn(result);

            return result.Select(c => c.Magnitude).Take(result.Length / 2).ToArray();

            //int offset = result.Length / 2;
            //int step = 2;
            //while (offset > 0)
            //{
            //    for (int i=0;i<result.Length;i+=2*offset)
            //    {
            //        for (int j=0;j<offset;j++)
            //        {
            //            result[i + j + offset] *= W(j, step);
            //            var next1 = result[i + j] + result[i + j + offset];
            //            var next2 = result[i + j] - result[i + j + offset];
            //            result[i + j] = next1;
            //            result[i + j + offset] = next2;
            //        }
            //    }
            //    offset >>= 1;
            //    step <<= 1;
            //}
            //double[] modulesInCorrectOrder = new double[result.Length];
            //for (int i = 0; i < result.Length; ++i)
            //    modulesInCorrectOrder[ReverseBits(i, logl)] = result[i].Magnitude;

            //return modulesInCorrectOrder;
        }

        public static double[] Cepstrum(this IEnumerable<double> data)
        {
            Complex[] result = data.Select(d => new Complex(d, 0)).ToArray();
            PerformCepstrumOn(result);
            return result.Select(c => c.Magnitude).ToArray();
        }
    }
}
