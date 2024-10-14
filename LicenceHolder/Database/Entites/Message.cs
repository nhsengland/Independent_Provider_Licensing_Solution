using Database.Entites.Core;
using Domain.Objects.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entites
{
    [Table(nameof(Message))]
    public class Message : BaseEntity
    {
        public int OrganisationId { get; set; }

        [Required]
        public Organisation Organisation { get; set; } = default!;

        [Required, MinLength(1)]
        public string Title { get; set; } = default!;

        [Required, MinLength(1)]
        public string Body { get; set; } = default!;

        [Required, MinLength(1)]
        public string From { get; set; } = default!;

        public DateTime SendDateTime { get; set; }

        public bool IsRead { get; set; }

        public int MessageTypeId { get; set; }
        
        [Required, ForeignKey(nameof(MessageTypeId))]
        public MessageType MessageType { get; set; } = default!;
    }
}
