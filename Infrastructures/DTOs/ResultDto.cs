namespace Infrastructures.DTOs
{
    public class ResultDto<T>
    {
        public bool Success { get; set; }
        public required string Message { get; set; }
        public required T Data { get; set; }
    }
}