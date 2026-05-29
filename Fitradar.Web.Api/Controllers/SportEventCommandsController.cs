using Fitradar.Application.UseCases.Reactions.Commands;
using Fitradar.Web.Api.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fitradar.Web.Api.Controllers
{
    [Authorize(Policy = FitradarPolicies.OnlyVerifiedByFitRadar)]
    public class SportEventCommandsController : BaseApiController
    {
        [Authorize(Policy = FitradarPolicies.OnlyEnabledUsers)]
        [HttpPost("events/{id}/comments")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddComment([FromRoute] Guid id, [FromBody] AddComment command)
        {
            command.SportEventInstancePublicId = id;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
