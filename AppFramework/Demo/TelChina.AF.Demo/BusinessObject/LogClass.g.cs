using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    public partial class LogClass : EntityBase
    {
        public override string EntityComponent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// 操作
        /// </summary>
        private string operating;
        /// <summary>
        /// 操作
        /// </summary>
        public virtual string Operating
        {
            get { return this.operating; }
            set
            {
                if (this.operating != value)
                {
                    RaisPropertyChangIngEvent("Operating");
                    operating = value;
                    RaisPropertyChangedEvent("Operating");
                }
            }
        }

        private Byte[] byteImage;

        public virtual Byte[] ByteImage
        {
            get
            {
                return byteImage;
            }
            set
            {
                if (this.byteImage != value)
                {
                    RaisPropertyChangIngEvent("ByteImage");
                    byteImage = value;
                    RaisPropertyChangedEvent("ByteImage");
                }
            }
        }
    }
}
