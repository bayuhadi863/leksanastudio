namespace LeksanaStudio.Common.Models
{
    public class BaseRequest<T>
    {
        public T Data { get; set; } = default!;
    }
}
