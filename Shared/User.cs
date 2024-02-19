using System.Runtime;

namespace BlazorEcommerceApp.Shared
{
    public  class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
