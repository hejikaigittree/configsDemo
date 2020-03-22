using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MSC.Common
{
    public abstract class PersistPassword : ICloneable
    {
        /// <summary>
        /// 加密码：用于XML保存
        /// </summary>
        public string Password { get; set; }

        public void Set(string plaincode)
        {
            Password = Encrypt(plaincode);
        }

        public bool Verify(string plaincode)
        {
            return Password == Encrypt(plaincode);
        }

        public override string ToString() { return "******"; }

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        protected abstract string Encrypt(string plaincode);
    }
}
