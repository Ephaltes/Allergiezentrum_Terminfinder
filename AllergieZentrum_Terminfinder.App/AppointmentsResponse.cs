// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AppointmentsResponse
    {
        public string responseType { get; set; }
        public Value value { get; set; }
    }

    public class Termin
    {
        public List<int> auftragWebIds { get; set; }
        public int beginnMinuten { get; set; }
        public string beginnZeitpunktFormatiert { get; set; }
        public int endeMinuten { get; set; }
        public List<object> kalenderArztWebIds { get; set; }
        public int kalenderdatenId { get; set; }
        public string styleClasses { get; set; }
        public DateOnly terminDatum { get; set; }
        public int terminId { get; set; }
        public string titel { get; set; }
    }

    public class Termine
    {
        public string beginnDatumTextKurz { get; set; }
        public string beginnDatumTextLang { get; set; }
        public string beginnUhrzeitText { get; set; }
        public Termin termin { get; set; }
        public string wochentag { get; set; }
    }

    public class Value
    {
        public int nochReservierbareTermineFuerIp { get; set; }
        public List<Termine> termine { get; set; }
        public string wartungsnachricht { get; set; }
    }

