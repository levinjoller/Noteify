using System;
using Noteify.Data.EntityFramework.Interfaces;
using Noteify.Data.Models.Auth;

namespace Noteify.Data.Models
{
    public class Note : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign key
        public string UserId { get; set; }
        public User User { get; set; }
    }
}