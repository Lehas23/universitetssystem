public class Student : Bruker
{
    public Student(string brukerId, string navn, string passord, string epost)
        : base(brukerId, navn, passord, epost)
    {
    }
    public override string Rolle => "Student";
    public List<Kurs> Kurs { get; } = new();
}

