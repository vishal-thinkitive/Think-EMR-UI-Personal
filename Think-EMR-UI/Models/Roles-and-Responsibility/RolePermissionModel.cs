using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkEMR_Care.DataAccess.Models.Roles_and_Responsibility
{
    public class RolePermissionModel
    {
        public string RoleName { get; set; }
        public List<int> PermissionIds  { get; set; }
    }
}
