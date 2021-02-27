using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Noteify.Data.Models.Auth
{
    public class User : IdentityUser<string>
    {
        public List<Note> Notes { get; set; }
    }
}