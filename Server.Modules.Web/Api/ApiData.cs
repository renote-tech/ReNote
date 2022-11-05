using System.Reflection;

using Server.Common.Exceptions;
using Server.Common.Utilities;
using Server.Web.Utilities;

namespace Server.Web.Api
{
    internal class ApiData
    {
        public static async Task<ApiResponse> CallEndpointAsync(ApiEndpoint apiEndpoint, ApiRequest apiRequest)
        {
            Type endpointClass = Type.GetType($"Server.ReNote.Api.{apiEndpoint.Name}");

            if (endpointClass == null)
                return ApiUtil.SendError("Endpoint not found");

            MethodInfo endpointMethod = endpointClass.GetMethod("OperateRequest", BindingFlags.Public | BindingFlags.Static);

            if (endpointMethod == null)
                return ApiUtil.SendError("Endpoint call not found");

            Task<ApiResponse> response = (Task<ApiResponse>)endpointMethod.Invoke(null, new object[] { apiRequest });
            return await response;
        }
    }
}