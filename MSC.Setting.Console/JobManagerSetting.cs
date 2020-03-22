using MSC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using System.Xml.Serialization;

namespace MSC.Setting.Console
{
    public enum MDUpdateType
    {
        NONE,
        UNEXPOSED,
        ALL,
        ASK
    }

    public enum JobFileServerType
    {
        FileSystem = 0,
        FTP = 1
    }

    public enum UserGroup
    {
        OP,
        ME,
        SE
    }

    public enum ScaleMode
    {
        [Description("自动")]
        AutoScale = 0,
        [Description("固定涨缩")]
        FixedScale = 1,
        [Description("分区涨缩")]
        SectionScale = 3,
        [Description("不涨缩")]
        NoneScale = 2,
        [Description("测量涨缩")]
        MeasureScale = 4,
        [Description("未知涨缩")]
        Undefined = 255
    }

    [TypeConverter(typeof(PropertySorter))]
    public class JobManagerSetting : IPersistObj
    {
        #region 料号服务器设置
        [Category("料号服务器设置"), PropertyOrder(0), DisplayName("类型")]
        [DefaultValue(JobFileServerType.FTP)]
        public JobFileServerType JobServerType
        {
            get { return jobServerType; }
            set
            {
                jobServerType = value;
                Tools.SetPropertyAttribute(this, "JobFileServerHost", typeof(BrowsableAttribute), "browsable", value == JobFileServerType.FTP);
                Tools.SetPropertyAttribute(this, "JobFileServerFullName", typeof(BrowsableAttribute), "browsable", value == JobFileServerType.FileSystem);
                Tools.SetPropertyAttribute(this, "JobFileServerDefaultProfile", typeof(BrowsableAttribute), "browsable", value == JobFileServerType.FileSystem);
            }
        }
        private JobFileServerType jobServerType;

        [Category("料号服务器设置"), PropertyOrder(1), DisplayName("主机")]
        [DefaultValue("192.168.6.99")]
        [Browsable(true)]
        public string JobFileServerHost { get; set; }

        [Category("料号服务器设置"), PropertyOrder(2), DisplayName("全路径")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue("E:\\jobfiles")]
        [Browsable(true)]
        public string JobFileServerFullName { get; set; }

        [Category("料号服务器设置"), PropertyOrder(3), DisplayName("用户名")]
        [DefaultValue("Administrator")]
        public string JobFileServerUserName	{ get; set; }

        [Category("料号服务器设置"), PropertyOrder(4), DisplayName("密码")]
        [PasswordPropertyText(true)]
        [DefaultValue("123456")]
        public string JobFileServerPassword { get; set; }

        [Category("料号服务器设置"), PropertyOrder(5), DisplayName("缓冲区(M)")]
        [DefaultValue(2)]
        public int JobFileServerBufferSize { get; set; }

        [Category("料号服务器设置"), PropertyOrder(6), DisplayName("是否使用缺省认证")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool JobFileServerDefaultProfile { get; set; }

        [Category("料号服务器设置"), PropertyOrder(7), DisplayName("测试连接(双击右侧编辑栏)")]
        [XmlIgnore]
        public bool TestConnection
        {
            get { return false; }
            set
            {
                MessageBox.Show("料号服务器测试成功");
            }
        }
        #endregion

        #region 料号数据库设置
        [Category("料号数据库设置"), PropertyOrder(0), DisplayName("服务器地址")]
        public string DBServerIP { get; set; }

        [Category("料号数据库设置"), PropertyOrder(1), DisplayName("服务器用户名")]
        [DefaultValue("sa")]
        public string DBUserName { get; set; }

        [Category("料号数据库设置"), PropertyOrder(2), DisplayName("服务器密码")]
        [PasswordPropertyText(true)]
        public string DBUserPwd { get; set; }

        [Category("料号数据库设置"), PropertyOrder(3), DisplayName("数据库名")]
        [TypeConverter(typeof(JobDBListConverter))]
        public string DBName { get; set; }

        [Category("料号数据库设置"), PropertyOrder(4), DisplayName("数据更新类型")]
        [DefaultValue(MDUpdateType.NONE)]
        public MDUpdateType UpdateType { get; set; }

        [Category("料号数据库设置"), PropertyOrder(5), DisplayName("数据库容量")]
        [DefaultValue(10000)]
        [Description("最大料号数量")]
        public int DBSystemCapacity { get; set; }

        /// <summary>
        /// 枚举可用的数据库列表
        /// </summary>
        public class JobDBListConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                List<string> DBList = new List<string>() { "A", "B" };
                return new StandardValuesCollection(DBList.ToArray());
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }
        }
        #endregion

        #region 用户管理
        [Category("用户管理"), PropertyOrder(0), DisplayName("SE用户密码")]
        [Editor(typeof(ChangePasswordEditor), typeof(UITypeEditor))]
        public JobManagerPassword SEPassword { get; set; }
        public class JobManagerPassword : PersistPassword
        {
            private static readonly byte[] key = Encoding.UTF8.GetBytes("SEPassword".Substring(0, 8));
            private static readonly byte[] iv = Encoding.UTF8.GetBytes("drowssaPES".Substring(0, 8));

            public override object Clone()
            {
                return new JobManagerPassword { Password = this.Password };
            }

            protected override string Encrypt(string plaincode)
            {
                return Tools.EncryptDES(plaincode, key, iv);
            }
        }

        [Category("用户管理"), PropertyOrder(1), DisplayName("缺省启动用户")]
        [DefaultValue(UserGroup.SE)]
        public UserGroup DefaultUser { get; set; }

        [Category("用户管理"), PropertyOrder(2), DisplayName("OP用户料号显示天数")]
        [Description("零或负数表示全部显示")]
        [DefaultValue(3)]
        public int OPShowDays { get; set; }
        #endregion

        #region 板尺寸控制
        [Category("板尺寸控制"), PropertyOrder(0), DisplayName("超大板板尺寸限制")]
        public BoardLimitSetting MergeTwinTableLimit { get; set; }

        [Category("板尺寸控制"), PropertyOrder(1), DisplayName("双台面板尺寸限制")]
        public BoardLimitSetting HandTwinTableLimit { get; set; }

        [Category("板尺寸控制"), PropertyOrder(2), DisplayName("是否超大板")]
        [ReadOnly(true)]
        public bool IsLargeBoard { get; set; }

        private BoardLimitSetting CurrentTableLimit { get { return IsLargeBoard ? MergeTwinTableLimit : HandTwinTableLimit; } }

        [Browsable(false), XmlIgnore]
        public decimal BoardWidthMax { get { return CurrentTableLimit.WidthBorder.Max; } set { CurrentTableLimit.WidthBorder.Max = value; } }

        [Browsable(false), XmlIgnore]
        public decimal BoardWidthMin { get { return CurrentTableLimit.WidthBorder.Min; } set { CurrentTableLimit.WidthBorder.Min = value; } }

        [Browsable(false), XmlIgnore]
        public decimal BoardHeightMax { get { return CurrentTableLimit.HeightBorder.Max; } set { CurrentTableLimit.HeightBorder.Max = value; } }

        [Browsable(false), XmlIgnore]
        public decimal BoardHeightMin { get { return CurrentTableLimit.HeightBorder.Min; } set { CurrentTableLimit.HeightBorder.Min = value; } }

        [Browsable(false), XmlIgnore]
        public decimal BoardThicknessMax { get { return CurrentTableLimit.ThicknessBorder.Max; } set { CurrentTableLimit.ThicknessBorder.Max = value; } }

        [Browsable(false), XmlIgnore]
        public decimal BoardThicknessMin { get { return CurrentTableLimit.ThicknessBorder.Min; } set { CurrentTableLimit.ThicknessBorder.Min = value; } }
        #endregion

        #region 料号显示控制
        [Category("料号显示控制"), PropertyOrder(0), DisplayName("是否启用混合靶标")]
        [DefaultValue(false)]
        public bool BoardMixTargetEnable { get; set; }

        [Category("料号显示控制"), PropertyOrder(1), DisplayName("是否启用矩阵孔")]
        [DefaultValue(false)]
        public bool BoardMatrixEnable { get; set; }

        [Category("料号显示控制"), PropertyOrder(2), DisplayName("CCD默认曝光时间")]
        public decimal CcdExposureDefaultTime { get; set; }

        [Category("料号显示控制"), PropertyOrder(3), DisplayName("孔直径最大限制(mm)")]
        public decimal BoardHoleDiameter { get; set; }
        #endregion

        #region 料号列表显示配置
        [Category("料号列表显示配置"), PropertyOrder(0), DisplayName("干膜名称")]
        [DefaultValue(true)]
        public bool ResistNameVisabilityJobSystem { get; set; }

        [Category("料号列表显示配置"), PropertyOrder(1), DisplayName("干膜剂量")]
        [DefaultValue(true)]
        public bool ResistDosageVisabilityJobSystem { get; set; }

        [Category("料号列表显示配置"), PropertyOrder(2), DisplayName("序号（LayerId）")]
        [DefaultValue(true)]
        public bool LayerIdVisabilityJobSystem { get; set; }

        [Category("料号列表显示配置"), PropertyOrder(3), DisplayName("机器名")]
        [DefaultValue(true)]
        public bool MachineVisabilityJobSystem { get; set; }

        [Category("料号列表显示配置"), PropertyOrder(4), DisplayName("优先级")]
        [DefaultValue(true)]
        public bool PriorityVisabilityExposureSystem { get; set; }

        [Category("料号列表显示配置"), PropertyOrder(5), DisplayName("锁定")]
        [DefaultValue(true)]
        public bool LockedVisabilityExposureSystem { get; set; }
        #endregion

        #region 基础信息
        [Category("基础信息"), PropertyOrder(0), DisplayName("涨缩单位使用Mil（否则使用百分之）")]
        public bool IsScaleUnitUseMil { get; set; }

        [Category("基础信息"), PropertyOrder(1), DisplayName("最左侧SMARK与吸盘左边缘的距离（毫米）")]
        public double SMarkLeftMargin { get; set; }

        [Category("基础信息"), PropertyOrder(2), DisplayName("最右侧SMARK与吸盘右边缘的距离（毫米）")]
        public double SMarkRightMargin { get; set; }

        [Category("基础信息"), PropertyOrder(5), DisplayName("可用涨缩列表")]
        [Description("用户可根据需要设置涨缩类型的集合，避免因选错涨缩类型带来损失。")]
        public ScaleMode[] EnableScaleModes { get; set; }

        [Category("基础信息"), PropertyOrder(6), DisplayName("是否允许所有机器")]
        [Description("料号Machine设置时，是否可以选择All。")]
        [DefaultValue(true)]
        public bool IsContainAll { get; set; }

        [Category("基础信息"), PropertyOrder(7), DisplayName("是否复制料号所有数据")]
        [Description("复制料号时，是否复制GDS文件。")]
        public List<int> IsCopyAll { get; set; }


        #endregion

        #region 自动料号模版配置

        #endregion

        #region 自动料号系统参数配置
        [Category("自动料号系统参数配置"), PropertyOrder(0), DisplayName("是否启用自动料号")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool IsEnableAutoJobMake { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(1), DisplayName("EZSERVER服务配置文件路径")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"c:\ezcam\share\.comms\server")]
        [Browsable(true)]
        public string FileEZServerConfigPath { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(2), DisplayName("ezServerd服务程序路径")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"c:\ezcam\1.1\ezServerd.exe")]
        [Browsable(true)]
        public string ExeEZServerPath { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(3), DisplayName("Gateway程序路径")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"c:\ezcam\1.1\bin\gateway.exe")]
        [Browsable(true)]
        public string ExeGatewayPath { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(4), DisplayName("AutoJob脚本存放路径")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"c:\ezcam\1.1\scripts\autojob.csh")]
        [Browsable(true)]
        public string FileAutojobPath { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(5), DisplayName("EZCAM解压后JOBS存放路径")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"c:\ezcam\fw\jobs")]
        [Browsable(true)]
        public string DirJobProvideForEZCAM { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(6), DisplayName("GDS输出路径")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"c:\ezcam\tmp")]
        [Browsable(true)]
        public string DirJobOutputByEZCAM { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(7), DisplayName("jobname等脚本参数文件路径")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"c:\ezcam\1.1\scripts\jobname.txt")]
        [Browsable(true)]
        public string FileScriptParamForAutoJob { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(8), DisplayName("JobTxt文件名格式")]
        [DefaultValue("{jobname}")]
        [Browsable(true)]
        [Description("如料号名为：a.tgz, Txt文件名为：a_LDI.txt, 则格式字符应为：{jobname}_LDI")]
        public string JobTxtFormat { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(9), DisplayName("TGZ资料库路径")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"e:\autojob")]
        [Browsable(true)]
        public string DirSharedJobPath { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(10), DisplayName("是否启用自动比对")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool IsEnableCompare { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(11), DisplayName("自动比对结果存放路径")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"c:\ezcam\tmp\autojob")]
        [Browsable(true)]
        public string DirCompareResultPath { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(12), DisplayName("最大上料数量")]
        [DefaultValue(1)]
        [Browsable(true)]
        public int MaxCountToMake { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(13), DisplayName("自动料号备份根目录")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"d:\jobwithlog")]
        [Browsable(true)]
        public string DirJobWithLog { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(14), DisplayName("自动料号料号备份根目录")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"d:\jobwithlog\jobbackup")]
        [Browsable(true)]
        public string DirJobBackup { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(15), DisplayName("自动料号料号正常导入备份目录")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"d:\jobwithlog\jobbackup\correctjobbackup")]
        [Browsable(true)]
        public string DirCorrrectJobBackup { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(16), DisplayName("自动料号料号错误导入备份目录")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"d:\jobwithlog\jobbackup\errorjobbackup")]
        [Browsable(true)]
        public string DirErrorJobBackup { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(17), DisplayName("自动料号料号拦截导入备份目录")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"d:\jobwithlog\jobbackup\interceptjobbackup")]
        [Browsable(true)]
        public string DirInterceptJobBackup { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(18), DisplayName("自动料号日志根目录")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"d:\jobwithlog\autolog")]
        [Browsable(true)]
        public string DirJobLogPath { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(19), DisplayName("自动料号错误日志目录")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"d:\jobwithlog\autolog\errorlog")]
        [Browsable(true)]
        public string DirJobErrorLogPath { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(20), DisplayName("自动料号正确日志目录")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"d:\jobwithlog\autolog\correctlog")]
        [Browsable(true)]
        public string DirJobCorrectLogPath { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(21), DisplayName("自动料号拦截日志目录")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"d:\jobwithlog\autolog\interceptlog")]
        [Browsable(true)]
        public string DirJobInterceptLogPath { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(22), DisplayName("自动料号用户管理")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [DefaultValue(@"d:\jobwithlog\autolog\userinfo.dat")]
        [Browsable(true)]
        public string FileUserInfoPath { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(23), DisplayName("PE冲孔涨缩是否启用")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool IsEnablePEScaleMode { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(24), DisplayName("PE冲孔symbol集合")]
        [DefaultValue("( attr:.out_scale )")]
        [Browsable(true)]
        [Description("格式必须有()，在小括号内填写具体的symbol集合，以逗号分割")]
        public string PEScalesCollection { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(25), DisplayName("PE冲孔涨缩存在时，脚本写入涨缩的symbol")]
        [DefaultValue("ldiscale")]
        [Browsable(true)]
        public string PEScaleSymbol { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(26), DisplayName("是否启用mil涨缩")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool IsEnableMilScale { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(27), DisplayName("Txt是否从tgz中抽取")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool IsExtractTxtFromTGZ { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(28), DisplayName("Txt从TGZ抽取时，Txt路径")]
        [DefaultValue("{jobname}/user/inner_scale")]
        [Browsable(true)]
        [Description("{jobname}不可省略，代表的是压缩包根目录")]
        public string TxtPathInTGZ { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(29), DisplayName("X阈值(inch)")]
        [DefaultValue(0)]
        [Browsable(true)]
        [Description("板宽大于该阈值时，图形旋转")]
        public double ThreasholdX { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(30), DisplayName("Y阈值(inch)")]
        [DefaultValue(0)]
        [Browsable(true)]
        [Description("板高小于该阈值时，图形旋转")]
        public double ThreasholdY { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(31), DisplayName("默认涨缩类型")]
        [DefaultValue(ScaleMode.AutoScale)]
        [Browsable(true)]
        [Description("当TXT中给出的涨缩值为0,0时，默认的涨缩类型")]
        public ScaleMode DefaultScaleMode { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(32), DisplayName("TXT中给出的缩边值的单位")]
        [DefaultValue("mm")]
        [Browsable(true)]
        public string IndentUnit { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(33), DisplayName("要删除的symbol集合")]
        [DefaultValue("1,2,3")]
        [Browsable(true)]
        [Description("要删除的symbol集合，用逗号分割，不可为空")]
        public string DeleteSymbolCollection { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(34), DisplayName("要反转极性的symbol集合")]
        [DefaultValue("1,2,3")]
        [Browsable(true)]
        [Description("要反转极性的symbol集合，用逗号分割，不可为空")]
        public string InvertSymbolCollection { get; set; }

        [Category("自动料号系统参数配置"), PropertyOrder(34), DisplayName("是否覆盖已存在料号")]
        [DefaultValue(true)]
        [Browsable(true)]
        [Description("覆盖已存在料号时，在导入新料号的时候会进行检查，如果发现数据库中有该料号，会自动删除")]
        public bool IsAutoDeleteSameJob { get; set; }
        #endregion

        public JobManagerSetting()
        {
            // 设置DefaultValue
            Tools.SetDefaultValueOfAttribute(this);

            // 无法使用DefaultValue：decimal
            CcdExposureDefaultTime = 10M;
            BoardHoleDiameter = 6M;

            // 无法使用DefaultValue：对象
            SEPassword = new JobManagerPassword();
            SEPassword.Set("RESET");
            HandTwinTableLimit = new BoardLimitSetting(
                        new BorderValue<decimal>(300, 623M),
                        new BorderValue<decimal>(100M, 800M),
                        new BorderValue<decimal>(0.05M, 5M));
            MergeTwinTableLimit = new BoardLimitSetting(
                        new BorderValue<decimal>(600, 1600M),
                        new BorderValue<decimal>(100M, 800M),
                        new BorderValue<decimal>(0.05M, 5M));
        }

        public void OnSave()
        {
        }

        #region 静态对象
        static JobManagerSetting()
        {
            Dialog = new XmlPersistObjSettingForm<JobManagerSetting>("料号系统设置", "JobManagerSetting.xml");
        }

        public static XmlPersistObjSettingForm<JobManagerSetting> Dialog { get; private set; }

        public static JobManagerSetting Inst { get { return Dialog.Setting; } }
        #endregion
    }
}
