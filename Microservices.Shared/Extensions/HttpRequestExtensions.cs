using Microservices.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Shared.Extensions
{
    public static class HttpRequestExtensions
    {
        public async static Task<T> GetBodyAsync<T>(this HttpRequest request)
        {
            var bodyString = await request.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<T>(bodyString);

            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(value, new ValidationContext(value, null, null), results, true);
            var validationResults = results;

            if (!isValid) throw new AppException(400, $"Model is invalid: {string.Join(", ", validationResults.Select(s => s.ErrorMessage).ToArray())}");

            return value;
        }

        public static T GetQueryParams<T>(this HttpRequest request)
        {
            var query = request.Query;
            var dict = new Dictionary<string, string>();
            foreach (var key in query.Keys)
            {
                StringValues val = new StringValues();
                query.TryGetValue(key, out val);
                dict.Add(key, val.ToString());
            }

            var queryJson = JsonConvert.SerializeObject(dict);
            var value = JsonConvert.DeserializeObject<T>(queryJson);

            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(value, new ValidationContext(value, null, null), results, true);
            var validationResults = results;

            if (!isValid) throw new AppException(400, $"Model is invalid: {string.Join(", ", validationResults.Select(s => s.ErrorMessage).ToArray())}");

            return value;
        }

        public static string GetPathParam(this HttpRequest request, string paramKey)
        {
            string paramValue = request.GetPathParam(paramKey);
            if (paramValue == null)
                throw new AppException(400, $"Path parameter {paramKey} not found.");

            return paramValue;
        }
    }
}
