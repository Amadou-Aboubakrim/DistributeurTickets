
using System;

public enum EtatSaisie
{
    Valide,
    Stop,
    Fin
}

public class GestionSaisies
{
    // Vérifier qu'un nom contient uniquement des lettres,
    // espaces, apostrophes ou traits d'union.
    private bool NomValide(string texte)
    {
        bool contientLettre = false;

        foreach (char caractere in texte)
        {
            if (char.IsLetter(caractere))
            {
                contientLettre = true;
            }
            else if (char.IsWhiteSpace(caractere) ||
                     caractere == '-' ||
                     caractere == '\'' ||
                     caractere == '’')
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return contientLettre;
    }

    // Lecture d'un champ obligatoire
    public EtatSaisie LireTexteObligatoire(
        string message,
        out string valeur,
        bool estNomOuPrenom = false)
    {
        while (true)
        {
            Console.Write(message);

            string? saisie = Console.ReadLine();

            if (saisie == null)
            {
                valeur = "";
                return EtatSaisie.Fin;
            }

            saisie = saisie.Trim();

            if (saisie.Equals("STOP", StringComparison.OrdinalIgnoreCase))
            {
                valeur = "";
                return EtatSaisie.Stop;
            }

            if (saisie.Equals("FIN", StringComparison.OrdinalIgnoreCase))
            {
                valeur = "";
                return EtatSaisie.Fin;
            }

            if (string.IsNullOrWhiteSpace(saisie))
            {
                Console.WriteLine("Ce champ est obligatoire. Recommencez.");
                continue;
            }

            if (estNomOuPrenom && !NomValide(saisie))
            {
                Console.WriteLine(
                    "Saisie invalide : utilisez uniquement des lettres."
                );
                continue;
            }

            valeur = saisie;
            return EtatSaisie.Valide;
        }
    }

    // Choix du type d'opération
    public EtatSaisie LireTypeOperation(out char type)
    {
        while (true)
        {
            Console.WriteLine("\nChoisissez votre opération :");
            Console.WriteLine("V - Versement");
            Console.WriteLine("R - Retrait");
            Console.WriteLine("I - Informations");
            Console.Write("Votre choix : ");

            string? saisie = Console.ReadLine();

            if (saisie == null)
            {
                type = '\0';
                return EtatSaisie.Fin;
            }

            saisie = saisie.Trim().ToUpperInvariant();

            if (saisie == "STOP")
            {
                type = '\0';
                return EtatSaisie.Stop;
            }

            if (saisie == "FIN")
            {
                type = '\0';
                return EtatSaisie.Fin;
            }

            switch (saisie)
            {
                case "V":
                case "VERSEMENT":
                    type = 'V';
                    return EtatSaisie.Valide;

                case "R":
                case "RETRAIT":
                    type = 'R';
                    return EtatSaisie.Valide;

                case "I":
                case "INFORMATIONS":
                    type = 'I';
                    return EtatSaisie.Valide;

                default:
                    Console.WriteLine("Choix invalide. Entrez V, R ou I.");
                    break;
            }
        }
    }

    // Vérification des réponses Oui / Non
    public EtatSaisie LireOuiNon(string message, out bool reponse)
    {
        while (true)
        {
            Console.Write(message);

            string? saisie = Console.ReadLine();

            if (saisie == null)
            {
                reponse = false;
                return EtatSaisie.Fin;
            }

            saisie = saisie.Trim().ToUpperInvariant();

            if (saisie == "STOP")
            {
                reponse = false;
                return EtatSaisie.Stop;
            }

            if (saisie == "FIN")
            {
                reponse = false;
                return EtatSaisie.Fin;
            }

            if (saisie == "OUI" || saisie == "O")
            {
                reponse = true;
                return EtatSaisie.Valide;
            }

            if (saisie == "NON" || saisie == "N")
            {
                reponse = false;
                return EtatSaisie.Valide;
            }

            Console.WriteLine("Réponse invalide. Entrez Oui ou Non.");
        }
    }
}
