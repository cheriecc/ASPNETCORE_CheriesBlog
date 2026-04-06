using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheriesBlog.Domain.Models
{
    [Table("message_box")]
    public class Message
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(250)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("email")]
        [MaxLength(250)]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Column("message")]
        [MaxLength(250)]
        [Required]
        public string MessageText { get; set; } = string.Empty;
    }
}
