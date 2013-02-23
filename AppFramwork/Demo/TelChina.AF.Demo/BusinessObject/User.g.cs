using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    public partial class User : EntityBase
    {
        private DateTime _birthday;
        private Name _name;

        public virtual DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                RaisPropertyChangIngEvent("Birthday");
                _birthday = value;
                RaisPropertyChangedEvent("Birthday");
            }
        }

        public virtual Name Name
        {
            get { return _name; }
            set 
            {
                RaisPropertyChangIngEvent("Name");
                _name = value;
                RaisPropertyChangedEvent("Name");
            }
        }

        /// <summary>
        /// 编号
        /// </summary>
        private string code;

        /// <summary>
        /// 编号
        /// </summary>
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
        private string names;
        public virtual string Names
        {
            get { return names; }
            set
            {
                RaisPropertyChangIngEvent("Names");
                names = value;
                RaisPropertyChangedEvent("Names");
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

                RaisPropertyChangIngEvent("Telphone");
                telphone = value;
                RaisPropertyChangedEvent("Telphone");

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

                RaisPropertyChangIngEvent("Address");
                address = value;
                RaisPropertyChangedEvent("Address");

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

                RaisPropertyChangIngEvent("PostCode");
                postCode = value;
                RaisPropertyChangedEvent("PostCode");
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
                RaisPropertyChangIngEvent("Gender");
                gender = value;
                RaisPropertyChangedEvent("Gender");
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
                RaisPropertyChangIngEvent("NativePlace");
                nativePlace = value;
                RaisPropertyChangedEvent("NativePlace");
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
                RaisPropertyChangIngEvent("Disabled");
                disabled = value;
                RaisPropertyChangedEvent("Disabled");
            }
        }

        private Decimal amount;

        /// <summary>
        /// 金额
        /// </summary>
        public virtual Decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                RaisPropertyChangIngEvent("Amount");
                amount = value;
                RaisPropertyChangedEvent("Amount");
            }
        }

        private Decimal quantity;

        /// <summary>
        /// 数量
        /// </summary>
        public virtual Decimal Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                RaisPropertyChangIngEvent("Quantity");
                quantity = value;
                RaisPropertyChangedEvent("Quantity");
            }
        }

        private Decimal price;

        /// <summary>
        /// 单价
        /// </summary>
        public virtual Decimal Price
        {
            get
            {
                return price;
            }
            set
            {
                RaisPropertyChangIngEvent("Price");
                price = value;
                RaisPropertyChangedEvent("Price");
            }
        }

        private int sumTotal;

        /// <summary>
        /// 总数
        /// </summary>
        public virtual int SumTotal
        {
            get
            {
                return sumTotal;
            }
            set
            {
                RaisPropertyChangIngEvent("SumTotal");
                sumTotal = value;
                RaisPropertyChangedEvent("SumTotal");
            }
        }

      
        private string userCode;

        /// <summary>
        /// 
        /// </summary>
        public virtual string UserCode
        {
            get { return this.userCode; }
            set
            {
                RaisPropertyChangIngEvent("UserCode");
                userCode = value;
                RaisPropertyChangedEvent("UserCode");
            }
        }

        private string userName;

        /// <summary>
        /// 
        /// </summary>
        public virtual string UserName
        {
            get { return this.userName; }
            set
            {
                RaisPropertyChangIngEvent("UserName");
                userName = value;
                RaisPropertyChangedEvent("UserName");
            }
        }

        private string userAddress;

        /// <summary>
        /// 
        /// </summary>
        public virtual string UserAddress
        {
            get { return this.userAddress; }
            set
            {
                RaisPropertyChangIngEvent("UserAddress");
                userAddress = value;
                RaisPropertyChangedEvent("UserAddress");
            }
        }

        private string userTelphone;

        /// <summary>
        /// 
        /// </summary>
        public virtual string UserTelphone
        {
            get { return this.userTelphone; }
            set
            {
                RaisPropertyChangIngEvent("UserTelphone");
                userTelphone = value;
                RaisPropertyChangedEvent("UserTelphone");
            }
        }

        /// <summary>
        /// 邮政编码
        /// </summary>
        private string uerPostCode;

        /// <summary>
        /// 邮政编码
        /// </summary>
        public virtual string UserPostCode
        {
            get
            {
                return uerPostCode;
            }
            set
            {

                RaisPropertyChangIngEvent("UserPostCode");
                uerPostCode = value;
                RaisPropertyChangedEvent("UserPostCode");
            }
        }

        /// <summary>
        /// 性别
        /// </summary>
        private bool userGender;

        /// <summary>
        /// 性别
        /// </summary>
        public virtual bool UserGender
        {
            get
            {
                return userGender;
            }
            set
            {
                RaisPropertyChangIngEvent("UserGender");
                userGender = value;
                RaisPropertyChangedEvent("UserGender");
            }
        }
        /// <summary>
        /// 籍贯
        /// </summary>
        private string userNativePlace;

        /// <summary>
        /// 籍贯
        /// </summary>
        public virtual string UserNativePlace
        {
            get
            {
                return userNativePlace;
            }
            set
            {
                RaisPropertyChangIngEvent("UserNativePlace");
                userNativePlace = value;
                RaisPropertyChangedEvent("UserNativePlace");
            }
        }

        /// <summary>
        /// 停用
        /// </summary>
        private bool userDisabled;

        /// <summary>
        /// 停用
        /// </summary>
        public virtual bool UserDisabled
        {
            get
            {
                return userDisabled;
            }
            set
            {
                RaisPropertyChangIngEvent("UserDisabled");
                userDisabled = value;
                RaisPropertyChangedEvent("UserDisabled");
            }
        }

        private Decimal userAmount;

        /// <summary>
        /// 金额
        /// </summary>
        public virtual Decimal UserAmount
        {
            get
            {
                return userAmount;
            }
            set
            {
                RaisPropertyChangIngEvent("UserAmount");
                userAmount = value;
                RaisPropertyChangedEvent("UserAmount");
            }
        }

        private Decimal userQuantity;

        /// <summary>
        /// 数量
        /// </summary>
        public virtual Decimal UserQuantity
        {
            get
            {
                return userQuantity;
            }
            set
            {
                RaisPropertyChangIngEvent("UserQuantity");
                userQuantity = value;
                RaisPropertyChangedEvent("UserQuantity");
            }
        }

        private Decimal userPrice;

        /// <summary>
        /// 单价
        /// </summary>
        public virtual Decimal UserPrice
        {
            get
            {
                return userPrice;
            }
            set
            {
                RaisPropertyChangIngEvent("UserPrice");
                userPrice = value;
                RaisPropertyChangedEvent("UserPrice");
            }
        }

        private Decimal userSumTotal;

        /// <summary>
        /// 单价
        /// </summary>
        public virtual Decimal UserSumTotal
        {
            get
            {
                return userSumTotal;
            }
            set
            {
                RaisPropertyChangIngEvent("UserSumTotal");
                userSumTotal = value;
                RaisPropertyChangedEvent("UserSumTotal");
            }
        }

        private DateTime userBirthday;

        public virtual DateTime UserBirthday
        {
            get { return userBirthday; }
            set
            {
                RaisPropertyChangIngEvent("UserBirthday");
                userBirthday = value;
                RaisPropertyChangedEvent("UserBirthday");
            }
        }

        private Guid userID;

        public virtual Guid UserID
        {
            get { return userID; }
            set
            {
                RaisPropertyChangIngEvent("UserID");
                userID = value;
                RaisPropertyChangedEvent("UserID");
            }
        }

        private string userCreateBy;

        public virtual string UserCreateBy
        {
            get { return userCreateBy; }
            set
            {
                RaisPropertyChangIngEvent("UserCreateBy");
                userCreateBy = value;
                RaisPropertyChangedEvent("UserCreateBy");
            }
        }

        private DateTime userCreateOn;

        public virtual DateTime UserCreateOn
        {
            get { return userCreateOn; }
            set
            {
                RaisPropertyChangIngEvent("UserCreateOn");
                userCreateOn = value;
                RaisPropertyChangedEvent("UserCreateOn");
            }
        }

        private string userUpdateBy;

        public virtual string UserUpdateBy
        {
            get { return userUpdateBy; }
            set
            {
                RaisPropertyChangIngEvent("UserUpdateBy");
                userUpdateBy = value;
                RaisPropertyChangedEvent("UserUpdateBy");
            }
        }

        private DateTime userUpdateOn;

        public virtual DateTime UserUpdateOn
        {
            get { return userUpdateOn; }
            set
            {
                RaisPropertyChangIngEvent("UserUpdateOn");
                userUpdateOn = value;
                RaisPropertyChangedEvent("UserUpdateOn");
            }
        }
    }
}
