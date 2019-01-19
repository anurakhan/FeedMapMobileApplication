using System;
using Microsoft.AspNetCore.Mvc;

namespace FeedMapWebApiApp
{
    public class ClientKeyAuthAttribute : TypeFilterAttribute
    {
        public ClientKeyAuthAttribute() : base(typeof(ClientKeyAuthorizeFilter))
        {
        }
    }
}
