
using System;
using System.ComponentModel;

namespace MSC.Setting.Console
{
    /// <summary>
    /// 边界值结构定义（最大/最小值）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class BorderValue<T> where T : struct
    {
        /// <summary>
        /// 获取或设置最小值
        /// </summary>
        public T Min { get; set; }

        /// <summary>
        /// 获取或设置最大值
        /// </summary>
        public T Max { get; set; }

        /// <summary>
        /// 默认构造
        /// </summary>
        public BorderValue()
        {
        }

        /// <summary>
        /// 根据最小/最大值构造
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public BorderValue(T min, T max)
        {
            this.Min = min;
            this.Max = max;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}]", Min, Max);
        }
    }

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class BoardLimitSetting
    {
        /// <summary>
        /// 获取或设置宽度限制
        /// </summary>
        public BorderValue<decimal> WidthBorder { get; set; }

        /// <summary>
        /// 获取或设置高度限制
        /// </summary>
        public BorderValue<decimal> HeightBorder { get; set; }

        /// <summary>
        /// 获取或设置厚度限制
        /// </summary>
        public BorderValue<decimal> ThicknessBorder { get; set; }

        /// <summary>
        /// 默认构造
        /// </summary>
        public BoardLimitSetting()
            : this(new BorderValue<decimal>(), new BorderValue<decimal>(), new BorderValue<decimal>())
        {
        }

        /// <summary>
        /// 带参构造
        /// </summary>
        public BoardLimitSetting(BorderValue<decimal> widthBorder, BorderValue<decimal> heightBorder, BorderValue<decimal> thicknessBorder)
        {
            this.WidthBorder = widthBorder;
            this.HeightBorder = heightBorder;
            this.ThicknessBorder = thicknessBorder;
        }

        public override string ToString()
        {
            return string.Format("Width:{0} | Height:{1} | Thickness:{2}", WidthBorder, HeightBorder, ThicknessBorder);
        }
    }
}
