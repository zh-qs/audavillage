using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundAnalysisLib
{
    public abstract class WindowFunction
    {
        protected int start = 0, end = -1;

        //public WindowFunction(int start, int end)
        //{
        //    this.start = start;
        //    this.end = end;
        //}
        public abstract double GetWindowedValue(int idx);
        public abstract double Area();
        public void GetBordersFromClip(Clip clip)
        {
            start = clip.StartIndex;
            end = start + clip.SampleCount;
        }

        public void GetBordersFromFrame(Frame frame)
        {
            start = frame.StartIndex;
            end = start + frame.FrameLength;
        }
    }

    public class RectangularWindow : WindowFunction
    {
        public override double Area()
        {
            return end - start;
        }

        //public RectangularWindow(int start, int end) : base(start, end) { }
        public override double GetWindowedValue(int idx)
        {
            return (idx >= start && idx <= end) ? 1 : 0;
        }
    }

    public class TriangularWindow : WindowFunction
    {
        public override double Area()
        {
            return (end - start) * 0.5;
        }

        //public TriangularWindow(int start, int end) : base(start, end) { }
        public override double GetWindowedValue(int idx)
        {
            if (idx < start || idx > end) return 0;
            return  1.0 - (2.0 * Math.Abs(idx - (start + end) * 0.5)) / (end - start);
        }
    }

    public class HanningWindow : WindowFunction
    {
        public override double Area()
        {
            return (end - start) * 0.5;
        }

        // public HanningWindow(int start, int end) : base(start, end) { }
        public override double GetWindowedValue(int idx)
        {
            if (idx < start || idx > end) return 0;
            return 0.5 * (1.0 - Math.Cos(2.0 * Math.PI * (idx - start) / (end - start)));
        }
    }

    public class HammingWindow : WindowFunction
    {
        public override double Area()
        {
            return (end - start) * 0.54;
        }

        //public HammingWindow(int start, int end) : base(start, end) { }
        public override double GetWindowedValue(int idx)
        {
            if (idx < start || idx > end) return 0;
            return 0.54 - 0.46 * Math.Cos(2.0 * Math.PI * (idx - start) / (end - start));
        }
    }

    public class BlackmanWindow : WindowFunction
    {
        public override double Area()
        {
            return (end - start) * 0.42;
        }

        //public BlackmanWindow(int start, int end) : base(start, end) { }
        public override double GetWindowedValue(int idx)
        {
            if (idx < start || idx > end) return 0;
            return 0.42 - 0.5 * Math.Cos(2.0 * Math.PI * (idx - start) / (end - start)) + 0.08 * Math.Cos(4.0 * Math.PI * (idx - start) / (end - start));
        }
    }

    public class GaussianWindow : WindowFunction
    {
        private const double stdDev = 0.5;
        public override double Area()
        {
            return Math.Sqrt(Math.PI * 0.5) * (end - start) * stdDev * ErfApprox(Math.Sqrt(0.5) / stdDev);
        }

        private double ErfApprox(double x)
        {
            const double p = 0.3275911, a1 = 0.254829592, a2 = -0.284496736, a3 = 1.241413741, a4 = -1.453152027, a5 = 1.061405429;
            double t = 1.0 / (1.0 + p * x);
            return 1.0 - t * (a1 + t * (a2 + t * (a3 + t * (a4 + t * a5)))) * Math.Exp(-x * x);
        }

        public override double GetWindowedValue(int idx)
        {
            if (idx < start || idx > end) return 0;
            double Ndiv2 = (end - start) * 0.5;
            double x = (idx - start - Ndiv2) / stdDev / Ndiv2;
            return Math.Exp(-0.5 * x * x);
        }
    }
}
