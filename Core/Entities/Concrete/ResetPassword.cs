using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities.Concrete
{
    [Table("ResetPassword")]
    public class ResetPassword:IEntity
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("UserId")]
        public int UserId { get; set; }
        [Column("Token")]
        public string Token { get; set; }
        [Column("ExpirationTime")]
        public DateTime ExpirationTime { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
