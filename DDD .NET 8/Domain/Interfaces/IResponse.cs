using Domain.Helpers;

namespace Domain.Interfaces
{
    public interface IResponse<T>
    {
        T Content { get; }
        IEnumerable<Error> Errors { get; }
        bool HasError { get; }
    }
}
