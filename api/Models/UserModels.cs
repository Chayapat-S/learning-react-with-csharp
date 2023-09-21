using System;
namespace api.Models
{
	public class User
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string PassWord { get; set; }
	}

    public class InsertUpdateUserRequest
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}

