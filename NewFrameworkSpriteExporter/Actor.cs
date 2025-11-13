using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewFrameworkSpriteExporter
{
    public class Actor
    {
        public int[] Alignment { get; set; }
        public double Alpha { get; set; }
        public double Angle { get; set; }
        public UInt32 Colour { get; set; } = 4294967295;

        public int Flip { get; set; }
        public double[] Position { get; set; }
        public double[] Scale { get; set; }
        public bool Shown { get; set; }
        public string sprite { get; set; }
        public int type { get; set; }
        public int uid { get; set; }

        /// <summary>
        /// Linear interpolation between two doubles on a timeline.
        /// </summary>
        /// <param name="valueStart">Initial value</param>
        /// <param name="valueEnd">Final value</param>
        /// <param name="timeStart">Initial time</param>
        /// <param name="timeEnd">Final time</param>
        /// <param name="time">Current time</param>
        /// <returns></returns>
        private static double Interpolate(double valueStart, double valueEnd, double timeStart, double timeEnd, double time)
        {
            if (!(timeStart <= time && time <= timeEnd)) return valueStart;
            return valueStart + ((time - timeStart) / (timeEnd - timeStart) * (valueEnd - valueStart));
        }

        /// <summary>
        /// Generates an Actor object with params based on its timeline and the current time.
        /// </summary>
        /// <param name="timeline"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Actor GetActorAtTime(StageItem[] timeline, double time)
        {
            Actor actor = new();

            StageItem prev = null, next = null;
            // find the previous keyframe (maximum of keyframes below time) and next keyframe (minimum of keyframes above time)
            foreach (var item in timeline)
            {
                if ((prev == null || item.Time > prev.Time) && item.Time <= time)
                    prev = item;
                else if ((next == null || item.Time < next.Time) && item.Time > time)
                    next = item;
            }

            // current time is start of timeline
            if (prev != null && next == null)
            {
                actor.Alignment = prev.Alignment;
                actor.Alpha = prev.Alpha;
                actor.Angle = prev.Angle;
                actor.Colour = prev.Colour;
                actor.Flip = prev.Flip;
                actor.Position = prev.Position;
                actor.Scale = prev.Scale;
                actor.Shown = prev.Shown;
            }
            // current time is end of timeline
            else if (prev == null && next != null)
            {
                actor.Alignment = next.Alignment;
                actor.Alpha = next.Alpha;
                actor.Angle = next.Angle;
                actor.Colour = next.Colour;
                actor.Flip = next.Flip;
                actor.Position = next.Position;
                actor.Scale = next.Scale;
                actor.Shown = next.Shown;
            }
            // current time is between two keyframes (interpolated)
            else
            {
                actor.Alignment = prev.Alignment;
                actor.Alpha = Interpolate(prev.Alpha, next.Alpha, prev.Time, next.Time, time);
                actor.Angle = Interpolate(prev.Angle, next.Angle, prev.Time, next.Time, time);
                actor.Flip = prev.Flip;
                actor.Position = [Interpolate(prev.Position[0], next.Position[0], prev.Time, next.Time, time), Interpolate(prev.Position[1], next.Position[1], prev.Time, next.Time, time)];
                actor.Scale = [Interpolate(prev.Scale[0], next.Scale[0], prev.Time, next.Time, time), Interpolate(prev.Scale[1], next.Scale[1], prev.Time, next.Time, time)];
                actor.Shown = prev.Shown;

                // each channel needs to be interpolated separately
                double a = Interpolate((prev.Colour >> 24) & 0xFF, (next.Colour >> 24) & 0xFF, prev.Time, next.Time, time);
                double b = Interpolate((prev.Colour >> 16) & 0xFF, (next.Colour >> 16) & 0xFF, prev.Time, next.Time, time);
                double g = Interpolate((prev.Colour >> 8) & 0xFF, (next.Colour >> 8) & 0xFF, prev.Time, next.Time, time);
                double r = Interpolate(prev.Colour & 0xFF, next.Colour & 0xFF, prev.Time, next.Time, time);

                actor.Colour = ((uint)a << 24) + ((uint)b << 16) + ((uint)g << 8) + (uint)r;
            }

            return actor;
        }
    }
}
