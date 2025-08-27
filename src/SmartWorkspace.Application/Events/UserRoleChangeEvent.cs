using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Events
{
    public class UserRoleChangeEvent : INotification
    {
        public Guid UserId { get; }
        public UserRoleChangeEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
