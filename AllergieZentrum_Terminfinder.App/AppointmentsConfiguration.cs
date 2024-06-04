namespace AllergieZentrum_Terminfinder.App;

public class AppointmentsConfiguration
{
    public IReadOnlyCollection<int> AuftragWebIds
    {
        get;
        set;
    } = new List<int>();

    public DateOnly DateBefore
    {
        get;
        set;
    } = DateOnly.MaxValue;

    public DateOnly DateAfter
    {
        get;
        set;
    } = DateOnly.MinValue;

    public int CheckIntervalInSeconds
    {
        get;
        set;
    } = 900;
}