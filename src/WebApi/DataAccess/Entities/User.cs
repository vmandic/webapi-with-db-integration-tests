using System.ComponentModel.DataAnnotations;

namespace WebApi.DataAccess.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; }
    }
}