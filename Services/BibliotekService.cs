public class BibliotekService
{
    private readonly List<Bok> bokListe;
    private readonly List<Lan> lanListe;
    private readonly List<Student> studentListe;
    private readonly List<Ansatt> ansattListe;

    public BibliotekService(
        List<Bok> bokListe,
        List<Lan> lanListe,
        List<Student> studentListe,
        List<Ansatt> ansattListe)
    {
        this.bokListe = bokListe;
        this.lanListe = lanListe;
        this.studentListe = studentListe;
        this.ansattListe = ansattListe;
    }

    public List<Bok> SokPaBok(string sok)
    {
        var treff = bokListe
            .Where(b =>
                (b.Id ?? "").Contains(sok, StringComparison.OrdinalIgnoreCase) ||
                (b.Tittel ?? "").Contains(sok, StringComparison.OrdinalIgnoreCase) ||
                (b.Forfatter ?? "").Contains(sok, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return treff;
    }

    public LanResultat LanBok(string brukerId, string bokId)   
    {     
        var bruker = FinnBruker(brukerId);

        if (bruker == null)
            return LanResultat.BrukerFinnesIkke;

        var bok = bokListe.FirstOrDefault(b =>
           string.Equals(b.Id, bokId, StringComparison.OrdinalIgnoreCase));

        if (bok == null)
            return LanResultat.BokFinnesIkke;

        bool harAktivtLan = lanListe.Any(l =>
            l.ReturnertDato == null &&
            string.Equals(l.Bruker.BrukerId, bruker.BrukerId, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(l.MediumId, bok.Id, StringComparison.OrdinalIgnoreCase));

        if (harAktivtLan)
            return LanResultat.AlleredeLan;
        
        ILanbar medium = bok;

        if (medium.LaanUt())
        {
            Lan nyttLan = new Lan(medium.Id, bruker, DateOnly.FromDateTime(DateTime.Now));
            lanListe.Add(nyttLan);
            return LanResultat.OK;
        }

        return LanResultat.IngenEksemplarer;
    }
    public ReturnerResultat ReturnerBok(string brukerId, string bokId)
    {
        var bruker = FinnBruker(brukerId);

        if (bruker == null)
            return ReturnerResultat.BrukerFinnesIkke;
       
        var lan = lanListe.FirstOrDefault(l =>
            string.Equals(l.MediumId, bokId, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(l.Bruker.BrukerId, brukerId, StringComparison.OrdinalIgnoreCase) &&
            l.ReturnertDato == null);

        if (lan == null)
            return ReturnerResultat.AktivtLanFinnesIkke;
        
        var bok = bokListe.FirstOrDefault(b =>
            string.Equals(b.Id, bokId, StringComparison.OrdinalIgnoreCase));

        if (bok == null)
            return ReturnerResultat.BokFinnesIkke;
        
        lan.ReturnertDato = DateOnly.FromDateTime(DateTime.Now);
        ILanbar medium = bok;
        medium.LeverInn();
        return ReturnerResultat.OK;
    }

    public RegistrerBokResultat RegistrerBok(
        string bokId,
        string bokTittel,
        string bokForfatter,
        int bokAar,
        int bokAntallEksemplarer)
    {
      bool finnes = bokListe.Any(b =>
            string.Equals(b.Id, bokId, StringComparison.OrdinalIgnoreCase));

      if (finnes)
            return RegistrerBokResultat.BokFinnesFraFor;

        Bok nyBok = new Bok(bokId, bokTittel, bokForfatter, bokAar, bokAntallEksemplarer);
        bokListe.Add(nyBok);

        return RegistrerBokResultat.OK;
    }

    public List<Lan> HentAktiveLan()
    {
        return lanListe
            .Where(l => l.ReturnertDato == null)
            .ToList();
    }

    public List<Lan> HentHistorikk()
    {
        return lanListe;
    }

    private Bruker? FinnBruker(string brukerId)
    {
        return studentListe
            .Cast<Bruker>()
            .Concat(ansattListe)
            .FirstOrDefault(b => string.Equals(b.BrukerId, brukerId, StringComparison.OrdinalIgnoreCase));
    }
}

       

