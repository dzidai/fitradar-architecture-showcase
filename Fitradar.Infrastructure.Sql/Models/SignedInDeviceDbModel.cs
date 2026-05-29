using System.ComponentModel.DataAnnotations.Schema;

namespace Fitradar.Application.Contracts.Persistence.Models
{
    [Table("SignedInDevices")]
    public class SignedInDeviceDbModel
    {
        public string UserId { get; set; }

        public string DeviceId { get; set; }

        public string FcmToken { get; set; }
    }
}
