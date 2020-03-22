using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace MSC.Common
{
    /// <summary>
    /// 对象的序列化和反序列化工具类
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// 序列化：object -> bytes
        /// 注：Class需要添加[Serializable]
        /// </summary>
        public static byte[] Serialize(object value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, value);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 反序列化：byes -> object
        /// 注：Class需要添加[Serializable]
        /// </summary>
        public static object Deserialize(byte[] value)
        {
            if (value == null) return null;
            using (MemoryStream stream = new MemoryStream(value))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 序列化：object -> 二进制文件
        /// 注：Class需要添加[Serializable]
        /// </summary>
        public static void SerializeToBinFile(object value, string fileName)
        {
            using (var ms = new FileStream(fileName, FileMode.CreateNew))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, value);
            }
        }

        /// <summary>
        /// 反序列化：二进制文件 -> object
        /// 注：Class需要添加[Serializable]
        /// </summary>
        public static object DeserializeFromBinFile(string fileName)
        {
            using (var ms = new FileStream(fileName, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 通过二进制序列化的方式Clone对象
        /// 注：Class需要添加[Serializable]
        /// </summary>
        public static object CloneByBinarySerialize(object value)
        {
            return Deserialize(Serialize(value));
        }

        // 序列化：对象 -> Xml文件
        public static void SerializeToXmlFile(object obj, string file)
        {
            using (TextWriter writer = new StreamWriter(file))
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj);
            }
        }

        // 反序列化：Xml文本 -> 对象
        public static TClass DeserializeFromXmlFile<TClass>(string file) where TClass : new()
        {
            if (!File.Exists(file))
            {
                return new TClass();
            }
            try
            {
                using (FileStream reader = new FileStream(file, FileMode.Open))
                {
                    XmlSerializer xmlSrlzer = new XmlSerializer(typeof(TClass));
                    return (TClass)xmlSrlzer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                ex.Source = ex.Source;
                return new TClass();
            }
        }

        /// <summary>
        /// DES对称加密解密：bytes
        /// </summary>
        /// <param name="encrypt">true: 加密；false: 解密</param>
        public static byte[] EncryptDES(byte[] input, byte[] key, byte[] iv, bool encrypt = true)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider() { Key = key, IV = iv };
            ICryptoTransform transform = encrypt ? des.CreateEncryptor() : des.CreateDecryptor();
            using (MemoryStream ms = new MemoryStream())
            {
                CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();
                cs.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// DES对称加密解密：string <-> Base64String
        /// </summary>
        /// <param name="encrypt">true: 加密；false: 解密</param>
        public static string EncryptDES(string text, byte[] key, byte[] iv, bool encrypt = true)
        {
            try
            {
                byte[] input = encrypt ? Encoding.UTF8.GetBytes(text) : Convert.FromBase64String(text);
                byte[] output = EncryptDES(input, key, iv, encrypt);
                string result = encrypt ? Convert.ToBase64String(output) : Encoding.UTF8.GetString(output);
                return result;
            }
            catch (Exception ex)
            {
                ex.Source = ex.Source;
                return "";
            }
        }

        /// <summary>
        /// MD5 16位加密
        /// </summary>
        public static string EncryptMD516(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }

        /// <summary>
        /// MD5 32位加密
        /// </summary>
        public static string EncryptMD532(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }

        public static bool CheckIsGuid(string strGuid)
        {
            try
            {
                var guid = new Guid(strGuid);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 设置对象属性的特性值，可用于动态修改PropertyGrid的Browsable特性
        /// </summary>
        public static void SetPropertyAttribute(object obj, string propertyName, Type attrType, string attrField, object value)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            Attribute attr = props[propertyName].Attributes[attrType];
            FieldInfo field = attrType.GetField(attrField, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(attr, value);
        }

        /// <summary>
        /// 给属性设置DefaultValueAttribute配置的值
        /// </summary>
        public static void SetDefaultValueOfAttribute(object obj)
        {
            Type attrType = typeof(DefaultValueAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            foreach (PropertyDescriptor prop in props)
            {
                DefaultValueAttribute attr = (DefaultValueAttribute)prop.Attributes[attrType];
                if (attr != null)
                {
                    prop.SetValue(obj, attr.Value);
                }
            }
        }

        /// <summary>
        /// 使用rar压缩指定文件夹 
        /// </summary>
        /// <param name="dir">压缩文件的存放目录</param>
        /// <param name="srcFolderName">需要压缩的源文件夹或文件，包括路径</param>
        /// <param name="compressedFileName">压缩后的文件名，如为空则用源文件夹名或文件名命名</param>
        /// <param name="deleteSrc">压缩后是否删除源文件夹或文件，默认是删除</param>
        public static void Compress(string dir, string srcFolderName, string compressedFileName = "", bool deleteSrc = true)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true; //win7上必须要设置，否则命令行没有成功执行；在win10上却可以不用设置
            Process rarPro = new Process() { StartInfo = psi };
            rarPro.Start();
            rarPro.StandardInput.WriteLine("cd /d " + dir);
            string targetFileName = string.IsNullOrEmpty(compressedFileName) ? new DirectoryInfo(srcFolderName).Name : compressedFileName;
            string delSwitcher = deleteSrc ? "-df" : "";
            string cmd = string.Format("{0}rar.exe a -k -r -inul {1} {2}.rar {3}", AppDomain.CurrentDomain.BaseDirectory, delSwitcher, targetFileName, srcFolderName);
            rarPro.StandardInput.WriteLine(cmd);
            rarPro.StandardInput.WriteLine("exit");
            rarPro.WaitForExit();
        }

        public static string GetEnumDescription(this Enum item)
        {
            string result = item.ToString();
            FieldInfo field = item.GetType().GetField(result);
            DescriptionAttribute description = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (description != null) result = description.Description;
            return result;
        }
    }
}