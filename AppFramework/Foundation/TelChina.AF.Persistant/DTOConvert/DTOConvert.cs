using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Sys.DTO;
using System.Reflection;

namespace TelChina.AF.Persistant
{
    public class DTOConvert
    {
        private static DtoToBeConvertor dtoToBe = new DtoToBeConvertor();
        private static BeToDtoConvertor beToDto = new BeToDtoConvertor();

        public static M ToBE<M>(DTOBase dto)
               where M : EntityBase, new()
        {
            return dtoToBe.Convert<M>(dto);
        }


        public static M ToDTO<M>(EntityBase be)
            where M : DTOBase, new()
        {
            return beToDto.Convert<M>(be);
        }

        class BeToDtoConvertor : AbstractBE_DTOConvertor
        {
            public virtual M Convert<M>(EntityBase be)
            where M : DTOBase, new()
            {
                return (M)BeToDto(be, typeof(M));
            }

            private DTOBase BeToDto(EntityBase be, Type dtoType)
            {
                DTOBase dto = Activator.CreateInstance(dtoType) as DTOBase;

                this.DoConvertBEToDTO(be, dto);
                return dto;
            }
        }
        class DtoToBeConvertor : AbstractBE_DTOConvertor
        {
            public virtual M Convert<M>(DTOBase dto)
            where M : EntityBase, new()
            {
                return (M)DtoToBe(dto, typeof(M));
            }

            private EntityBase DtoToBe(DTOBase dto, Type beType)
            {
                EntityBase be = Activator.CreateInstance(beType) as EntityBase;

                this.DoConvertDTOToBE(dto, be);

                return be;
            }
        }
        class AbstractBE_DTOConvertor
        {
            protected virtual void DoConvertDTOToBE(DTOBase dto, EntityBase entity)
            {
                PropertyInfo[] ps1 = dto.GetType().GetProperties();
                PropertyInfo[] ps2 = entity.GetType().GetProperties();
                Dictionary<string, PropertyInfo> bepdic = new Dictionary<string, PropertyInfo>();
                foreach (PropertyInfo p1 in ps1)
                {
                    bepdic.Add(p1.Name, p1);
                }

                foreach (PropertyInfo p2 in ps2)
                {
                    if (!bepdic.ContainsKey(p2.Name))
                    {
                        continue;
                    }
                    PropertyInfo p1 = bepdic[p2.Name];
                    if (p1.PropertyType == p2.PropertyType)
                    {
                        if (p1.CanWrite)
                        {
                            p2.SetValue(entity, p1.GetValue(dto, null), null);
                        }
                    }
                }

            }

            protected virtual void DoConvertBEToDTO(EntityBase entity, DTOBase dto)
            {
                PropertyInfo[] ps1 = entity.GetType().GetProperties();
                PropertyInfo[] ps2 = dto.GetType().GetProperties();
                Dictionary<string, PropertyInfo> bepdic = new Dictionary<string, PropertyInfo>();
                foreach (PropertyInfo p1 in ps1)
                {
                    bepdic.Add(p1.Name, p1);
                }

                foreach (PropertyInfo p2 in ps2)
                {
                    if (!bepdic.ContainsKey(p2.Name))
                    {
                        continue;
                    }
                    PropertyInfo p1 = bepdic[p2.Name];
                    if (p1.PropertyType == p2.PropertyType)
                    {
                        if (p1.CanWrite)
                        {
                            p2.SetValue(dto, p1.GetValue(entity, null), null);
                        }
                    }
                }

            }
        }
    }
}
