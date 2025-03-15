using System;
using System.Collections;
using System.Collections.Generic; // Importerer System.Collections.Generic for at bruge lister

class Program
{
    // Opretter en liste til at gemme spilobjekter
    static List<Spil> lager = new List<Spil>();
    private static string stand;
    private static string reperation;

    static void Main()
    {
        // Tilføjer nogle eksempler på brætspil til listen
        lager.Add(new Spil("Sequence", "Strategi", 135, 5, "slidt"));
        lager.Add(new Spil("7 wonders", "Familie", 75, 8, "ok"));
        lager.Add(new Spil("Matador", "Familie", 150, 2, "ny"));
        lager.Add(new Spil("Bad People", "Strategi", 45, 3, "ok"));
        lager.Add(new Spil("Aldverdens", "Familie", 32.50, 2, "ok"));
        lager.Add(new Spil("Ticket To Ride", "Strategi", 25, 3, "slidt"));

        GemTilFil(); // Gemmer de hardkodede spil til filen linje 65

        while (true) // Løkke, der holder programmet kørende, indtil brugeren vælger at afslutte
        {
            // Viser menuen til brugeren

            Console.WriteLine("\n1. Vis spil\n2. Tilføj spil\n3. Sletspil\n4. Henter & opdater fil\n5. ReserverSpil\n6. Afslut program");
            Console.Write("Vælg en mulighed: ");
            string valg = Console.ReadLine(); // Læser brugerens input

            switch (valg) // Switch-case til at håndtere brugerens valg
            {
                case "1":
                    VisSpil(); // Kalder metoden til at vise alle spil
                    break;
                case "2":
                    TilføjSpil(); // Kalder metoden til at tilføje et nyt spil
                    GemTilFil(); // Gemmer spil til fil efter tilføjelse
                    break;
                case "3":
                    SletSpil(lager); // Kalder metoden til at slette et spil
                    GemTilFil(); // Gemmer og opdater spilfil efter sletning
                    break;
                case "4":
                    HentFraFil(); // Henter spil fra fil
                    Console.WriteLine("Spillene er blevet hentet fra filen.");
                    break;
                case "5":
                    ReserverSpil(); // Kalder metoden til at reservere et spil
                    break;
                case "6":
                    return; // Afslutter programmet
                default:
                    Console.WriteLine("Ugyldigt valg. Prøv igen."); // Fejlmeddelelse ved forkert input
                    break;
            }
        }
    }


    const string FilePath = "spilLager.csv"; // Filens placering

    // Metode til at gemme spil til en CSV-fil
    static void GemTilFil()
    {
        // Konverterer listen over spil til en liste af kommaseparerede strenge, så de kan gemmes i en fil
        var linjer = lager.Select(spil => $"{spil.Titel},{spil.Genre},{spil.Pris},{spil.Lagerstatus}");
        // Skriver alle linjer til filen på den angivne filsti
        File.WriteAllLines(FilePath, linjer);
    }

    static void ReserverSpil()
    {
        Console.Write("\nIndtast spillets titel: ");
        string spilNavn = Console.ReadLine();

        // Opretter en variabel 'spil', der starter som null (ingen spil fundet endnu)
        Spil spil = null;

        // Går igennem alle spil i lageret ét ad gangen
        foreach (var s in lager)
        {
            // Tjekker om spillets titel matcher det spil, brugeren søger efter
            // StringComparison.OrdinalIgnoreCase sikrer, at store og små bogstaver ignoreres
            if (s.Titel.Equals(spilNavn, StringComparison.OrdinalIgnoreCase))
            {
                // Hvis spillet findes, gemmes det i variablen 'spil'
                spil = s;

                // Stopper løkken, da vi har fundet det spil, vi leder efter
                break;
            }
        }

        // Efter løkken vil 'spil' enten være null (hvis spillet ikke blev fundet) 
        // eller indeholde det første matchende spil fra lageret.

        if (spil == null)
        {
            Console.WriteLine($"Spillet \"{spilNavn}\" findes ikke i lageret.");
            return;
        }

        if (spil.Lagerstatus > 0)
        {
            // Reducerer lagerstatus med 1, fordi spillet bliver reserveret
            spil.Lagerstatus--;
            Console.WriteLine($"Spillet \"{spil.Titel}\" er nu reserveret! Resterende på lager: {spil.Lagerstatus}");
        }
        else
        {
            // Hvis spillet er udsolgt, gives en besked til brugeren
            Console.WriteLine($"Spillet \"{spil.Titel}\" er desværre udsolgt.");
        }
    }

    // Metode til at hente spil fra en CSV-fil og gemme dem i lager-listen
    static void HentFraFil()
    {
        if (File.Exists(FilePath)) // Tjekker om filen findes, så vi ikke forsøger at læse en ikke-eksisterende fil
        {
            var linjer = File.ReadAllLines(FilePath); // Læser alle linjer fra filen og gemmer dem som en array af strings
            lager = linjer.Select(line => // Går igennem hver linje og konverterer den til et Spil-objekt
            {
                var parts = line.Split(','); // Splitter linjen ved komma for at få de individuelle værdier
                return new Spil(parts[0], parts[1], double.Parse(parts[2]), int.Parse(parts[3])); // Opretter et nyt Spil-objekt ud fra værdierne
            }).ToList(); // Konverterer resultatet til en liste og gemmer den i 'lager'
        }
    }

    // Metode til at vise alle spil i lageret
    static void VisSpil()
    {
        Console.WriteLine("\nLagerliste over brætspil:");
        foreach (var spil in lager) // Gennemgår alle spil i listen
        {
            Console.WriteLine(spil); // Udskriver hvert spil (ToString()-metoden kaldes automatisk)
        }
    }

    static void SletSpil(List<Spil> lager)
    {
        // Tjekker om der er spil på lageret
        if (lager.Count == 0)
        {
            Console.WriteLine("Der er ingen spil i lageret.");
            return;
        }

        // Beder brugeren om at indtaste navnet på det spil, der skal slettes
        Console.Write("Indtast navnet på spillet, du vil slette: ");
        string spilNavn = Console.ReadLine();

        // Forsøger at fjerne spillet fra lageret
        bool fjernet = lager.RemoveAll(spil => spil.Titel.Equals(spilNavn, StringComparison.OrdinalIgnoreCase)) > 0;

        // Udskriver resultatet til brugeren
        Console.WriteLine(fjernet
            ? $"Spillet \"{spilNavn}\" er blevet fjernet." // Hvis 'fjernet' er true, betyder det, at mindst ét spil blev fjernet, og denne besked vises
            : $"Spillet \"{spilNavn}\" blev ikke fundet."); // Hvis 'fjernet' er false, blev spillet ikke fundet i lageret, og denne besked vises
    }

    // Metode til at tilføje et nyt spil til lageret
    static void TilføjSpil()
        {
            // Beder brugeren om input til det nye spil
            Console.Write("Indtast titel: ");
            string titel = Console.ReadLine();

            Console.Write("Indtast genre: ");
            string genre = Console.ReadLine();

            Console.Write("Indtast pris: ");
            double pris = double.Parse(Console.ReadLine()); // Konverterer input fra string til double

            Console.Write("Indtast lagerstatus (antal på lager): ");
            int lagerstatus = int.Parse(Console.ReadLine()); // Konverterer input fra string til int

            Console.Write("Indtast stand: ");
            string stand = Console.ReadLine();

        // Opretter et nyt spilobjekt og tilføjer det til listen
        lager.Add(new Spil(titel, genre, pris, lagerstatus, stand));
            Console.WriteLine("Spil tilføjet!");
        }
        
}

// Klasse, der repræsenterer et brætspil
class Spil
{
    // Egenskaber for spilobjektet
    public string Titel { get; set; }
    public string Genre { get; set; }
    public double Pris { get; set; }
    public int Lagerstatus { get; set; }
    public string Stand { get; set; }
    public string Reperation { get; set; }

    // Konstruktør, der initialiserer et nyt spil
    public Spil(string titel, string genre, double pris, int lagerstatus, string stand = "Ukendt")
    {
        Titel = titel;
        Genre = genre;
        Pris = pris;
        Lagerstatus = lagerstatus;
        Stand = stand;
        
    }

    // Overrider ToString()-metoden for at formatere udskriften af et spil
    public override string ToString()
    {
        return $"{Titel} ({Genre}) - {Pris} DKK - {Lagerstatus} stk. på lager - stand er: {Stand}";
    }
}

