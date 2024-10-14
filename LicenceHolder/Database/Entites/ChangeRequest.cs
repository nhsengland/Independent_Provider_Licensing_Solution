using Database.Entites.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entites
{
    [Table(nameof(ChangeRequest))]
    public class ChangeRequest : BaseEntity
    {
        public int CompanyId { get; set; }

        [Required]
        public Company Company { get; set; } = default!;

        public int TypeId { get; set; }

        [Required]
        public ChangeRequestType Type { get; set; } = default!;

        public int StatusId { get; set; }

        [Required]
        public ChangeRequestStatus Status { get; set; } = default!;

        public DateTime DateCreated { get; set; }

        public DateTime DateLastUpdated { get; set; }

        public DateOnly? FinancialYearEnd { get; set; }

        public string? Address { get; set; }

        public string? Name { get; set; }

        public int RaisedById { get; set; }

        [Required, ForeignKey(nameof(RaisedById))]
        public User RaisedBy { get; set; } = default!;
    }
}
