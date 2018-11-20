using System;
using System.Drawing;
namespace MMShareBLL.Model
{
    /// <summary>
    /// ��Ⱦ�ȼ�
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
        /// ��ţ���ʶ����
        /// </summary>
        public int LevelID
        {
            set { levelId = value; }
            get { return levelId; }
        }

        /// <summary>
        /// ����Ⱦ�ȼ���API����
        /// </summary>
        public float StartRegion
        {
            set { startRegion = value; }
            get { return startRegion; }
        }

        /// <summary>
        /// ����Ⱦ�ȼ���API����
        /// </summary>
        public float EndRegion
        {
            set { endRegion = value; }
            get { return endRegion; }
        }

        /// <summary>
        /// �ȼ����ƣ��磺һ��������......
        /// </summary>
        public string Name
        {
            set { name = value; }
            get { return name; }
        }

        /// <summary>
        /// �ȼ����ţ��磺�񡢢򡢢�......
        /// </summary>
        public string Symbol
        {
            set { symbol = value; }
            get { return symbol; }
        }

        /// <summary>
        /// ��Ⱦ�̶�
        /// </summary>
        public string Grade
        {
            set { grade = value; }
            get { return grade; }
        }

        /// <summary>
        /// �Խ�����Ӱ��
        /// </summary>
        public string HealthImpact
        {
            set { healthimpact = value; }
            get { return healthimpact; }
        }

        /// <summary>
        /// �����ȡ�Ĵ�ʩ
        /// </summary>
        public string Measure
        {
            set { measure = value; }
            get { return measure; }
        }

        /// <summary>
        /// ����Ⱦ�ȼ������ֵ���ɫ����
        /// </summary>
        public string ColorName
        {
            set { colorName = value; }
            get { return colorName; }
        }

        /// <summary>
        /// ����Ⱦ�ȼ������ֵ���ɫֵ
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

