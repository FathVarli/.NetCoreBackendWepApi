using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Concrete
{
    [Table("User")]
    public class User:IEntity
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("FirsName")]
        public string FirstName { get; set; }
        [Column("LastName")]
        public string LastName { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("PasswordSalt")]
        public byte[] PasswordSalt { get; set; }
        [Column("PasswordHash")]
        public byte[] PasswordHash { get; set; }
    }
}
