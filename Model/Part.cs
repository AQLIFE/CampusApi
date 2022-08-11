using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDB.Model
{
    [Table("Parts")]
    public class Parts
    {
        [Key,Column("uuId")]
        public int Id { set; get; }

        [Column("uuName"), MaxLength(255)]
        public string? Name { set; get; }

        //public Authority? Authority { set; get; }

        [Column("uuStatus"), Precision(3)]
        public int Status { set; get; }

        [Column("uuPass"), MaxLength(255)]
        public string? Pass { set; get; }
    }
}
