// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class AppointmentsRequest
{
    public AppointmentsRequest(int arztId, int kalenderWebId, int ausblendenStundenVorTermin, int anzeigenMonateVorTermin)
    {
        this.arztId = arztId;
        this.kalenderWebId = kalenderWebId;
        this.ausblendenStundenVorTermin = ausblendenStundenVorTermin;
        this.anzeigenMonateVorTermin = anzeigenMonateVorTermin;
    }
    public int arztId { get; set; }
    public int kalenderWebId { get; set; }
    public int ausblendenStundenVorTermin { get; set; }
    public int anzeigenMonateVorTermin { get; set; }
}