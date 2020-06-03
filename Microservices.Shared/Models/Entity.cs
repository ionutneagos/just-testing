
namespace Microservices.Shared
{
    using Newtonsoft.Json;
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class Entity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
