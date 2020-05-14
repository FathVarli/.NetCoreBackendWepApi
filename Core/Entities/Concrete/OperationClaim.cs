using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Concrete
{
    [Table("OperationClaim")]
    public class OperationClaim:IEntity
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
    }
}
