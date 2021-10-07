using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebAppEs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAppEs.Models;

namespace WebAppEs
{
	public static class DbInitializer
	{
		public static void Initialize(IApplicationBuilder app)
		{
			//using (var serviceScope = app.ApplicationServices.CreateScope())
			//{
			//	var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
			//	context.Database.EnsureCreated();

			//	var _userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			//	var _roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			//	if (!context.Users.Any(usr => usr.UserName == "superadmin"))
			//	{
			//		var user = new ApplicationUser()
			//		{
			//			UserName = "superadmin",
			//			Email = "admin@test.com",
			//			EmailConfirmed = true,
			//			Department = "Mobile R&D",
			//			EmployeeID = "45461",
			//			Name = "Estiak Ahmed",
			//		};
			//		var userResult = _userManager.CreateAsync(user, "123456").Result;
			//	}

			//	if (!context.Users.Any(usr => usr.UserName == "admin"))
			//	{
			//		var user = new ApplicationUser()
			//		{
			//			UserName = "admin",
			//			Email = "manager@test.com",
			//			EmailConfirmed = true,
			//			Department = "Mobile R&D",
			//			EmployeeID = "45462",
			//			Name = "Tushar Khan",
			//};

			//		var userResult = _userManager.CreateAsync(user, "123456").Result;
			//	}

			//	if (!context.Users.Any(usr => usr.UserName == "user1"))
			//	{
			//		var user = new ApplicationUser()
			//		{
			//			UserName = "user1",
			//			Email = "employee@test.com",
			//			EmailConfirmed = true,
			//			Department = "Mobile R&D",
			//			EmployeeID = "45463",
			//			Name = "Imtiaz Mahmud",
			//		};

			//		var userResult = _userManager.CreateAsync(user, "123456").Result;
			//	}

			//	if (!_roleManager.RoleExistsAsync("SuperAdmin").Result)
			//	{
			//		var role = _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" }).Result;
			//	}

			//	if (!_roleManager.RoleExistsAsync("Admin").Result)
			//	{
			//		var role = _roleManager.CreateAsync(new IdentityRole { Name = "Admin" }).Result;
			//	}

			//	if (!_roleManager.RoleExistsAsync("User").Result)
			//	{
			//		var role = _roleManager.CreateAsync(new IdentityRole { Name = "User" }).Result;
			//	}

			//	var adminUser = _userManager.FindByNameAsync("superadmin").Result;
			//	var adminRole = _userManager.AddToRolesAsync(adminUser, new string[] { "SuperAdmin" }).Result;

			//	var managerUser = _userManager.FindByNameAsync("admin").Result;
			//	var managerRole = _userManager.AddToRolesAsync(managerUser, new string[] { "Admin" }).Result;

			//	var employeeUser = _userManager.FindByNameAsync("user1").Result;
			//	var userRole = _userManager.AddToRolesAsync(employeeUser, new string[] { "User" }).Result;

			//	var permissions = GetPermissions();
			//	foreach (var item in permissions)
			//	{
			//		if (!context.NavigationMenu.Any(n => n.Name == item.Name))
			//		{
			//			context.NavigationMenu.Add(item);
			//		}
			//	}

			//	var _adminRole = _roleManager.Roles.Where(x => x.Name == "SuperAdmin").FirstOrDefault();
			//	var _managerRole = _roleManager.Roles.Where(x => x.Name == "Admin").FirstOrDefault();
			//	var _employeeRole = _roleManager.Roles.Where(x => x.Name == "User").FirstOrDefault();

			//	if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0")))
			//	{
			//		context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0") });
			//	}

			//	if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c")))
			//	{
			//		context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c") });
			//	}

			//	if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c")))
			//	{
			//		context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c") });
			//	}

			//	//if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("F704BDFD-D3EA-4A6F-9463-DA47ED3657AB")))
			//	//{
			//	//	context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("F704BDFD-D3EA-4A6F-9463-DA47ED3657AB") });
			//	//}

			//	//if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5")))
			//	//{
			//	//	context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5") });
			//	//}

			//	if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734")))
			//	{
			//		context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734") });
			//	}

			//	if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == new Guid("94C22F11-6DD2-4B9C-95F7-9DD4EA1002E6")))
			//	{
			//		context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = new Guid("94C22F11-6DD2-4B9C-95F7-9DD4EA1002E6") });
			//	}

			//	if (!context.RoleMenuPermission.Any(x => x.RoleId == _managerRole.Id && x.NavigationMenuId == new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0")))
			//	{
			//		context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _managerRole.Id, NavigationMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0") });
			//	}

			//	if (!context.RoleMenuPermission.Any(x => x.RoleId == _managerRole.Id && x.NavigationMenuId == new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c")))
			//	{
			//		context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _managerRole.Id, NavigationMenuId = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c") });
			//	}

			//	context.SaveChanges();
			//}
		}

		private static List<NavigationMenu> GetPermissions()
		{
			return new List<NavigationMenu>()
			{
				new NavigationMenu()
				{
					Id = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					Name = "User Manual",
					ControllerName = "",
					ActionName = "",
					ParentMenuId = null,
					DisplayOrder=1,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("283264d6-0e5e-48fe-9d6e-b1599aa0892c"),
					Name = "Roles",
					ControllerName = "Admin",
					ActionName = "Roles",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=1,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("7cd0d373-c57d-4c70-aa8c-22791983fe1c"),
					Name = "Users",
					ControllerName = "Admin",
					ActionName = "Users",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=3,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("F704BDFD-D3EA-4A6F-9463-DA47ED3657AB"),
					Name = "External Google Link",
					ControllerName = "",
					ActionName = "",
					IsExternal = true,
					ExternalUrl = "https://www.google.com/",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=2,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("913BF559-DB46-4072-BD01-F73F3C92E5D5"),
					Name = "Create Role",
					ControllerName = "Admin",
					ActionName = "CreateRole",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=3,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("3C1702C5-C34F-4468-B807-3A1D5545F734"),
					Name = "Edit User",
					ControllerName = "Admin",
					ActionName = "EditUser",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=3,
					Visible = false,
				},
				new NavigationMenu()
				{
					Id = new Guid("94C22F11-6DD2-4B9C-95F7-9DD4EA1002E6"),
					Name = "Edit Role Permission",
					ControllerName = "Admin",
					ActionName = "EditRolePermission",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=3,
					Visible = false,
				},
				new NavigationMenu()
				{
					Id = new Guid("802631f0-eeb8-43b7-965e-2e2bed61a1d1"),
					Name = "Sign Up User",
					ControllerName = "Admin",
					ActionName = "AddUser",
					ParentMenuId = new Guid("13e2f21a-4283-4ff8-bb7a-096e7b89e0f0"),
					DisplayOrder=3,
					Visible = false,
				},

				new NavigationMenu()
				{
					Id = new Guid("ba680c48-f355-4819-9cd0-339ae8f5e1a2"),
					Name = "Add Faults",
					ControllerName = "",
					ActionName = "",
					ParentMenuId = null,
					DisplayOrder=2,
					Visible = true,
				},

				new NavigationMenu()
				{
					Id = new Guid("674e92a8-a617-4cde-a61a-951ed078ab58"),
					Name = "Faults",
					ControllerName = "AddFaultsInfo",
					ActionName = "Index",
					ParentMenuId = new Guid("ba680c48-f355-4819-9cd0-339ae8f5e1a2"),
					DisplayOrder=1,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("41fd2353-6091-46f2-8060-a4cd0824014c"),
					Name = "Add New ",
					ControllerName = "AddFaultsInfo",
					ActionName = "CreateFaultsInfo",
					ParentMenuId = new Guid("ba680c48-f355-4819-9cd0-339ae8f5e1a2"),
					DisplayOrder=1,
					Visible = true,
				},
				new NavigationMenu()
				{
					Id = new Guid("ff10971d-37fa-4a58-be7e-217c025cd89f"),
					Name = "User Setup",
					ControllerName = "",
					ActionName = "",
					ParentMenuId = null,
					DisplayOrder=2,
					Visible = true,
				},

				new NavigationMenu()
				{
					Id = new Guid("44265833-a3a2-4378-97e2-f2190607c05d"),
					Name = "Add Model",
					ControllerName = "ProductModel",
					ActionName = "Index",
					ParentMenuId = new Guid("ff10971d-37fa-4a58-be7e-217c025cd89f"),
					DisplayOrder=1,
					Visible = true,
				},

			};
		}
	}
}