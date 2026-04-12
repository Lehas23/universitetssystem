using System.Collections.Generic;
using Xunit;

public class KursServiceTests
{
    [Fact]
    public void OpprettKurs_NyKode_ReturnererOK()
    {
        var kursListe = new List<Kurs>();
        var studentListe = new List<Student>();
        var service = new KursService(kursListe, studentListe);

        var resultat = service.OpprettKurs("IS-110", "Databaser", 10, 30);

        Assert.Equal(OpprettKursResultat.OK, resultat);
    }

    [Fact]
    public void OpprettKurs_SammeKode_ReturnererKursFinnesFraFor()
    {
        var kursListe = new List<Kurs>();
        var studentListe = new List<Student>();
        var service = new KursService(kursListe, studentListe);
        service.OpprettKurs("IS-110", "Databaser", 10, 30);

        var resultat = service.OpprettKurs("IS-110", "Databaser", 10, 30);

        Assert.Equal(OpprettKursResultat.KursFinnesFraFor, resultat);
    }

    [Fact]
    public void Lanbok_IngenEksemplarer_ReturnererIngenEksemplarer()
    {
        var bokListe = new List<Bok>();
        var lanListe = new List<Lan>();
        var studentListe = new List<Student>();
        var ansattListe = new List<Ansatt>();
        var service = new BibliotekService(bokListe, lanListe, studentListe, ansattListe);

        var bok = new Bok("B1", "Tittel", "Forfatter", 2020, 0);
        var student = new Student("S1", "Test", "1234", "test@uia.no");
        bokListe.Add(bok);
        studentListe.Add(student);

        var resultat = service.LanBok("S1", "B1");

        Assert.Equal(LanResultat.IngenEksemplarer, resultat);
    }

    [Fact]
    public void MeldStudentTilKurs_SammeKurs_ReturnererAlleredePameldt()
    {
        var kursListe = new List<Kurs>();
        var studentListe = new List<Student>();

        var service = new KursService(kursListe, studentListe);

        var kurs = new Kurs("IS-110", "Databaser", 10, 30);
        var student = new Student("S1", "Test", "1234", "test@uia.no");
        kursListe.Add(kurs);
        studentListe.Add(student);

        service.MeldStudentTilKurs("S1", "IS-110");
        var resultat = service.MeldStudentTilKurs("S1", "IS-110");

        Assert.Equal(PameldingResultat.AlleredePameldt, resultat);
    }
}