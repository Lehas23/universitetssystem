public class Ansatt : Bruker
{
    public Ansatt(string brukerId, string navn, string passord, string epost, Stilling stilling, string avdeling)
        : base(brukerId, navn, passord, epost)
    {
        Stilling = stilling;
        Avdeling = avdeling;
    }
    public Stilling Stilling { get; }
    public string Avdeling { get; }
    public override string Rolle => "Ansatt";
}