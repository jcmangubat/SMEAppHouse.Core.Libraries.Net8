using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Abstractions;

namespace SMEAppHouse.Core.Patterns.WebApi.Models
{
    public class User : IntKeyedEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; set; }
    }
}