using System;
using System.Threading.Tasks;
using Server.ReNote.Helpers;
using Server.Web.Api;
using Server.Web.Api.Responses;
using Server.Web.Helpers;
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
            switch (req.Method)
            {
                case "GET":
                    return await Get(req);
                default:
                    return await ApiHelper.SendAsync(405, ApiMessages.MethodNotAllowed());
            }
        }

        /// <summary>
        /// Operates a GET request.
        /// </summary>
        /// <param name="req">The <see cref="ApiRequest"/> to be proceeded.</param>
        /// <returns><see cref="ApiResponse"/></returns>
        private static async Task<ApiResponse> Get(ApiRequest req)
        {
            string[] rawQuotations = DatabaseHelper.GetValues(Constants.DB_ROOT_QUOTATIONS);
            if (rawQuotations.Length == 0)
                return await ApiHelper.SendAsync(200, ApiMessages.Success(), new QuotationResponse()
                {
                    Author = "Sun Tzu, The Art of War",
                    Content = "The opportunity of defeating the enemy\nis provided by the enemy himself."
                });

            int quotationIndex = new Random().Next(0, rawQuotations.Length);

            if (rawQuotations[quotationIndex] == null)
                return await ApiHelper.SendErrorAsync("The quotation index returned a null value");

            QuotationResponse response = JsonConvert.DeserializeObject<QuotationResponse>(rawQuotations[quotationIndex]);
            return await ApiHelper.SendAsync(200, ApiMessages.Success(), response);
        }
    }
}