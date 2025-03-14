using System;
using System.Collections.Generic; // Importerer System.Collections.Generic for at bruge lister

class Program
{
    // Opretter en liste til at gemme spilobjekter
    static List<Spil> lager = new List<Spil>();

    static void Main()
    {
        // Tilføjer nogle eksempler på brætspil til listen
        lager.Add(new Spil("Sequence", "Strategi", 135, 5));
        lager.Add(new Spil("7 wonders", "Familie", 75, 8));
        lager.Add(new Spil("Matador", "Familie", 150, 2));
        lager.Add(new Spil("Bad People", "Strategi", 45, 3));
        lager.Add(new Spil("Aldverdens", "Familie", 32.50, 2));
        lager.Add(new Spil("Ticket To Ride", "Strategi", 25, 3));

        while (true) // Løkke, der holder programmet kørende, indtil brugeren vælger at afslutte
        {
            // Viser menuen til brugeren
            Console.WriteLine("\n1. Vis spil\n2. Tilføj spil\n3. Slette spil\n4. Afslut");
            Console.Write("Vælg en mulighed: ");
            string valg = Console.ReadLine(); // Læser brugerens input

            switch (valg) // Switch-case til at håndtere brugerens valg
            {
                case "1":
                    VisSpil(); // Kalder metoden til at vise alle spil
                    break;
                case "2":
                    TilføjSpil(); // Kalder metoden til at tilføje et nyt spil
                    break;
                case "3":
                    SletSpil(lager); // Kalder metoden til at slette et spil
                    break;
                case "4":
                    return; // Afslutter programmet
                default:
                    Console.WriteLine("Ugyldigt valg. Prøv igen."); // Fejlmeddelelse ved forkert input
                    break;
            }
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

    static void SletSpil(List<string> lager, string spilNavn) 
    {

        Console.WriteLine("\nLagerliste over brætspil:");

        // Tjekker om der er spil på lager
        if (lager.Count == 0)
        {
            Console.WriteLine("Der er ingen spil i lageret.");
            return;
        }

        // Viser listen over spil listet op i rækkefølge (index)
        for (int i = 0; i < lager.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {lager[i]}");
        }

        if (lager.Remove(spilNavn))
        {
            Console.WriteLine($"\nSpillet \"{spilNavn}\" er blevet fjernet fra lageret.");
        }
        else
        {
            Console.WriteLine($"\nSpillet \"{spilNavn}\" blev ikke fundet i lageret.");
        }



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

            // Opretter et nyt spilobjekt og tilføjer det til listen
            lager.Add(new Spil(titel, genre, pris, lagerstatus));
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

    // Konstruktør, der initialiserer et nyt spil
    public Spil(string titel, string genre, double pris, int lagerstatus)
    {
        Titel = titel;
        Genre = genre;
        Pris = pris;
        Lagerstatus = lagerstatus;
    }

    // Overrider ToString()-metoden for at formatere udskriften af et spil
    public override string ToString()
    {
        return $"{Titel} ({Genre}) - {Pris} DKK - {Lagerstatus} stk. på lager";
    }
}

