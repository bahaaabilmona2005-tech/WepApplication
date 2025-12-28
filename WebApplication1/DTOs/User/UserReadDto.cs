
namespace WebApplication1.DTOs.User
{
    public class UserReadDto
    {
        internal DateTime? CreatedAt;
        internal DateTime? UpdatedAt;

        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
       
        public string createdAt { get;}

        public string updatedAt { get;}
    }
}
