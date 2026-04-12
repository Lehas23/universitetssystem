public abstract class Bruker
{
    public string Navn { get; set; }
    public string Epost { get; set; }
    public string Passord { get; private set; }
    public abstract string Rolle { get; }
    public string BrukerId { get; }

    public Bruker(string brukerId, string navn, string passord, string epost)
    {
        BrukerId = brukerId;
        Navn = navn;
        Passord = passord;
        Epost = epost;
    }
}
