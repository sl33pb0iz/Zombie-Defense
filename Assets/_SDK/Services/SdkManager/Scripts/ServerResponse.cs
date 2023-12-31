using Newtonsoft.Json;

namespace RocketTeam.Sdk.Services.Manager
{
    public class ServerResponse<T>
    {
        [JsonProperty("code")]
        public int code;

        [JsonProperty("data")]
        public T data;

        public bool IsSuccess()
        {
            return code == SdkReturnCode.SUCCESS;
        }
    }
}