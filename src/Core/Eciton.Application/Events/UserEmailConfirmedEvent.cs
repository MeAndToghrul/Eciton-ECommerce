using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.Events
{
    public class UserEmailConfirmedEvent 
    {
        public string UserId { get; set; }

        public UserEmailConfirmedEvent(string userId)
        {
            UserId = userId;
        }
    }
}
