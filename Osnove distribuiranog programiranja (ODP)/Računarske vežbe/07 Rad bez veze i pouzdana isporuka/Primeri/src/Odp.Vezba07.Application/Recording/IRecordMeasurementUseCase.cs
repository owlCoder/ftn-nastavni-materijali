namespace Odp.Vezba07.Application.Recording;

public interface IRecordMeasurementUseCase
{
    string Record(string messageId, string payload);
}
