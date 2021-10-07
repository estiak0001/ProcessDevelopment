namespace WebAppEs.Models
{
	public class UserViewModel
	{
		public string Id { get; set; }

		public string EmployeeID { get; set; }
		public string Name { get; set; }
		public string UserName { get; set; }

		public string Email { get; set; }

		public string Department { get; set; }

		public RoleViewModel[] Roles { get; set; }
	}
}