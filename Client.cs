
using System.Collections.Generic;

public class Client
{
    // Informations personnelles du client
    public string NumeroCompte { get; }
    public string Nom { get; }
    public string Prenom { get; }

    // Tickets associés aux types d'opérations
    public Dictionary<string, string> Tickets { get; } = new();

    public Client(string numeroCompte, string nom, string prenom)
    {
        NumeroCompte = numeroCompte;
        Nom = nom;
        Prenom = prenom;
    }
}
