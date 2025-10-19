namespace Api1.DTO
{
    public class GeneralResponse
    {
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public string? JwtToken { get; set; }

        //public object? Data { get; set; }

        public List<string>? Errors { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? Expiration { get; set; }



    }
}
