using MediatR;
using SmartWorkspace.Application.Events;
using SmartWorkspace.Domain.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Application.Users
{
    public class AssignRoleCommandHandler : IRequest<Result<AssignRoleCommand>>
    {
        private readonly IMediator _mediator;

        public AssignRoleCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<AssignRoleCommand>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
        {
            // Fire event
            await _mediator.Publish(new UserRoleChangeEvent(Guid.Parse(request.UserId)), cancellationToken);

            return Result<AssignRoleCommand>.Success(new AssignRoleCommand());
        }
    }
}
