using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Concrete
{
    [Table("UserOperationClaim")]
    public class UserOperationClaim:IEntity
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("UserId")]
        public int UserId { get; set; }
        [Column("OperationClaimId")]
        public int OperationClaimId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("OperationClaimId")]
        public OperationClaim OperationClaim { get; set; }
    }
}
