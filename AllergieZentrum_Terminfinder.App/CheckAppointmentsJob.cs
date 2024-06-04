using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Quartz;

namespace AllergieZentrum_Terminfinder.App;

public class CheckAppointmentsJob : IJob
{
    private readonly AppointmentsConfiguration _appointmentsConfiguration;
    private readonly ILogger<CheckAppointmentsJob> _logger;
    private readonly IAppointmentsService _service;

    public CheckAppointmentsJob(IAppointmentsService service, IOptions<AppointmentsConfiguration> appointmentsConfigurations, IOptions<AppointmentsConfiguration> appointmentsConfiguration, ILogger<CheckAppointmentsJob> logger)
    {
        _service = service;
        _logger = logger;
        _appointmentsConfiguration = appointmentsConfiguration.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Checking Appointments");
        
        AppointmentsRequest request = new(68, 1376, 8, 12);

        AppointmentsResponse response = await _service.GetAppointments(request);

        List<Termine> checkupAppointments = response.value.termine
                                                    .Where(appointment =>
                                                               appointment.termin.auftragWebIds.Any(id => _appointmentsConfiguration.AuftragWebIds.Contains(id)) &&
                                                               appointment.termin.terminDatum >= _appointmentsConfiguration.DateAfter &&
                                                               appointment.termin.terminDatum <= _appointmentsConfiguration.DateBefore)
                                                    .ToList();

        if (checkupAppointments.Count == 0)
            return;

        checkupAppointments.ForEach(appointment =>
                                        _logger.LogWarning(
                                            $"Termin: {appointment.termin.terminDatum}" +
                                            $"    Uhrzeit: {appointment.beginnUhrzeitText}" +
                                            $"    AuftragWebId: {string.Join(", ", appointment.termin.auftragWebIds)}"));

        Console.Beep();
    }
}