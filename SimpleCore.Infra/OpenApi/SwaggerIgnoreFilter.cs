using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleCore.OpenApi
{
    public class SwaggerIgnoreFilter : ISchemaFilter
    {
        #region ISchemaFilter Members

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            
            if (schema?.Properties == null || context.Type == null)
                return;
            var type = context.Type;
            if (typeof(Models.QueryViewModel).IsAssignableFrom(type)) {
                Console.WriteLine("xx");
            }
            var excludedProperties = type.GetProperties()
                                         .Where(t =>
                                                t.GetCustomAttribute<SwaggerIgnoreAttribute>()
                                                != null);
            
            foreach (var excludedProperty in excludedProperties)
            {
                var key = System.Text.RegularExpressions.Regex.Replace(excludedProperty.Name,"^[A-Z]", (match) => match.Value.ToLower());
                if (schema.Properties.ContainsKey(key))
                    schema.Properties.Remove(key);
            }
        }

        #endregion
    }
}
