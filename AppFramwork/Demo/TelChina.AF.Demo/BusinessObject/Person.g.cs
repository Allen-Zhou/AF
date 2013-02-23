using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    public partial class Person : EntityBase
    {
        #region 属性

        /// <summary>
        /// 编号
        /// </summary>
        private string code;

        /// <summary>
        /// 编号
        /// </summary>
        [PropertyMetaDataAttribute(AllowNull = false, Length = 15)]
        public virtual string Code
        {
            get { return this.code; }
            set
            {
                if ((this.Code != null && this.code.Trim() != value) || (this.Code == null && value != null))
                {
                    RaisPropertyChangIngEvent("Code");

                    code = value;
                    RaisPropertyChangedEvent("Code");
                }
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        private string name;

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    RaisPropertyChangIngEvent("Name");
                    name = value;
                    RaisPropertyChangedEvent("Name");
                }
            }
        }

        /// <summary>
        /// 电话
        /// </summary>
        private string telphone;

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Telphone
        {
            get { return this.telphone; }
            set
            {
                if (this.telphone != value)
                {
                    RaisPropertyChangIngEvent("Telphone");
                    telphone = value;
                    RaisPropertyChangedEvent("Telphone");
                }
            }
        }
        /// <summary>
        /// 地址
        /// </summary>
        private string address;

        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Address
        {
            get { return this.address; }
            set
            {
                if (this.address != value)
                {
                    RaisPropertyChangIngEvent("Address");
                    address = value;
                    RaisPropertyChangedEvent("Address");
                }
            }
        }

        /// <summary>
        /// 邮政编码
        /// </summary>
        private string postCode;

        /// <summary>
        /// 邮政编码
        /// </summary>
        public virtual string PostCode
        {
            get
            {
                return postCode;
            }
            set
            {
                if (this.postCode != value)
                {
                    RaisPropertyChangIngEvent("PostCode");
                    postCode = value;
                    RaisPropertyChangedEvent("PostCode");
                }
            }
        }

        /// <summary>
        /// 生日
        /// </summary>
        private DateTime birthday;
        /// <summary>
        /// 生日
        /// </summary>
        public virtual DateTime Birthday
        {
            get
            {
                return birthday;
            }
            set
            {
                if (this.birthday != value)
                {
                    RaisPropertyChangIngEvent("Birthday");
                    birthday = value;
                    RaisPropertyChangedEvent("Birthday");
                }
            }
        }

        /// <summary>
        /// 性别
        /// </summary>
        private bool gender;

        /// <summary>
        /// 性别
        /// </summary>
        public virtual bool Gender
        {
            get
            {
                return gender;
            }
            set
            {
                if (this.gender != value)
                {
                    RaisPropertyChangIngEvent("Gender");
                    gender = value;
                    RaisPropertyChangedEvent("Gender");
                }
            }
        }
        /// <summary>
        /// 籍贯
        /// </summary>
        private string nativePlace;

        /// <summary>
        /// 籍贯
        /// </summary>
        public virtual string NativePlace
        {
            get
            {
                return nativePlace;
            }
            set
            {
                if (this.nativePlace != value)
                {
                    RaisPropertyChangIngEvent("NativePlace");
                    nativePlace = value;
                    RaisPropertyChangedEvent("NativePlace");
                }
            }
        }

        /// <summary>
        /// 停用
        /// </summary>
        private bool disabled;

        /// <summary>
        /// 停用
        /// </summary>
        public virtual bool Disabled
        {
            get
            {
                return disabled;
            }
            set
            {
                if (this.disabled != value)
                {
                    RaisPropertyChangIngEvent("Disabled");
                    disabled = value;
                    RaisPropertyChangedEvent("Disabled");
                }
            }
        }

        private string entityComponet;

        public override string EntityComponent
        {
            get
            {
                return entityComponet;
            }
            set
            {
                this.entityComponet = value;
            }
        }

        private Guid _idDepartment ;

        /// <summary>
        /// 所属部门ID
        /// </summary>
        public virtual Guid idDepartment
        {
            get { return _idDepartment; }
            set { _idDepartment = value; }
        }

        /// <summary>
        /// 所属部门
        /// </summary>
        private Department _department;

        public virtual Department Department
        {
            get { return _department; }
            set { _department = value; }
        }
        #endregion
    }
}
