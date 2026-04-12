public class Kurs
{
    public string Kode { get; }
    public string Navn { get; }
    public int Studiepoeng { get; }
    public int MaksPlasser { get; }

    public Kurs(string kode, string navn, int studiepoeng, int maksPlasser)
    {
        Kode = kode;
        Navn = navn;
        Studiepoeng = studiepoeng;
        MaksPlasser = maksPlasser;
    }

    private readonly List<Student> deltagere = new();
    public IReadOnlyList<Student> Deltagere => deltagere;
    public List<string> Pensum { get; } = new();
    public Dictionary<Student, string> Karakterer { get; } = new();

    public PameldingResultat LeggTilStudent(Student student)
    {
        if (deltagere.Any(s => s.BrukerId == student.BrukerId))
            return PameldingResultat.AlleredePameldt;
        if (deltagere.Count >= MaksPlasser) 
            return PameldingResultat.Fullt;

        deltagere.Add(student);
        student.Kurs.Add(this);
        return PameldingResultat.OK;
    }

    public AvmeldingResultat FjernStudent(Student student)
    {
        if (deltagere.Remove(student))
        {
            student.Kurs.Remove(this);

            return AvmeldingResultat.OK;
        }
        else
            return AvmeldingResultat.IkkePameldt;
    }
}

