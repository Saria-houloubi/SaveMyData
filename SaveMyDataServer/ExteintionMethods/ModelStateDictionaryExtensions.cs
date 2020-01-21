using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace SaveMyDataServer.ExteintionMethods
{
    /// <summary>
    /// Extension methods for the ModelStateDictionary
    /// </summary>
    public static class ModelStateDictionaryExtensions
    {

        /// <summary>
        /// Gets the erros messages from the model state
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<string> GetValidationErrors(this ModelStateDictionary model)
        {
            return model.Values.SelectMany(item => item.Errors).Select(e => e.ErrorMessage).ToList();
        }
    }
}
