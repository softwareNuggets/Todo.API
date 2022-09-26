namespace Todo.API.Services
{
    public interface IStatusCodeRepository
    {
        Task<IEnumerable<StatusCode>> GetAllStatusCodesAsync();
        Task<StatusCode?> GetStatusCodeAsync(string statusCode);

        Task<StatusCode?> InsertStatusCodeAsync(StatusCode statusCode);
        
        StatusCode? UpdateStatusCode(StatusCode statusCode);
        StatusCode? RemoveStatusCode(StatusCode statusCode);
    }
}
