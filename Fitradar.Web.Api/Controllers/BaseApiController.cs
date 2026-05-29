using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fitradar.Web.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        private IAuthorizationService _authorizationService;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IAuthorizationService AuthorizationService => _authorizationService ??= HttpContext.RequestServices.GetService<IAuthorizationService>();
    }
}
