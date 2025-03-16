using System;
using System.Collections;
using System.Collections.Generic; // Importerer System.Collections.Generic for at bruge lister

class Program
{
    // Opretter en liste til at gemme spilobjekter
    //public static List<Game> Stock { get; private set; } = new List<Game>();
    static List<Game> lager = new List<Game>();
    private static string condition;
    private static string reperation;
    private static string title;
    private static double price;
    private static int stockstatus;
    private static string? gameName;
    private static int stockStatus;

    static void Main()
    {
        // Tilføjer nogle eksempler på brætspil til listen
        lager.Add(new Game("Sequence", "Strategi", 135, 5, "slidt"));
        lager.Add(new Game("7 wonders", "Familie", 75, 8, "ok"));
        lager.Add(new Game("Matador", "Familie", 150, 2, "ny"));
        lager.Add(new Game("Bad People", "Strategi", 45, 3, "ok"));
        lager.Add(new Game("Alverdens", "Familie", 32.50, 2, "ok"));
        lager.Add(new Game("Ticket To Ride", "Strategi", 25, 3, "slidt"));
        lager.Add(new Game("Carcassonne", "Strategi", 50, 4, "ny"));
        SaveToFile(); // Gemmer de hardkodede spil til filen linje 119

        while (true) // Løkke, der holder programmet kørende, indtil brugeren vælger at afslutte
        {
            // Viser menuen til brugeren

            Console.WriteLine("\n1. Vis spil\n2. Tilføj spil\n3. Sletspil\n4. Henter & opdater fil\n5. ReserverSpil\n6. Afslut program");
            Console.Write("Vælg en mulighed: ");
            string valg = Console.ReadLine(); // Læser brugerens input

            switch (valg) // Switch-case til at håndtere brugerens valg
            {
                case "1":
                    ShowGame(); // Kalder metoden til at vise alle spil
                    break;
                case "2":
                    AddGame(); // Kalder metoden til at tilføje et nyt spil
                    SaveToFile(); // Gemmer spil til fil efter tilføjelse
                    ShowGame(); // Viser alle spil i lageret
                    break;
                case "3":
                    DeleteGame(lager); // Kalder metoden til at slette et spil
                    SaveToFile(); // Gemmer og opdater spilfil efter sletning
                    break;
                case "4":
                    GetFromFile(); // Henter spil fra fil
                    Console.WriteLine("Spillene er blevet hentet fra filen.");
                    break;
                case "5":
                    ReservesGame(); // Kalder metoden til at reservere et spil
                    break;
                case "6":
                    return; // Afslutter programmet
                default:
                    Console.WriteLine("Ugyldigt valg. Prøv igen."); // Fejlmeddelelse ved forkert input
                    break;
            }
        }
    }
    // Metode der viser alle spil i lageret ved at udskrive dem til konsollen
    private static void ShowGame() => lager.ForEach(Console.WriteLine);

    
    private static void AddGame()
    {
        // Beder brugeren om at indtaste titel og gemmer inputtet
        Console.Write("Tilføj titel: "); string title = Console.ReadLine();
        // Beder brugeren om at indtaste genre og gemmer inputtet
        Console.Write("Tilføj genre: "); string genre = Console.ReadLine();
        // Beder brugeren om at indtaste pris og gemmer inputtet
        Console.Write("Tilføj pris: "); double price = double.Parse(Console.ReadLine());
        // Beder brugeren om at indtaste lagerstatus og gemmer inputtet
        Console.Write("Lagerstatus antal er: ");
        while (!int.TryParse(Console.ReadLine(), out stockStatus)) // Tjekker om inputtet er et heltal
        {
            Console.Write("Ugyldigt input! Indtast et heltal for lagerstatus: "); // Fejlmeddelelse ved ugyldigt input
        }
        Console.Write("Stand er: "); string condition = Console.ReadLine(); // Beder brugeren om at indtaste stand og gemmer inputtet

        lager.Add(new Game(title, genre, price, stockStatus, condition)); // Tilføjer det nye spil til lageret
        Console.WriteLine("Spil tilføjet!"); // Besked til brugeren om, at spillet er tilføjet
    }

    // Metode til at slette et spil fra lageret baseret på titel
    private static void DeleteGame(List<Game> lager)
    {
        // Beder brugeren om at indtaste titlen på spillet, der skal slettes
        Console.Write("Indtast titlen på spillet, du vil slette: "); 
        string title = Console.ReadLine();

        var gameToRemove = lager.FirstOrDefault(game => game.Title.Equals(title, StringComparison.OrdinalIgnoreCase)); // Finder spillet i lageret

        if (gameToRemove != null) // Tjekker om spillet blev fundet
        {
            lager.Remove(gameToRemove); // Fjerner spillet fra lageret
            
            Console.WriteLine($"Spillet \"{title}\" er blevet slettet."); // Besked til brugeren om, at spillet er slettet
            SaveToFile(); // Opdaterer filen efter sletning
        }
        else
        {
            Console.WriteLine($"Spillet \"{title}\" blev ikke fundet."); // Besked til brugeren om, at spillet ikke blev fundet
        }
    }

    const string FilePath = "Lager.csv"; // Filens placering

    public static object GameName { get; private set; }
    // public static object Stock { get; private set; }

    // Metode til at gemme spil til en CSV-fil
    static void SaveToFile()
    {
        var linjer = lager.Select(static Game => // Går igennem hvert spil i lageret og omdanner det til en linje i filen
        {
            int stock = Game.StockStatus; // Sikrer, at vi arbejder med en int

            if (stock < 0) // Tjekker kun, hvis stock er en valid int
            {
                Console.WriteLine($"ADVARSEL! '{Game.Title}' har en ugyldig lagerstatus ({stock}). Sætter til 0."); // Advarsel til brugeren
                stock = 0; // Standardværdi for ugyldige værdier
            }

            return $"{Game.Title},{Game.Genre},{Game.Price},{stock},{Game.Condition}"; // Returnerer en formateret linje til filen
        }).ToList(); // Gemmer linjerne i en liste

        File.WriteAllLines(FilePath, linjer); // Skriver linjerne til filen
        Console.WriteLine("Data gemt til fil."); // Besked til brugeren om, at data er gemt
    }

    static void ReservesGame() // Metode til at reservere et spil
    {
        Console.Write("\nIndtast spillets titel: "); // Bed brugeren om at indtaste spillets titel
        string spilNavn = Console.ReadLine(); // Gemmer brugerens input i variablen 'spilNavn'

        // Opretter en variabel 'spil', der starter som null (ingen spil fundet endnu)
        Game game = null;

        // Går igennem alle spil i lageret ét ad gangen
        foreach (var s in lager)
        {
            // Tjekker om spillets titel matcher det spil, brugeren søger efter
            // StringComparison.OrdinalIgnoreCase sikrer, at store og små bogstaver ignoreres
            if (s.Title.Equals(spilNavn, StringComparison.OrdinalIgnoreCase)) 
            {
                // Hvis spillet findes, gemmes det i variablen 'spil'
                game = s;

                // Stopper løkken, da vi har fundet det spil, vi leder efter
                break;
            }
        }

        // Efter løkken vil 'spil' enten være null (hvis spillet ikke blev fundet) 
        // eller indeholde det første matchende spil fra lageret.

        if (game == null) // Hvis spillet ikke blev fundet, gives en besked til brugeren
        {
            Console.WriteLine($"Spillet \"{spilNavn}\" findes ikke i lageret."); 
            return; // Stopper metoden, da der ikke er noget spil at reservere
        }

        if (game.StockStatus > 0) // Hvis spillet er på lager, kan det reserveres
        {
            // Reducerer lagerstatus med 1, fordi spillet bliver reserveret
            game.StockStatus--;
            Console.WriteLine($"Spillet \"{game.Title}\" er nu reserveret! Resterende på lager: {game.StockStatus}"); // Besked til brugeren om, at spillet er reserveret
        }
        else
        {
            // Hvis spillet er udsolgt, gives en besked til brugeren
            Console.WriteLine($"Spillet \"{game.Title}\" er desværre udsolgt.");
        }
    }

    // Metode til at hente spil fra en CSV-fil og gemme dem i lager-listen
    static void GetFromFile()
    {
        if (!File.Exists(FilePath)) // Tjekker om filen findes
        {
            Console.WriteLine("Filen findes ikke."); // Besked til brugeren om, at filen ikke findes
            return; // Stopper metoden, da der ikke er nogen fil at hente fra
        }

        lager = File.ReadAllLines(FilePath) // Læser alle linjer fra filen
            .Select(line => line.Split(',')) // Splitter hver linje ved komma
            .Where(parts => parts.Length >= 5) // Sikrer, at der er nok værdier
            .Select(parts => // Omdanner de splittede dele til et Game-objekt
            {
                // Forsøger at konvertere pris og lagerstatus fra tekst til henholdsvis double og int
                if (double.TryParse(parts[2], out double price) && int.TryParse(parts[3], out int stockStatus)) 
                {
                    return new Game(parts[0], parts[1], price, stockStatus, parts[4]); // Returnerer et nyt Game-objekt
                }
                return null; // Returnerer null for ugyldige linjer
            })
            .Where(game => game != null) // Fjerner ugyldige linjer
            .ToList(); // Gemmer de gyldige Game-objekter i en liste

        Console.WriteLine("Spil hentet fra fil."); // Besked til brugeren om, at spil er hentet

        // Metode til at vise alle spil i lageret

        static void ShowGame()
        {
            Console.WriteLine("\nLagerliste over brætspil:");
            foreach (var Game in lager) // Gennemgår alle spil i listen
            {
                Console.WriteLine(Game); // Udskriver hvert spil (ToString()-metoden kaldes automatisk)
            }
        }

        static void DeleteGame(List<Game> lager)
        {
            // Tjekker om der er spil på lageret
            if (lager.Count == 0)
            {
                Console.WriteLine("Der er ingen spil i lageret.");
                return;
            }

            // Beder brugeren om at indtaste navnet på det spil, der skal slettes
            Console.Write("Indtast navnet på spillet, du vil slette: ");
            string gamename = Console.ReadLine();

            // Forsøger at fjerne spillet fra lageret
            bool fjernet = lager.RemoveAll(Game => Game.Title.Equals(gameName, StringComparison.OrdinalIgnoreCase)) > 0;

            // Udskriver resultatet til brugeren
            Console.WriteLine(fjernet
                ? $"Spillet \"{gamename}\" er blevet fjernet." // Hvis 'fjernet' er true, betyder det, at mindst ét spil blev fjernet, og denne besked vises
                : $"Spillet \"{gamename}\" blev ikke fundet."); // Hvis 'fjernet' er false, blev spillet ikke fundet i lageret, og denne besked vises
        }
    }

           
}

// Klasse, der repræsenterer et brætspil
class Game
{
    // Egenskaber for spilobjektet
    public string Title { get; set; }
    public string Genre { get; set; }
    public double Price { get; set; }
    public int StockStatus { get; set; }  
    public string Condition { get; set; }

    // Konstruktør, der initialiserer et nyt spil
    public Game(string title, string genre, double price, int stockStatus, string condition)
    {
        Title = title;
        Genre = genre;
        Price = price;
        StockStatus = stockStatus;
        Condition = condition;
    }

    public Game(string v1, string v2, double v3, int v4)
    {
        Title = v1;
        Genre = v2;
        Price = v3;
        StockStatus = v4;
    }

    // Overrider ToString()-metoden for at formatere udskriften af et spil
    public override string ToString()
    {
        return $"{Title} ({Genre}) - {Price} DKK - {StockStatus} stk. på lager - stand er: {Condition}";
    }
}





