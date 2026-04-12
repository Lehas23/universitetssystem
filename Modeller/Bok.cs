public class Bok : Medium
{
    public string Forfatter { get; private set; }

    public Bok(string id, string tittel, string forfatter, int aar, int antallEksemplarer)
        : base(id, tittel, aar, antallEksemplarer)
    {
        Forfatter = forfatter; 
    }
  }