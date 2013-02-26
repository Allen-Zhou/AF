using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;
using AutoMapper;

namespace TelChina.AF.Demo
{
    public partial class User : EntityBase
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

        protected override void SetDefaultValue()
        {

        }

        protected override void OnValidate()
        {

        }

        protected override void OnInserting()
        {

        }

        protected override void OnInserted()
        {

        }

        protected override void OnUpdating()
        {

        }

        protected override void OnUpdated()
        {

        }

        protected override void OnDeleting()
        {

        }

        protected override void OnDeleted()
        {

        }

        public virtual User ToBE(UserDTO dto)
        {
            User be = DTOConvert.ToBE<User>(dto);
            return be;
        }

        public virtual UserDTO TransitiontoDto(User be)
        {
            Mapper.CreateMap<User, UserDTO>();

            UserDTO dto = (UserDTO)Mapper.Map<User, UserDTO>(be);

            return dto;
        }

        public virtual User TransitiontoBE(UserDTO dto)
        {
            Mapper.CreateMap<UserDTO, User>();

            User be = (User)Mapper.Map<UserDTO, User>(dto);

            return be;
        }

        public virtual UserDTO ToDTO(User be)
        {
            UserDTO dto = DTOConvert.ToDTO<UserDTO>(be);
            return dto;
        }

        public virtual User ReturnUser()
        {
            User user = new User();
            user.ID = Guid.NewGuid();
            user.Code = "XT-001";
            user.Address = "SuiZ";
            user.Amount = decimal.Parse("121.00");
            user.Birthday = DateTime.Now;
            user.CreatedBy = "DS";
            user.CreatedOn = DateTime.Now;
            user.Disabled = false;
            user.Gender = true;
            user.Names = "ZS";
            user.NativePlace = "han";
            user.PostCode = "441309";
            user.Price = decimal.Parse("12.10");
            user.Quantity = decimal.Parse("10.00");
            user.SumTotal = 10;
            user.SysVersion = 1;
            user.Telphone = "131589461";
            user.UpdatedBy = "ZL";
            user.UpdatedOn = DateTime.Now;
            user.UserAddress = "UserSuiz";
            user.UserAmount = decimal.Parse("225.00");
            user.UserBirthday = DateTime.Now;
            user.UserCode = "UserXT-001";
            user.UserCreateBy = "UserDS";
            user.UserCreateOn = DateTime.Now;
            user.UserDisabled = true;
            user.UserGender = false;
            user.UserID = Guid.NewGuid(); ;
            user.UserName = "UserZS";
            user.UserNativePlace = "Userhan";
            user.UserPostCode = "User441309";
            user.UserPrice = decimal.Parse("1.50");
            user.UserQuantity = decimal.Parse("150.00");
            user.UserSumTotal = 12;
            user.UserTelphone = "User131589461";
            user.UserUpdateBy = "UserZL";
            user.UserUpdateOn = DateTime.Now;
            return user;
        }

        public virtual UserDTO ReturnUserDTO()
        {
            UserDTO user = new UserDTO();
            user.ID = Guid.NewGuid();
            user.Code = "XT-001";
            user.Address = "SuiZ";
            user.Amount = decimal.Parse("121.00");
            user.Birthday = DateTime.Now;
            user.CreatedBy = "DS";
            user.CreatedOn = DateTime.Now;
            user.Disabled = false;
            user.Gender = true;
            user.Names = "ZS";
            user.NativePlace = "han";
            user.PostCode = "441309";
            user.Price = decimal.Parse("12.10");
            user.Quantity = decimal.Parse("10.00");
            user.SumTotal = 10;
            user.SysVersion = 1;
            user.Telphone = "131589461";
            user.UpdatedBy = "ZL";
            user.UpdatedOn = DateTime.Now;
            user.UserAddress = "UserSuiz";
            user.UserAmount = decimal.Parse("225.00");
            user.UserBirthday = DateTime.Now;
            user.UserCode = "UserXT-001";
            user.UserCreateBy = "UserDS";
            user.UserCreateOn = DateTime.Now;
            user.UserDisabled = true;
            user.UserGender = false;
            user.UserID = Guid.NewGuid(); ;
            user.UserName = "UserZS";
            user.UserNativePlace = "Userhan";
            user.UserPostCode = "User441309";
            user.UserPrice = decimal.Parse("1.50");
            user.UserQuantity = decimal.Parse("150.00");
            user.UserSumTotal = 12;
            user.UserTelphone = "User131589461";
            user.UserUpdateBy = "UserZL";
            user.UserUpdateOn = DateTime.Now;
            return user;
        }

    }
}
