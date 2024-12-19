using System;
using System.Collections.Generic;

namespace AccountManagement
{
    class Program
    {
        // De hoofdfunctie die het programma uitvoert
        static void Main(string[] args)
        {
            Account account = null;

            // Welkomstbericht
            Console.WriteLine("Welkom bij de Account Management Applicatie!");

            // Vraag de naam van de gebruiker
            Console.Write("Voer uw naam in: ");
            string name = Console.ReadLine();

            // Vraag de leeftijd van de gebruiker en valideer of deze minimaal 18 is
            Console.Write("Voer uw leeftijd in: ");
            int age;
            while (!int.TryParse(Console.ReadLine(), out age) || age < 18)
            {
                Console.WriteLine("Ongeldige leeftijd. U moet minstens 18 jaar oud zijn.");
            }

            // Bereken de geboortedatum van de gebruiker op basis van de ingevoerde leeftijd
            DateTime birthDate = DateTime.Now.AddYears(-age);

            // Genereer een wachtwoord voor de gebruiker
            string password = GeneratePassword(name, birthDate);
            Console.WriteLine($"Uw account is aangemaakt. Uw wachtwoord is: {password}");

            // Maak een nieuw account aan
            account = new Account(name, birthDate);

            bool exit = false;
            while (!exit)
            {
                // Toon het menu voor het beheren van het account
                Console.WriteLine("\nKies een optie:");
                Console.WriteLine("1. Kind toevoegen");
                Console.WriteLine("2. Partner toevoegen");
                Console.WriteLine("3. Spaarrekening beheren");
                Console.WriteLine("4. Alles tonen");
                Console.WriteLine("5. Sluiten");

                // Verwerk de keuze van de gebruiker
                switch (Console.ReadLine())
                {
                    case "1":
                        AddKind(account);  // Voeg een kind toe
                        break;
                    case "2":
                        AddPartner(account);  // Voeg een partner toe
                        break;
                    case "3":
                        ManageSavings(account);  // Beheer de spaarrekening
                        break;
                    case "4":
                        account.DisplayAll();  // Toon alle accountinformatie
                        break;
                    case "5":
                        exit = true;  // Stop het programma
                        break;
                    default:
                        Console.WriteLine("Ongeldige keuze. Probeer opnieuw.");  // Foutmelding bij een ongeldige keuze
                        break;
                }
            }

            // Afsluitbericht
            Console.WriteLine("Bedankt voor het gebruik van de applicatie. Tot ziens!");
        }

        // Genereer een wachtwoord op basis van de naam en geboortedatum van de gebruiker
        static string GeneratePassword(string name, DateTime birthDate)
        {
            return name.Substring(0, 3) + birthDate.ToString("ddMMyyyy");
        }

        // Voeg een kind toe aan het account
        static void AddKind(Account account)
        {
            bool addMore = true;
            while (addMore)
            {
                // Vraag de naam en leeftijd van het kind
                Console.Write("Voer de naam van het kind in: ");
                string kindName = Console.ReadLine();

                Console.Write("Voer de leeftijd van het kind in: ");
                int kindAge = 0;
                bool validAge = false;

                // Valideer de leeftijd van het kind
                while (!validAge)
                {
                    if (int.TryParse(Console.ReadLine(), out kindAge) && kindAge > 0 && kindAge <= account.GetAge() - 18)
                    {
                        validAge = true;
                    }
                    else
                    {
                        Console.WriteLine("De leeftijd van het kind moet minimaal 18 jaar jonger zijn dan uw eigen leeftijd. Probeer opnieuw.");
                    }
                }

                // Voeg het kind toe aan het account
                account.AddKind(kindName, kindAge);

                // Vraag de gebruiker of hij nog een kind wil toevoegen
                Console.Write("Wilt u nog een kind toevoegen? (ja/nee): ");
                addMore = Console.ReadLine().ToLower() == "ja";
            }
        }

        // Voeg een partner toe aan het account
        static void AddPartner(Account account)
        {
            // Vraag de naam, leeftijd en beroep van de partner
            Console.Write("Voer de naam van de partner in: ");
            string partnerName = Console.ReadLine();

            Console.Write("Voer de leeftijd van de partner in: ");
            int partnerAge;
            while (!int.TryParse(Console.ReadLine(), out partnerAge) || Math.Abs(partnerAge - account.GetAge()) > 10)
            {
                Console.WriteLine("De leeftijd van de partner mag maximaal 10 jaar verschillen van uw eigen leeftijd. Probeer opnieuw.");
            }

            Console.Write("Voer het beroep van de partner in: ");
            string partnerJob = Console.ReadLine();

            // Voeg de partner toe aan het account
            account.AddPartner(partnerName, partnerAge, partnerJob);
        }

        // Beheer de spaarrekening van het account
        static void ManageSavings(Account account)
        {
            bool manageMore = true;
            while (manageMore)
            {
                // Toon het menu voor het beheren van de spaarrekening
                Console.WriteLine("\nKies een optie:");
                Console.WriteLine("1. Geld toevoegen");
                Console.WriteLine("2. Geld afhalen");
                Console.WriteLine("3. Saldo bekijken");
                Console.WriteLine("4. Terug");

                // Verwerk de keuze van de gebruiker
                switch (Console.ReadLine())
                {
                    case "1":
                        // Voeg geld toe aan de spaarrekening
                        Console.Write("Voer het bedrag in om toe te voegen: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal amountToAdd))
                        {
                            account.AddToSavings(amountToAdd);
                        }
                        else
                        {
                            Console.WriteLine("Ongeldig bedrag.");
                        }
                        break;
                    case "2":
                        // Haal geld af van de spaarrekening
                        Console.Write("Voer het bedrag in om af te halen: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal amountToWithdraw))
                        {
                            account.WithdrawFromSavings(amountToWithdraw);
                        }
                        else
                        {
                            Console.WriteLine("Ongeldig bedrag.");
                        }
                        break;
                    case "3":
                        // Toon het saldo van de spaarrekening
                        Console.WriteLine($"Huidig saldo: {account.GetSavingsBalance():F2}"); // Weergeven met 2 decimalen
                        break;
                    case "4":
                        // Terug naar het vorige menu
                        manageMore = false;
                        break;
                    default:
                        Console.WriteLine("Ongeldige keuze. Probeer opnieuw.");  // Foutmelding bij een ongeldige keuze
                        break;
                }
            }
        }
    }

    // De Account klasse voor het beheren van accountinformatie
    class Account
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        private List<Kind> Kinderen { get; set; } = new List<Kind>();
        private Partner Partner { get; set; }
        private decimal SavingsBalance { get; set; } = 0;

        // Constructor voor het aanmaken van een account
        public Account(string name, DateTime birthDate)
        {
            Name = name;
            BirthDate = birthDate;
        }

        // Bereken de leeftijd van de account houder
        public int GetAge()
        {
            return DateTime.Now.Year - BirthDate.Year;
        }

        // Voeg een kind toe aan de account
        public void AddKind(string name, int age)
        {
            DateTime birthDate = DateTime.Now.AddYears(-age);
            Kinderen.Add(new Kind { Name = name, BirthDate = birthDate });
            Console.WriteLine("Kind toegevoegd.");
        }

        // Voeg een partner toe aan de account
        public void AddPartner(string name, int age, string job)
        {
            DateTime birthDate = DateTime.Now.AddYears(-age);
            Partner = new Partner { Name = name, BirthDate = birthDate, Job = job };
            Console.WriteLine("Partner toegevoegd.");
        }

        // Voeg geld toe aan de spaarrekening
        public void AddToSavings(decimal amount)
        {
            SavingsBalance += amount;
            Console.WriteLine($"{amount:F2} toegevoegd aan spaarrekening."); // Bedrag wordt met 2 decimalen weergegeven
        }

        // Haal geld af van de spaarrekening
        public void WithdrawFromSavings(decimal amount)
        {
            if (amount <= SavingsBalance)
            {
                SavingsBalance -= amount;
                Console.WriteLine($"{amount:F2} afgenomen van spaarrekening.");
            }
            else
            {
                Console.WriteLine("Onvoldoende saldo.");
            }
        }

        // Verkrijg het saldo van de spaarrekening
        public decimal GetSavingsBalance()
        {
            return SavingsBalance;
        }

        // Toon alle accountinformatie, inclusief kinderen, partner en spaarrekening
        public void DisplayAll()
        {
            Console.WriteLine("\nAccountinformatie:");
            Console.WriteLine($"Naam: {Name}");
            Console.WriteLine($"Geboortedatum: {BirthDate:dd-MM-yyyy}");

            Console.WriteLine("\nKinderen:");
            foreach (var kind in Kinderen)
            {
                Console.WriteLine($"- {kind.Name}, geboren op {kind.BirthDate:dd-MM-yyyy}");
            }

            if (Partner != null)
            {
                Console.WriteLine("\nPartner:");
                Console.WriteLine($"Naam: {Partner.Name}, Geboortedatum: {Partner.BirthDate:dd-MM-yyyy}, Beroep: {Partner.Job}");
            }

            Console.WriteLine($"\nSpaarrekening saldo: {SavingsBalance:F2}"); // Spaarrekening wordt met 2 decimalen weergegeven
        }
    }

    // De Kind klasse voor het opslaan van gegevens over een kind
    class Kind
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }

    // De Partner klasse voor het opslaan van gegevens over een partner
    class Partner
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Job { get; set; }
    }
}
