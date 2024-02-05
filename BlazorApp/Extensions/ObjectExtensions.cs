
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BlazorApp.Extensions;

public static class ObjectExtensions
{
    public static T DeepCopy<T>(this T self)
    {
          var serialized = JsonConvert.SerializeObject(self); 
          return JsonConvert.DeserializeObject<T>(serialized) 
            ?? throw new ArgumentNullException(nameof(self));
    }
}