using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Firebase_Auth.Data.Constant;

namespace Firebase_Auth.Data
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public EfState State { get; set; } = EfState.Active;
    }
}