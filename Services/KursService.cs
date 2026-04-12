public class KursService
{
    private readonly List<Kurs> kursListe;
    private readonly List<Student> studentListe;

    public KursService(
        List<Kurs> kursListe,
        List<Student> studentListe)
    {
        this.kursListe = kursListe;
        this.studentListe = studentListe;
    }

    public OpprettKursResultat OpprettKurs(
        string kursKode,
        string kursNavn,
        int studiePoeng,
        int maksPlasser)
    {
        bool kursFinnes = kursListe.Any(k =>
            string.Equals(k.Kode, kursKode, StringComparison.OrdinalIgnoreCase));

        if (kursFinnes)
            return OpprettKursResultat.KursFinnesFraFor;
      
        Kurs nyttKurs = new Kurs(kursKode, kursNavn, studiePoeng, maksPlasser);
        kursListe.Add(nyttKurs);
        return OpprettKursResultat.OK;
    }

    public PameldingResultat MeldStudentTilKurs(string studentId, string kursKode)
    {
        var student = studentListe.FirstOrDefault(s => 
            string.Equals(s.BrukerId, studentId, StringComparison.OrdinalIgnoreCase));

        if (student == null)
            return PameldingResultat.StudentFinnesIkke;
        
        var kurs = kursListe.FirstOrDefault(k =>
            string.Equals(k.Kode, kursKode, StringComparison.OrdinalIgnoreCase));

        if (kurs == null)
            return PameldingResultat.KursFinnesIkke;

        var resultat = kurs.LeggTilStudent(student);
        return resultat;
    }

    public AvmeldingResultat MeldStudentAvKurs(string studentId, string kursKode)
    {
        var student = studentListe.FirstOrDefault(s =>
            string.Equals(s.BrukerId, studentId, StringComparison.OrdinalIgnoreCase));

        if (student == null)
            return AvmeldingResultat.StudentFinnesIkke;
        
        var kurs = kursListe.FirstOrDefault(k =>
            string.Equals(k.Kode, kursKode, StringComparison.OrdinalIgnoreCase));

        if (kurs == null)
            return AvmeldingResultat.KursFinnesIkke;
        
        var resultat = kurs.FjernStudent(student);
        return resultat;
    }

    public List<Kurs> SokPaKurs(string sok)
    {
        var treff = kursListe
            .Where(k =>
                k.Kode.Contains(sok, StringComparison.OrdinalIgnoreCase) ||
                k.Navn.Contains(sok, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return treff;
    }

    public List<Kurs> HentAlleKurs()
    {
        return kursListe;
    }

    public List<Kurs> HentKursForStudent(string studentId)
    {
        var student = studentListe.FirstOrDefault(s =>
            string.Equals(s.BrukerId, studentId, StringComparison.OrdinalIgnoreCase));

        if (student == null)
        {
            return new List<Kurs>();
        }

        var kurs = kursListe
            .Where(k => k.Deltagere.Any(s =>
             string.Equals(s.BrukerId, studentId, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        return kurs;
    }

    public SettKarakterResultat SettKarakter(string kursKode, string studentId, string karakter)
    {
        var kurs = kursListe.FirstOrDefault(k =>
            string.Equals(k.Kode, kursKode, StringComparison.OrdinalIgnoreCase));

        if (kurs == null)
            return SettKarakterResultat.KursFinnesIkke;

        if (!kurs.Deltagere.Any(s => s.BrukerId == studentId))
            return SettKarakterResultat.StudentIkkeMedIKurs;

        var student = kurs.Deltagere.FirstOrDefault(s => s.BrukerId == studentId);
        if (student == null)
            return SettKarakterResultat.StudentIkkeMedIKurs;

        kurs.Karakterer[student] = karakter;
        return SettKarakterResultat.OK;
    }

    public RegistrerPensumResultat RegistrerPensum(string kursKode, string pensum)
    {
        var kurs = kursListe.FirstOrDefault(k =>
            string.Equals(k.Kode, kursKode, StringComparison.OrdinalIgnoreCase));

        if (kurs == null)
            return RegistrerPensumResultat.KursFinnesIkke;

        kurs.Pensum.Add(pensum);
        return RegistrerPensumResultat.OK;
    }
}