using System;
namespace FeedMapApp.Models
{
    public class RestReturnObj<T>
    {
        public bool IsSuccess { get; set; }
        public T Obj { get; set; }
    }
}
