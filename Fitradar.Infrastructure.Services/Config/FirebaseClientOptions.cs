using Google.Apis.Auth.OAuth2;

namespace Fitradar.Application.Contracts.Integration.Services.Config
{
    public class FirebaseClientOptions : JsonCredentialParameters
    {
        public string WebApiKey { get; set; }

        public bool OnlyValidate { get; set; }

        public string StorageBucketName { get; set; }
    }
}
