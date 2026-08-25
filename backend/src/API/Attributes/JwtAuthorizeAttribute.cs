using Microsoft.AspNetCore.Mvc;
using LeksanaStudio.API.Filters;

namespace LeksanaStudio.API.Attributes
{
    public class JwtAuthorizeAttribute : TypeFilterAttribute
    {
        public JwtAuthorizeAttribute()
            : base(typeof(JwtAuthorizationFilter)) { }
    }
}
