using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Sys.Service;
using TelChina.AF.Persistant;
using System.ServiceModel;
using TelChina.AF.Sys.DTO;

namespace TelChina.AF.Resource
{
    public partial class EditResourceService : ServiceBase, IEditResourceService
    {
        public virtual IList<UserResourceDTO> GetTypeResource()
        {
            IList<UserResourceDTO> dtoList = new List<UserResourceDTO>();

            UserResource userResource = new UserResource();

            var userList = userResource.GetTypeResource();

            SystemResource systemResource = new SystemResource();
            var sysList = systemResource.GetTypeResource();

            //是否存在字典
            Dictionary<string, string> list = new Dictionary<string, string>();

            foreach (UserResource user in userList)
            {
                if (!list.ContainsKey(user.ResourceType))
                {
                    list.Add(user.ResourceType, user.ResourceName);
                    UserResourceDTO userDTo = DTOConvert.ToDTO<UserResourceDTO>(user);
                    userDTo.State = ResourceState.Unchanged;
                    dtoList.Add(userDTo);
                }
            }

            foreach (SystemResource sys in sysList)
            {
                if (!list.ContainsKey(sys.ResourceType))
                {
                    list.Add(sys.ResourceType, sys.ResourceName);

                    UserResourceDTO userDTo = DTOConvert.ToDTO<UserResourceDTO>(sys);
                    userDTo.State = ResourceState.Unchanged;
                    dtoList.Add(userDTo);
                }
            }        
            return dtoList;
        }

        public virtual IList<UserResourceDTO> GetResourceByType(string resourceType)
        {
            IList<UserResourceDTO> dtoList = new List<UserResourceDTO>();

            UserResource userResource = new UserResource();

            var userList = userResource.GetResourceByType(resourceType);

            SystemResource systemResource = new SystemResource();
            var sysdtoList = systemResource.GetResourceByType(resourceType);
            //是否存在字典
            Dictionary<string, string> list = new Dictionary<string, string>();
           
            foreach (UserResource user in userList)
            {
                if (!list.ContainsKey(user.ResourceCode.Trim()))
                {
                    list.Add(user.ResourceCode.Trim(), user.ResourceName);
                    UserResourceDTO userDTo = DTOConvert.ToDTO<UserResourceDTO>(user);
                    userDTo.State = ResourceState.Unchanged;
                    dtoList.Add(userDTo);
                }
            }

            foreach (SystemResource system in sysdtoList)
            {
                if (!list.ContainsKey(system.ResourceCode.Trim()))
                {
                    list.Add(system.ResourceCode.Trim(), system.ResourceName);
                    UserResourceDTO userdto = new UserResourceDTO();
                    userdto.ID = Guid.NewGuid();
                    userdto.State = ResourceState.Inserting;
                    userdto.ResourceCode = system.ResourceCode;
                    userdto.ResourceName = system.ResourceName;
                    userdto.ResourceType = system.ResourceType;
                    dtoList.Add(userdto);
                }
            }

            return dtoList;
        }

        public virtual void UpdateResource(IList<UserResourceDTO> userResources)
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                int saveCount = 0;
                foreach (UserResourceDTO dto in userResources)
                {
                    if (dto.State == ResourceState.Updating)
                    {
                        UserResource userResource = DTOConvert.ToBE<UserResource>(dto);
                        repo.Update(userResource);
                        saveCount++;
                    }
                    if (dto.State == ResourceState.Inserted)
                    {
                        UserResource userResource = DTOConvert.ToBE<UserResource>(dto);
                        repo.Add(userResource);
                        saveCount++;
                    }
                }
                if (saveCount > 0)
                {
                    repo.SaveChanges();
                }
            }
        }

        private void UpdateResource_Extend(IList<UserResourceDTO> userResources)
        {
            using (var repo = RepositoryContext.GetRepository())
            {
                foreach (UserResourceDTO dto in userResources)
                {
                    if (dto.State == ResourceState.Updating)
                    {
                        UserResource userResource = DTOConvert.ToBE<UserResource>(dto);
                        repo.Update(userResource);
                    }
                    if (dto.State == ResourceState.Inserted)
                    {
                        UserResource userResource = DTOConvert.ToBE<UserResource>(dto);
                        repo.Add(userResource);
                    }
                }
                repo.SaveChanges();
            }
        }
    }
}
