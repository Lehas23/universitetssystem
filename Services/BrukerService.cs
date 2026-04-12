public class BrukerService
{
    private readonly List<Student> studentListe;
    private readonly List<Ansatt> ansattListe;

    public BrukerService(
        List<Student> studentListe,
        List<Ansatt> ansattListe)
    {
        this.studentListe = studentListe;
        this.ansattListe = ansattListe;
    }

    public Bruker? FinnBruker(string brukerId)
    {
        return studentListe
        .Cast<Bruker>()
        .Concat(ansattListe)
        .FirstOrDefault(b => string.Equals(b.BrukerId, brukerId, StringComparison.OrdinalIgnoreCase));
    }

    public Bruker? LoggInn(string brukerId, string passord)
    {
        var bruker = FinnBruker(brukerId);

        if (bruker == null)
            return null;
        
        if (bruker.Passord != passord)
            return null;
        
        return bruker;
    }

    public RegistrerResultat RegistrerStudent(string brukerId, string navn, string passord, string epost)
    {
        if (FinnBruker(brukerId) != null)
            return RegistrerResultat.FinnesAllerede;

        Student s = new Student(brukerId, navn, passord, epost);
        studentListe.Add(s);
        return RegistrerResultat.OK;
    }

    public RegistrerResultat RegistrerAnsatt(string brukerId, string navn, string passord, string epost, Stilling stilling)
    {
        if (FinnBruker(brukerId) != null)
            return RegistrerResultat.FinnesAllerede;

        Ansatt a = new Ansatt(brukerId, navn, passord, epost, stilling, "UIA");
        ansattListe.Add(a);
        return RegistrerResultat.OK;
    }
}