using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewFrameworkSpriteExporter
{
    public class Sprite
    {
        public Actor[] actors { get; set; }
        public Dictionary<string, string> _spriteInfo;
        public double _stageLength;
        public Dictionary<int, StageItem[]> _timelines;
        public double minX { get; set; } = double.PositiveInfinity;
        public double maxX { get; set; } = double.NegativeInfinity;
        public double minY { get; set; } = double.PositiveInfinity;
        public double maxY { get; set; } = double.NegativeInfinity;


        public StageOptions stageOptions
        {
            get
            {
                return null;
            }
            set
            {
                _stageLength = value.StageLength;
                _spriteInfo = value.SpriteInfo.ToDictionary(s => s.SpriteInfo, s => s.Texture);
            }
        }

        public List<Timeline> timelines
        {
            get
            {
                return null;
            }
            set
            {
                _timelines = value.ToDictionary(t => t.spriteuid, t => t.stage);
            }
        }

        public double GetLongestTimeline()
        {
            return _timelines.Max(p => p.Value == null ? 0 : p.Value.Max(pp => pp.Time));
        }
    }
    

    public class StageOptions
    {
        public double StageLength { get; set; }
        public SpriteInfoItem[] SpriteInfo { get; set; }
    }

    public class SpriteInfoItem
    {
        public string SpriteInfo { get; set; }
        public string Texture { get; set; }
    }

    public class Timeline
    {
        public int spriteuid { get; set; }
        public StageItem[] stage { get; set; }
    }

    public class StageItem
    {
        public int[] Alignment { get; set; }
        public double Alpha { get; set; }
        public double Angle { get; set; }
        public UInt32 Colour { get; set; } = 4294967295;
        public int Flip { get; set; }
        public double[] Position { get; set; }
        public double[] Scale { get; set; }
        public bool Shown { get; set; }
        public double Time { get; set; }
    }
}
