using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Sys.DTO;
using System.Runtime.Serialization;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    [DataContract]
    [Serializable]
   public  class UserDTO : BizDTO
    {
        [DataMember]
        public virtual DateTime Birthday
        {
            get;
            set;
        }

        [DataMember]
        public virtual string Names
        {
            get;
            set;
        }
       
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public virtual string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 电话
        /// </summary>
        [DataMember]
        public virtual string Telphone
        {
            get;
            set;
        }       

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public virtual string Address
        {
            get;
            set;
        }
        
        /// <summary>
        /// 邮政编码
        /// </summary>
        [DataMember]
        public virtual string PostCode
        {
            get;
            set;
        }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public virtual bool Gender
        {
            get;
            set;
        }
       
        /// <summary>
        /// 籍贯
        /// </summary>
        [DataMember]
        public virtual string NativePlace
        {
            get;
            set;
        }


        /// <summary>
        /// 停用
        /// </summary>
        [DataMember]
        public virtual bool Disabled
        {
            get;
            set;
        }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public virtual Decimal Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public virtual Decimal Quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 单价
        /// </summary>
        [DataMember]
        public virtual Decimal Price
        {
            get;
            set;
        }

        /// <summary>
        /// 总数
        /// </summary>
        [DataMember]
        public virtual int SumTotal
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string UserCode
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public virtual string UserAddress
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public virtual string UserTelphone
        {
            get;
            set;
        }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [DataMember]
        public virtual string UserPostCode
        {
            get;
            set;
        }
        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public virtual bool UserGender
        {
            get;
            set;
        }

        /// <summary>
        /// 籍贯
        /// </summary>
        [DataMember]
        public virtual string UserNativePlace
        {
            get;
            set;
        }
        /// <summary>
        /// 停用
        /// </summary>
        [DataMember]
        public virtual bool UserDisabled
        {
            get;
            set;
        }

        /// <summary>
        /// 金额
        /// </summary>
        public virtual Decimal UserAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public virtual Decimal UserQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// 单价
        /// </summary>
        [DataMember]
        public virtual Decimal UserPrice
        {
            get;
            set;
        }
        [DataMember]
        public virtual Decimal UserSumTotal
        {
            get;
            set;
        }

        [DataMember]
        public virtual DateTime UserBirthday
        {
            get;
            set;
        }

        [DataMember]
        public virtual Guid UserID
        {
            get;
            set;
        }

        [DataMember]
        public virtual string UserCreateBy
        {
            get;
            set;
        }

        [DataMember]
        public virtual DateTime UserCreateOn
        {
            get;
            set;
        }

        [DataMember]
        public virtual string UserUpdateBy
        {
            get;
            set;
        }

        [DataMember]
        public virtual DateTime UserUpdateOn
        {
            get;
            set;
        }

        public virtual User ToBE(UserDTO dto)
        {
            User user = new User();

            user.ID = dto.ID;
            user.Code = dto.Code;
            user.Address = dto.Address;
            user.Amount = dto.Amount;
            user.Birthday = dto.Birthday;
            user.CreatedBy = dto.CreatedBy;
            user.CreatedOn = dto.CreatedOn;
            user.Disabled = dto.Disabled;
            user.Gender = dto.Gender;
            user.Names = dto.Names;
            user.NativePlace = dto.NativePlace;
            user.PostCode = dto.PostCode;
            user.Price = dto.Price;
            user.Quantity = dto.Quantity;
            user.SumTotal = dto.SumTotal;
            user.SysVersion = dto.SysVersion;
            user.Telphone = dto.Telphone;
            user.UpdatedBy = dto.UpdatedBy;
            user.UpdatedOn = dto.UpdatedOn;
            user.UserAddress = dto.UserAddress;
            user.UserAmount = dto.UserAmount;
            user.UserBirthday = dto.UserBirthday;
            user.UserCode = dto.UserCode;
            user.UserCreateBy = dto.UserCreateBy;
            user.UserCreateOn = dto.UserCreateOn;
            user.UserDisabled = dto.UserDisabled;
            user.UserGender = dto.UserGender;
            user.UserID = dto.UserID;
            user.UserName = dto.UserName;
            user.UserNativePlace = dto.UserNativePlace;
            user.UserPostCode = dto.UserPostCode;
            user.UserPrice = dto.UserPrice;
            user.UserQuantity = dto.UserQuantity;
            user.UserSumTotal = dto.UserSumTotal;
            user.UserTelphone = dto.UserTelphone;
            user.UserUpdateBy = dto.UserUpdateBy;
            user.UserUpdateOn = dto.UserUpdateOn;            
            return user;            
        }

        public virtual UserDTO ToDTO(User user)
        {
            UserDTO dto = new UserDTO();

            dto.ID = user.ID;
            dto.Code = user.Code;
            dto.Address = user.Address;
            dto.Amount = user.Amount;
            dto.Birthday = user.Birthday;
            dto.CreatedBy = user.CreatedBy;
            dto.CreatedOn = user.CreatedOn;
            dto.Disabled = user.Disabled;
            dto.Gender = user.Gender;
            dto.Names = user.Names;
            dto.NativePlace = user.NativePlace;
            dto.PostCode = user.PostCode;
            dto.Price = user.Price;
            dto.Quantity = user.Quantity;
            dto.SumTotal = user.SumTotal;
            dto.SysVersion = user.SysVersion;
            dto.Telphone = user.Telphone;
            dto.UpdatedBy = user.UpdatedBy;
            dto.UpdatedOn = user.UpdatedOn;
            dto.UserAddress = user.UserAddress;
            dto.UserAmount = user.UserAmount;
            dto.UserBirthday = user.UserBirthday;
            dto.UserCode = user.UserCode;
            dto.UserCreateBy = user.UserCreateBy;
            dto.UserCreateOn = user.UserCreateOn;
            dto.UserDisabled = user.UserDisabled;
            dto.UserGender = user.UserGender;
            dto.UserID = user.UserID;
            dto.UserName = user.UserName;
            dto.UserNativePlace = user.UserNativePlace;
            dto.UserPostCode = user.UserPostCode;
            dto.UserPrice = user.UserPrice;
            dto.UserQuantity = user.UserQuantity;
            dto.UserSumTotal = user.UserSumTotal;
            dto.UserTelphone = user.UserTelphone;
            dto.UserUpdateBy = user.UserUpdateBy;
            dto.UserUpdateOn = user.UserUpdateOn;
            return dto;
        }
    }
}
