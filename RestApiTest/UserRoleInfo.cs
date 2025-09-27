using System.Text.Json;

namespace RestApiTest
{
    public class UserRoleInfo
    {
        public string? ContainerId { get; set; }
        public string? loginName { get; set; }
        public string? role { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
