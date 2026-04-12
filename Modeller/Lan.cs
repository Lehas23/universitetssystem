public class Lan
{

    public Lan(string mediumId, Bruker bruker, DateOnly utlantDato)
    {
        MediumId = mediumId;
        Bruker = bruker;
        UtlantDato = utlantDato;
    }
    public string MediumId { get; }

    public Bruker Bruker { get; }

    public DateOnly UtlantDato { get; }

    public DateOnly? ReturnertDato { get; set; }

}
