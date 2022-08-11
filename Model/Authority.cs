using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDB.Model
{
    public class Authority
    {
        [Column("authorityId")]
        public int Id { set; get; }

        [Column("authorityStatus"), Precision(3)]
        public int Status { set; get; }

        [Column("authorityTitle"), MaxLength(255)]
        public string? Title { set; get; }

        //public List<Parts>? Part { get; set; }
    }
}
