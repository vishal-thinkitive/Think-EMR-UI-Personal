using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Role_And_Permission.Roles_and_Responsibility;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Think_EMR_UI.Models.Roles_and_Responsibility;
using ThinkEMR_Care.DataAccess.Models.Roles_and_Responsibility;

namespace Think_EMR_UI.Controllers
{
    public class Roles_and_ResponsibilityController : Controller
    {
        string baseURL = "https://localhost:7286/api/";
        [HttpGet]
        public async Task<IActionResult> Roles_and_Permissions()
        {
            List<string> roles = new List<string>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("Roles_and_Responsibility/allPermissions").Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var roleTypes = JsonConvert.DeserializeObject<List<RoleType>>(result);
                    if (roleTypes != null)
                    {
                        foreach (var roleType in roleTypes)
                        {
                            roles.Add(roleType.RoleTypeName);
                        }
                        List<SelectListItem> roleItems = roles.Select(option => new SelectListItem { Value = option, Text = option }).ToList();

                        ViewData["roleTypes"] = roleTypes;
                        ViewData["roleItems"] = roleItems;

                    }
                }
                else
                {
                    Console.WriteLine("error while calling web api");
                }
            }
            return View();
        }




        [HttpGet("rolePermissions")]
        public async Task<ActionResult<IEnumerable<string>>> GetPermissionWithRoleType(string roleTypeName)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"Roles_and_Responsibility/rolePermissions?roleTypeName={roleTypeName}").Result;
                List<string> permissions = new List<string>();
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    permissions = JsonConvert.DeserializeObject<List<string>>(result);
                }
                return permissions;

            }

        }

        [HttpPost("addNewRole")]
        public async Task<IActionResult> AddNewRole(string roleTypeName, string roleName)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var postData = new RoleDataModel
                {
                    RoleTypeName = roleTypeName,
                    RoleName = roleName

                };
                var json = JsonConvert.SerializeObject(postData);

                HttpResponseMessage response = await client.PostAsJsonAsync<RoleDataModel>($"Roles_and_Responsibility/addNewRole", postData);
                if (response.IsSuccessStatusCode)
                {
                    return Ok("Role added successfully");

                }
                return NotFound();
            }
        }

        [HttpPost("addRoleWithPermission")]
        public async Task<IActionResult> AddRoleWithPermission(string roleName, string permissionIds)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                List<int> intList = JsonConvert.DeserializeObject<List<int>>(permissionIds);

                var postData = new RolePermissionModel
                {
                    RoleName = roleName,
                    PermissionIds = intList

                };
                var json = JsonConvert.SerializeObject(postData);

                HttpResponseMessage response = await client.PostAsJsonAsync<RolePermissionModel>($"Roles_and_Responsibility/addRoleWithPermission", postData);
                if (response.IsSuccessStatusCode)
                {
                    return Ok("Role Permissions added successfully");

                }
                return NotFound();
            }
        }
    }
}
