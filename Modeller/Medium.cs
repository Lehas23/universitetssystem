public abstract class Medium : ILanbar
{
    public string Id { get; }
    public string Tittel { get; }
    public int Aar { get; }
    public int AntallEksemplarer { get; private set; }
    
    protected Medium(string id, string tittel, int aar, int antallEksemplarer)
    {
        Id = id;
        Tittel = tittel;
        Aar = aar;
        AntallEksemplarer = antallEksemplarer;
    }

    public bool LaanUt()
    {
        if (AntallEksemplarer < 1) return false;
        AntallEksemplarer--;
        return true;
    }

    public void LeverInn() => AntallEksemplarer++;
}