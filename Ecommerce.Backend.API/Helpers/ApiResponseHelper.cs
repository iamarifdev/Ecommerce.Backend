using System;
using System.Collections.Generic;
using System.Linq;
using Ecommerce.Backend.Common.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ecommerce.Backend.API.Helpers
{
  public static class ApiResponseHelper
  {
    public static ApiResponse<bool> CreateSuccessResponse(this bool result, string message = null)
    {
      return new ApiResponse<bool> { Message = message, Result = result, Success = true };
    }
    public static ApiResponse<TResult> CreateSuccessResponse<TResult>(this TResult result, string message = null) where TResult : class
    {
      return new ApiResponse<TResult> { Message = message, Result = result, Success = true };
    }

    public static ApiResponse<TResult> CreateErrorResponse<TResult>(this TResult result) where TResult : Exception
    {
      return new ApiResponse<TResult> { Message = $"{result.Message}", Result = null, Success = false };
    }

    public static ApiResponse<Dictionary<string, IEnumerable<string>>> CreateErrorResponse(this ModelStateDictionary result)
    {
      var errorMessagesMap = new Dictionary<string, IEnumerable<string>>();
      result.Keys.ToList().ForEach((key) =>
      {
        errorMessagesMap.Add(key, result[key].Errors.Select(e => e.ErrorMessage));
      });

      return new ApiResponse<Dictionary<string, IEnumerable<string>>>
      {
        Message = "Request payload validation failed.",
        Result = errorMessagesMap,
        Success = false
      };
    }
  }
}