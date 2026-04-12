public class Utvekslingsstudent : Student
{
    public Utvekslingsstudent(string brukerId, string navn, string passord, string epost, string hjemuniversitet, string land, DateOnly periodeFra, DateOnly periodeTil)
        : base(brukerId, navn, passord, epost)
    {
        Hjemuniversitet = hjemuniversitet;
        Land = land;
        PeriodeFra = periodeFra;
        PeriodeTil = periodeTil;
    }

    public override string Rolle => "Utvekslingsstudent";
    public string Hjemuniversitet { get; }
    public string Land { get; }
    public DateOnly PeriodeFra { get; }
    public DateOnly PeriodeTil { get; }
}
