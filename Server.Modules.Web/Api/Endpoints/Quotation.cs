using System;
using System.Threading.Tasks;
using Server.ReNote.Utilities;
using Server.Web.Api;
using Server.Web.Api.Responses;
using Server.Web.Utilities;
using Newtonsoft.Json;

namespace Server.ReNote.Api
{
    internal class Quotation
    {
        /// <summary>
        /// Operates a request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        public static async Task<ApiResponse> OperateRequest(ApiRequest req)
        {
            switch (req.Method.ToUpper())
            {
                case "GET":
                    return await Get(req);
                default:
                    return await ApiUtil.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
        }

        /// <summary>
        /// Operates a GET request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Get(ApiRequest req)
        {
            string[] rawQuotations = DatabaseUtil.GetValues(Constants.DB_ROOT_QUOTATIONS);
            if (rawQuotations.Length == 0)
                return await ApiUtil.SendWithDataAsync(200, ApiMessages.Success(), new QuotationResponse()
                {
                    Author = "Sun Tzu, The Art of War",
                    Content = "The opportunity of defeating the enemy\nis provided by the enemy himself."
                });

            int quotationIndex = new Random().Next(0, rawQuotations.Length);

            if (rawQuotations[quotationIndex] == null)
                return await ApiUtil.SendErrorAsync("The quotation index returned a null value");

            QuotationResponse response = JsonConvert.DeserializeObject<QuotationResponse>(rawQuotations[quotationIndex]);
            return await ApiUtil.SendWithDataAsync(200, ApiMessages.Success(), response);
        }
    }
}