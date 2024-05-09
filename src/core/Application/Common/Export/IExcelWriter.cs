using Shared.ServiceContracts;

namespace Application.Common.Export;

public interface IExcelWriter : ITransientService
{
    Stream WriteToStream<T>(IList<T> data);
}