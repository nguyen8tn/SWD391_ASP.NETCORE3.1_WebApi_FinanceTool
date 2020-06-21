using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SWD391.Models
{
    public class QueryParams
    {
        public string Term { get; set; }
        public DateTime MinDate { get; set; }
        public bool IncludeInactive { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public string OrderBy { get; set; }
        public string SearchBy { get; set; }
    }
    public class TestQuery
    {
        [JsonProperty("@odata.select")]
        public string Select { get; set; }
    }
    public class PagedCollectionResponse<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
    }

    public class CustomSwaggerAttribute : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            MethodInfo methodInfo;
            var ss = context.ApiDescription.TryGetMethodInfo(out methodInfo);
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();
            //var t = context.ApiDescription.TryGetMethodInfo();
            //operation.Parameters.Add(new OpenApiParameter
            //{
            //    Name = "x-customHeader",
            //    In = ParameterLocation.Header,
            //    Required = true,
            //    Schema = new OpenApiSchema
            //    {
            //        Type = "String"
            //    }
            //});
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$filter",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                }
            });
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$select",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                }
            });
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$skip",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                }
            });
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$top",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                }
            });
        }
    }
    public class MyClass
    {
        private IModelMetadataProvider provider;

        MyClass(IModelMetadataProvider provider)
        {
            this.provider = provider;
        }

        public void OnProvidersExecuting(ApiDescriptionProviderContext context)
        {
            foreach (var result in context.Results)
            {
                HandleODataReponse(result);
            }
        }

        private void HandleODataReponse(ApiDescription result)
        {
            var odataParam = result.ParameterDescriptions.FirstOrDefault(p => p.ParameterDescriptor.ParameterType.IsGenericType && p.ParameterDescriptor.ParameterType.GetGenericTypeDefinition() == typeof(ODataQueryOptions<>));
            if (odataParam != null)
            {
                result.ParameterDescriptions.Remove(odataParam);
                result.ParameterDescriptions.Add(new ApiParameterDescription
                {
                    Name = "$filter",
                    ModelMetadata = provider.GetMetadataForType(typeof(string)),
                    Source = BindingSource.Query
                });
                result.ParameterDescriptions.Add(new ApiParameterDescription
                {
                    Name = "$orderby",
                    ModelMetadata = provider.GetMetadataForType(typeof(string)),
                    Source = BindingSource.Query
                });
                result.ParameterDescriptions.Add(new ApiParameterDescription
                {
                    Name = "$top",
                    ModelMetadata = provider.GetMetadataForType(typeof(int)),
                    Source = BindingSource.Query
                });
                result.ParameterDescriptions.Add(new ApiParameterDescription
                {
                    Name = "$skip",
                    ModelMetadata = provider.GetMetadataForType(typeof(int)),
                    Source = BindingSource.Query
                });
                result.ParameterDescriptions.Add(new ApiParameterDescription
                {
                    Name = "$count",
                    ModelMetadata = provider.GetMetadataForType(typeof(bool)),
                    Source = BindingSource.Query
                });
            }
        }
    }

    //public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    //{
    //    readonly IApiVersionDescriptionProvider provider;
    //    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
    //        this.provider = provider;
    //    public void Configure(SwaggerGenOptions options)
    //    {
    //        ODataQueryOptions queryOptions
    //        foreach (var description in provider.ApiVersionDescriptions)
    //        {
    //            options.SwaggerGeneratorOptions.SwaggerDocs(description.GroupName)
    //        }
    //        throw new NotImplementedException();
    //    }
    //}

}
