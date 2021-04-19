using KPDNHEP.Console.Services.APIs;
using KPDNHEP.Console.Services.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KPDNHEP.Console.Services.Services
{
    public class UserGroupService
    {
        ConfigurationService configurationService = new ConfigurationService();
        API api = new API();
        string baseUrl;
        public UserGroupService()
        {
            baseUrl=configurationService.GetService();
        }
        public List<UserGroup> GetUserGroups(string userName)
        {
            return api.GetAll<UserGroup>(String.Format("{0}/UserGroup/GetGroupNamesByUser/{1}", baseUrl, userName));
        }
        public UserGroupApplication UpdateApplicationGroups(UserGroupApplication userGroupApplication)
        {
            return api.Post(String.Format("{0}/UserGroupApplication/AddApplicationToGroup",baseUrl), userGroupApplication);
        }
        public UserGroup CreateGroup(UserGroup userGroup)
        {
            return api.Post(String.Format("{0}/UserGroup/CreateGroup", configurationService.baseUrl), userGroup);
        }
        public UserGroup DeleteGroup(UserGroup userGroup)
        {
            return api.Post(String.Format("{0}/UserGroup/DeleteGroup", configurationService.baseUrl), userGroup);
        }
    }
}
