using TranslationDB.Models.DTOs;

namespace TranslationDB.Services
{
    public class BaseService
    {
        protected ServiceResult<T> Success<T>(T data, string message = "")
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        protected ServiceResult<T> Error<T>(string message, T data = default(T))
        {
            return new ServiceResult<T>
            {
                Success = false,
                Data = data,
                Message = message
            };
        }
    }
}