using System;
using System.Drawing;
namespace MMShareBLL.Model
{
    /// <summary>
    /// 污染等级
    /// </summary>
    public class PolluteLevel
    {
        private int levelId;
        private float startRegion;
        private float endRegion;
        private string name;
        private string symbol;
        private string grade;
        private string healthimpact;
        private string measure;
        private string colorName;
        private string colorValue;
        private Color color;

        /// <summary>
        /// 编号，标识属性
        /// </summary>
        public int LevelID
        {
            set { levelId = value; }
            get { return levelId; }
        }

        /// <summary>
        /// 该污染等级的API下限
        /// </summary>
        public float StartRegion
        {
            set { startRegion = value; }
            get { return startRegion; }
        }

        /// <summary>
        /// 该污染等级的API上限
        /// </summary>
        public float EndRegion
        {
            set { endRegion = value; }
            get { return endRegion; }
        }

        /// <summary>
        /// 等级名称，如：一级、二级......
        /// </summary>
        public string Name
        {
            set { name = value; }
            get { return name; }
        }

        /// <summary>
        /// 等级符号，如：Ⅰ、Ⅱ、Ⅲ......
        /// </summary>
        public string Symbol
        {
            set { symbol = value; }
            get { return symbol; }
        }

        /// <summary>
        /// 污染程度
        /// </summary>
        public string Grade
        {
            set { grade = value; }
            get { return grade; }
        }

        /// <summary>
        /// 对健康的影响
        /// </summary>
        public string HealthImpact
        {
            set { healthimpact = value; }
            get { return healthimpact; }
        }

        /// <summary>
        /// 建议采取的措施
        /// </summary>
        public string Measure
        {
            set { measure = value; }
            get { return measure; }
        }

        /// <summary>
        /// 该污染等级所表现的颜色名称
        /// </summary>
        public string ColorName
        {
            set { colorName = value; }
            get { return colorName; }
        }

        /// <summary>
        /// 该污染等级所表现的颜色值
        /// </summary>
        public string ColorValue
        {
            set
            {
                colorValue = value;
                Color theColor = Color.Blue;
                try
                {
                    theColor = Color.FromArgb(Int32.Parse(colorValue, System.Globalization.NumberStyles.HexNumber));
                    color = theColor;
                }
                catch { }
            }
            get { return colorValue; }
        }

        public Color Color
        {
            get { return color; }
        }

        public PolluteLevel Clone()
        {
            PolluteLevel newObj = new PolluteLevel();
            newObj.levelId = this.levelId;
            newObj.startRegion = this.startRegion;
            newObj.endRegion = this.endRegion;
            newObj.name = this.name;
            newObj.symbol = this.symbol;
            newObj.grade = this.grade;
            newObj.healthimpact = this.healthimpact;
            newObj.measure = this.measure;
            newObj.colorName = this.colorName;
            newObj.colorValue = this.colorValue;
            newObj.color = this.color;
            return newObj;
        }
    }
}

