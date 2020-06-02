
namespace Microservices.Shared
{
    using Newtonsoft.Json;
    public abstract class Entity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
