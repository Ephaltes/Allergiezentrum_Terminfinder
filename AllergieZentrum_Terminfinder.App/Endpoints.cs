using Refit;

namespace AllergieZentrum_Terminfinder.App;

public interface IAppointmentsService
{
    [Post("/ACETOwebkalender/rest/reservierung/v1.0/ladeTermineFuerListenAnsicht")]
    [Headers("Content-Type: application/json")]
    Task<AppointmentsResponse> GetAppointments([Body(BodySerializationMethod.Serialized)] AppointmentsRequest request);
}
