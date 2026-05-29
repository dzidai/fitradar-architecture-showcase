using Fitradar.Application.Behaviors;
using Fitradar.Application.Common;
using Fitradar.Application.Persistence.ReadStores;
using Fitradar.Application.Persistence.ReadStores.Models;
using Fitradar.Application.Services;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Fitradar.Application.UseCases.Reactions.Commands;

public class AddComment : ISportEventInteractionCommand, IReadStoreCommand
{
    [JsonIgnore]
    public Guid SportEventInstancePublicId { get; set; }

    [JsonPropertyName("comment")]
    public string CommentText { get; init; }


    public class Handler : IReadStoreCommandHandler<AddComment>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ICommentReadStore _commentReadStore;

        public Handler(
            ICurrentUserContext currentUserContext,
            ICommentReadStore commentReadStore)
        {
            _currentUserContext = currentUserContext;
            _commentReadStore = commentReadStore;
        }

        public async Task Handle(AddComment request, CancellationToken cancellationToken)
        {
            var commentProjection = new CommentReadModel
            {
                CommentText = request.CommentText,
                PostedById = _currentUserContext.UserId,
                PostedAt = DateTime.UtcNow,
                SportEventInstancePublicId = request.SportEventInstancePublicId
            };

            await _commentReadStore.CreateAsync(commentProjection, cancellationToken);
        }
    }
}
