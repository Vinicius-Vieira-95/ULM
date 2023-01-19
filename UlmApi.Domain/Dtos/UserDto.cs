namespace UlmApi.Domain.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string Role { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public UserDto(Keycloak.Net.Models.Users.User user, string role) 
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Active = (bool) user.Enabled;
            Role = role;
        }

    }
}
