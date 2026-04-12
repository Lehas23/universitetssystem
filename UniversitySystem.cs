public class UniversitySystem
{
    // --- Init ---

    private readonly List<Kurs> kursListe = new();

    private readonly List<Student> studentListe = new();

    private readonly List<Ansatt> ansattListe = new();

    private readonly List<Bok> bokListe = new();

    private readonly List<Lan> lanListe = new();

    private BibliotekService bibliotekService;
    private KursService kursService;
    private BrukerService brukerService;

    public UniversitySystem()
    {
        bibliotekService = new BibliotekService(
            bokListe,
            lanListe,
            studentListe,
            ansattListe
            );

        kursService = new KursService(
            kursListe,
            studentListe
            );

        brukerService = new BrukerService(
            studentListe,
            ansattListe
            );
    }

    // --- Auth ---

    public Bruker? LoggInn()
    {
        string brukerId = LesIkkeTom("Skriv inn Bruker-ID:", "ID kan ikke være tom.");
        string passord = LesIkkeTom("Skriv inn Passord:", "Passord kan ikke være tomt.");

        var bruker = brukerService.LoggInn(brukerId, passord);

        if (bruker == null)
        {
            Console.WriteLine("Feil brukernavn eller passord.");
            return null;
        }

        return bruker;
    }

    public void RegistrerBruker()
    {
        Console.WriteLine("1. Student");
        Console.WriteLine("2. Ansatt");
        string valg = Console.ReadLine() ?? "";

        string brukerId = LesIkkeTom("Skriv inn ID:", "ID kan ikke være tom.");
        string navn = LesIkkeTom("Skriv inn Navn:", "Navn kan ikke være tomt");
        string passord = LesIkkeTom("Skriv inn Passord:", "Passord kan ikke være tomt");
        string epost = LesIkkeTom("Skriv inn Epost:", "Epost kan ikke være tom");

        if (valg == "1")
        {
            var resultat = brukerService.RegistrerStudent(brukerId, navn, passord, epost);

            switch (resultat)
            {
                case RegistrerResultat.FinnesAllerede:
                    Console.WriteLine("Bruker finnes allerede");
                    break;

                case RegistrerResultat.OK:
                    Console.WriteLine("Bruker opprettet!");
                    break;
            }
        }
        else if (valg == "2")
        {
            Console.WriteLine("1. Faglærer");
            Console.WriteLine("2. Bibliotekar");
            string stillingsvalg = Console.ReadLine() ?? "";

            Stilling stilling;

            switch (stillingsvalg)
            {
                case "1":
                    stilling = Stilling.Faglærer;
                    break;

                case "2":
                    stilling = Stilling.Bibliotekar;
                    break;

                default:
                    Console.WriteLine("Ugyldig valg.");
                    return;
            }

            var resultat = brukerService.RegistrerAnsatt(brukerId, navn, passord, epost, stilling);

            switch (resultat)
            {
                case RegistrerResultat.FinnesAllerede:
                    Console.WriteLine("Bruker finnes allerede.");
                    break;

                case RegistrerResultat.OK:
                    Console.WriteLine("Bruker opprettet!");
                    break;
            }
        }
    }

    // --- Kurs ---

    public void OpprettKurs()
    {
        string kursNavn = LesIkkeTom("Skriv inn kursnavn:", "Kursnavn kan ikke være tom.");
        string kursKode = LesIkkeTom("Skriv inn kurskode:", "Kurskode kan ikke være tom.");

        int studiePoeng = LesHeltall("Skriv inn antall studiepoeng", 1);
        int maksPlasser = LesHeltall("Skriv inn maks antall plasser", 1);

        var resultat = kursService.OpprettKurs(
            kursKode,
            kursNavn,
            studiePoeng,
            maksPlasser
        );

        switch (resultat)
        {
            case OpprettKursResultat.KursFinnesFraFor:
                Console.WriteLine("Kurskoden finnes fra før av.");
                break;

            case OpprettKursResultat.OK:
                Console.WriteLine("Kurs opprettet!");
                break;
        }
    }

    public void MeldStudentTilKurs(Bruker bruker)
    {
        string kursKode = LesIkkeTom("Skriv inn kurskode:", "Kurskode kan ikke være tom.");

        var resultat = kursService.MeldStudentTilKurs(bruker.BrukerId, kursKode);

        switch (resultat)
        {
            case PameldingResultat.KursFinnesIkke:
                Console.WriteLine("Fant ikke kurs.");
                break;
                   
            case PameldingResultat.AlleredePameldt:
                Console.WriteLine("Du er allerede påmeldt");
                break;

            case PameldingResultat.Fullt:
                Console.WriteLine("Kurset er fullt.");
                break;

            case PameldingResultat.OK:
                Console.WriteLine($"Du er nå påmeldt i {kursKode}");
                break;
        }
    }

    public void MeldStudentAvKurs(Bruker bruker)
    {
        string kursKode = LesIkkeTom("Skriv inn kurskode:", "Kurskode kan ikke være tom.");
        
        var resultat = kursService.MeldStudentAvKurs(bruker.BrukerId, kursKode);

        switch (resultat)
        {
            case AvmeldingResultat.KursFinnesIkke:
                Console.WriteLine("Fant ikke kurs.");
                break;

            case AvmeldingResultat.IkkePameldt:
                Console.WriteLine("Du er ikke påmeldt i dette kurset.");
                break;

            case AvmeldingResultat.OK:
                Console.WriteLine($"Du er nå fjernet fra {kursKode}.");
                break;
        }
    }
 
    public void SokPaKurs()
    {
        string sok = LesIkkeTom("Skriv inn kursnavn eller kurskode:", "Kurskode eller kursnavn kan ikke være tomt.");

        var treff = kursService.SokPaKurs(sok);

        if (!treff.Any())
        {
            Console.WriteLine("Ingen kurs funnet.");
            return;
        }

        foreach (var k in treff)
        {
            Console.WriteLine($"{k.Kode} - {k.Navn}");
        }
        
    }

    public void PrintKursOgDeltagere()
    {
        var kursListe = kursService.HentAlleKurs();

        if (!kursListe.Any())
        {
            Console.WriteLine("Ingen kurs opprettet.");
            return;
        }

        foreach (var k in kursListe)
        {
            Console.WriteLine($"{k.Kode} - {k.Navn}");

            if (k.Deltagere.Count == 0)
                Console.WriteLine("Ingen deltagere");
            else
            {
                foreach (var student in k.Deltagere)
                {
                    Console.WriteLine($"{student.BrukerId} - {student.Navn}");
                }
            }
        }
    }

    public void SeMineKurs(Bruker bruker)
    {
        var kurs = kursService.HentKursForStudent(bruker.BrukerId);

        if (!kurs.Any())
        {
            Console.WriteLine("Ingen kurs funnet.");
            return;
        }

        foreach (var k in kurs)
        {
            Console.WriteLine($"{k.Kode} - {k.Navn}");
        }
    }

    public void SettKarakter()
    {
        string kursKode = LesIkkeTom("Skriv inn kurskode:", "Kurskode kan ikke være tom.");
        string studentId = LesIkkeTom("Skriv inn Student-ID:", "Student-ID kan ikke være tom.");
        string karakter = LesIkkeTom("Skriv inn karakter:", "Karakter kan ikke være tom.");

        var resultat = kursService.SettKarakter(kursKode, studentId, karakter);

        switch (resultat)
        {
            case SettKarakterResultat.KursFinnesIkke:
                Console.WriteLine("Fant ikke kurs.");
                break;

            case SettKarakterResultat.StudentIkkeMedIKurs:
                Console.WriteLine("Studenten er ikke med i dette kurset.");
                break;

            case SettKarakterResultat.OK:
                Console.WriteLine("Karakter satt!");
                break;
        }
    }

    public void SeKarakterer(Bruker bruker)
    {
        var kursListe = kursService.HentKursForStudent(bruker.BrukerId);

        if (!kursListe.Any())
        {
            Console.WriteLine("Ingen kurs.");
            return;
        }

        foreach (var k in kursListe)
        {
            var student = k.Deltagere.FirstOrDefault(s => s.BrukerId == bruker.BrukerId);

            if (student != null && k.Karakterer.ContainsKey(student))
            {
                Console.WriteLine($"{k.Kode}: {k.Karakterer[student]}");
            }
            else
            {
                Console.WriteLine($"{k.Kode}: Ingen karakter ennå.");
            }
        }
    }

    public void RegistrerPensum()
    {
        string kursKode = LesIkkeTom("Skriv inn kurskode:", "Kurskode kan ikke være tom.");
        string pensum = LesIkkeTom("Skriv inn pensum:", "Pensum kan ikke være tom.");

        var resultat = kursService.RegistrerPensum(kursKode, pensum);

        switch (resultat)
        {
            case RegistrerPensumResultat.KursFinnesIkke:
                Console.WriteLine("Fant ikke kurs.");
                break;

            case RegistrerPensumResultat.OK:
                Console.WriteLine("Pensum registrert!");
                break;
        }
    }

    // --- Bibliotek ---

    public void RegistrerBok()
    {
        string bokId = LesIkkeTom("Skriv inn Bok-ID:", "Bok-ID kan ikke være tom.");
        string bokTittel = LesIkkeTom("Skriv inn Tittel:", "Tittel kan ikke være tom.");
        string bokForfatter = LesIkkeTom("Skriv inn Forfatter:", "Forfatter kan ikke være tom.");

        int bokAar;
        bool gyldigAar;

        do
        {
            Console.WriteLine("Skriv inn år:");
            gyldigAar = int.TryParse(Console.ReadLine(), out bokAar);

            if (!gyldigAar)
            {
                Console.WriteLine("Ugyldig tall, prøv igjen.");
            }
            else if (bokAar < 1000 || bokAar > DateTime.Now.Year)
            {
                Console.WriteLine("Ugyldig år.");
                gyldigAar = false;
            }

        } while (!gyldigAar);

        int bokAntallEksemplarer = LesHeltall("Skriv inn antall eksemplarer:", 1);

        var resultat = bibliotekService.RegistrerBok(
            bokId,
            bokTittel,
            bokForfatter,
            bokAar,
            bokAntallEksemplarer
        );

        switch (resultat)
        {
            case RegistrerBokResultat.BokFinnesFraFor:
                Console.WriteLine("Bok-ID finnes allerede.");
                break;

            case RegistrerBokResultat.OK:
                Console.WriteLine("Bok registrert!");
                break;
        }
    }

    public void SokPaBok()
    {
        string sok = LesIkkeTom("Skriv inn søk (id, tittel eller forfatter):", "Søk kan ikke være tomt.");

        var treff = bibliotekService.SokPaBok(sok);

        if (treff.Count == 0)
        {
            Console.WriteLine("Ingen bøker funnet");
        }
        else
        {
            foreach (var b in treff)
            { 
                Console.WriteLine($"{b.Id} - {b.Tittel} - {b.Forfatter} - {b.AntallEksemplarer}");
            }
        }
    }

    public void LanBok(Bruker bruker)
    {
        string bokId = LesIkkeTom("Skriv inn bok-ID:", "Bok-ID kan ikke være tom.");
        var resultat = bibliotekService.LanBok(bruker.BrukerId, bokId);

        switch (resultat)
        {
            case LanResultat.BrukerFinnesIkke:
                Console.WriteLine("Fant ingen bruker med den ID-en.");
                break;

            case LanResultat.BokFinnesIkke:
                Console.WriteLine("Bok-ID finnes ikke.");
                break;

            case LanResultat.AlleredeLan:
                Console.WriteLine("Brukeren har allerede et aktivt lån på denne boken.");
                break;

            case LanResultat.IngenEksemplarer:
                Console.WriteLine("Ingen eksemplarer tilgjengelig.");
                break;

            case LanResultat.OK:
                Console.WriteLine("Bok utlånt!");
                break;
        }        
    }

    public void ReturnerBok(Bruker bruker)
    {
        string bokId = LesIkkeTom("Skriv inn bok-ID:", "Bok-ID kan ikke være tom.");
        var resultat = bibliotekService.ReturnerBok(bruker.BrukerId, bokId);

        switch (resultat)
        {
            case ReturnerResultat.BrukerFinnesIkke:
                Console.WriteLine("Fant ingen bruker med den ID-en.");
                break;

            case ReturnerResultat.AktivtLanFinnesIkke:
                Console.WriteLine("Ingen aktivt lån funnet.");
                break;

            case ReturnerResultat.BokFinnesIkke:
                Console.WriteLine("Bok-ID finnes ikke.");
                break;

            case ReturnerResultat.OK:
                Console.WriteLine("Bok returnert!");
                break;
        }
    }

    private void PrintLan(List<Lan> lanListe)
    {
        foreach (var l in lanListe)
        {
            string returTekst = l.ReturnertDato == null
                ? "Ikke returnert"
                : l.ReturnertDato.ToString()!;

            Console.WriteLine($"{l.MediumId} - {l.Bruker.Navn} - {l.Bruker.Rolle} - Utlånt: {l.UtlantDato} - Returnert: {returTekst}");
        }
    }

    // --- Menyer ---

    public void Run()
    {
        bool kjorer = true;

        while (kjorer)
        {
            Console.WriteLine("1. Logg inn");
            Console.WriteLine("2. Registrer bruker");
            Console.WriteLine("0. Avslutt");

            string valg = Console.ReadLine() ?? "";

            switch (valg)
            {
                case "1":
                    var bruker = LoggInn();
                    if (bruker != null)
                    {
                        SendTilRiktigMeny(bruker);
                    }
                    break;

                case "2":
                    RegistrerBruker();
                    break;

                case "0":
                    kjorer = false;
                    break;
            }
        }
    }

    private void SendTilRiktigMeny(Bruker bruker)
    {
        if (bruker is Student)
        {
            VisStudentMeny(bruker);
        }
        else if (bruker is Ansatt ansatt)
        {
            if (ansatt.Stilling == Stilling.Faglærer)
            {
                VisFagLaererMeny(ansatt);
            }
            else if (ansatt.Stilling == Stilling.Bibliotekar)
            {
                VisBibliotekarMeny(ansatt);
            }
        }
    }

    public void VisStudentMeny(Bruker bruker)
    {
        bool kjorer = true;

        while (kjorer)
        {
            Console.WriteLine("1. Meld til kurs");
            Console.WriteLine("2. Meld av kurs");
            Console.WriteLine("3. Se karakterer");
            Console.WriteLine("4. Søk på bok");
            Console.WriteLine("5. Lån bok");
            Console.WriteLine("6. Returner bok");
            Console.WriteLine("7. Mine kurs");
            Console.WriteLine("0. Logg ut");

            string valg = Console.ReadLine() ?? "";

            switch (valg)
            {
                case "1":
                    MeldStudentTilKurs(bruker);
                    break;

                case "2":
                    MeldStudentAvKurs(bruker);
                    break;

                case "3":
                    SeKarakterer(bruker);
                    break;

                case "4":
                    SokPaBok();
                    break;

                case "5":
                    LanBok(bruker);
                    break;

                case "6":
                    ReturnerBok(bruker);
                    break;

                case "7":
                    SeMineKurs(bruker);
                    break;

                case "0":
                    kjorer = false;
                    break;
            }
        }
    }

    public void VisFagLaererMeny(Ansatt ansatt)
    {
        bool kjorer = true;

        while (kjorer)
        {
            Console.WriteLine("1. Opprett kurs");
            Console.WriteLine("2. Søk på kurs");
            Console.WriteLine("3. Søk på bok");
            Console.WriteLine("4. Lån bok");
            Console.WriteLine("5. Returner bok");
            Console.WriteLine("6. Sett karakter");
            Console.WriteLine("7. Registrer pensum");
            Console.WriteLine("8. Se kurs og deltagere");
            Console.WriteLine("0. Logg ut");

            string valg = Console.ReadLine() ?? "";

            switch (valg)
            {
                case "1":
                    OpprettKurs();
                    break;

                case "2":
                    SokPaKurs();
                    break;

                case "3":
                    SokPaBok();
                    break;

                case "4":
                    LanBok(ansatt);
                    break;

                case "5":
                    ReturnerBok(ansatt);
                    break;

                case "6":
                    SettKarakter();
                    break;

                case "7":
                    RegistrerPensum();
                    break;

                case "8":
                    PrintKursOgDeltagere();
                    break;

                case "0":
                    kjorer = false;
                    break;
            }
        }
    }

    public void VisBibliotekarMeny(Ansatt ansatt)
    {
        bool kjorer = true;

        while (kjorer)
        {
            Console.WriteLine("1. Registrer bok");
            Console.WriteLine("2. Se aktive lån");
            Console.WriteLine("3. Se historikk");
            Console.WriteLine("0. Logg ut");

            string valg = Console.ReadLine() ?? "";

            switch (valg)
            {
                case "1":
                    RegistrerBok();
                    break;

                case "2":
                    var aktive = bibliotekService.HentAktiveLan();
                    if (!aktive.Any())
                        Console.WriteLine("Ingen aktive lån.");
                    else
                        PrintLan(aktive);
                    break;

                case "3":
                    var historikk = bibliotekService.HentHistorikk();
                    if (!historikk.Any())
                        Console.WriteLine("Ingen historikk.");
                    else
                        PrintLan(historikk);
                    break;

                case "0":
                    kjorer = false;
                    break;
            }
        }
    }

    // --- Helpers ---

    private string LesIkkeTom(string prompt, string feilmelding)
    {
        string input;

        do
        {
            Console.WriteLine(prompt);
            input = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine(feilmelding);
            }

        } while (string.IsNullOrWhiteSpace(input));

        return input;
    }

    private int LesHeltall(string prompt, int min)
    {
        int verdi;

        do
        {
            Console.WriteLine(prompt);

            if (!int.TryParse(Console.ReadLine(), out verdi) || verdi < min)
            {
                Console.WriteLine($"Ugyldig tall. Må være minst {min}.");
                verdi = 0;
            }
        } while (verdi < min);

        return verdi;
    }
}
