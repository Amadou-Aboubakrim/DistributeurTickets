
using System;
using System.Collections.Generic;
using System.IO;

public class GestionCompteurs
{
    private readonly string cheminFichier;

    private readonly Dictionary<char, int> compteurs = new()
    {
        { 'V', 0 },
        { 'R', 0 },
        { 'I', 0 }
    };

    public string CheminFichier
    {
        get { return cheminFichier; }
    }

    public GestionCompteurs()
    {
        cheminFichier = Path.Combine(
            Path.GetTempPath(),
            "fnuméro.txt"
        );

        ChargerOuCreerFichier();
    }

    // Lire les compteurs ou créer le fichier s'il n'existe pas
    private void ChargerOuCreerFichier()
    {
        if (File.Exists(cheminFichier))
        {
            string[] lignes = File.ReadAllLines(cheminFichier);

            foreach (string ligne in lignes)
            {
                string[] elements = ligne.Split('=');

                if (elements.Length == 2 &&
                    elements[0].Length == 1 &&
                    compteurs.ContainsKey(elements[0][0]) &&
                    int.TryParse(elements[1], out int valeur) &&
                    valeur >= 0)
                {
                    compteurs[elements[0][0]] = valeur;
                }
            }
        }

        // Créer le fichier ou actualiser son contenu
        Sauvegarder();
    }

    // Attribuer le prochain numéro d'un type d'opération
    public int AttribuerNumero(char type)
    {
        if (!compteurs.ContainsKey(type))
        {
            throw new ArgumentException("Type d'opération invalide.");
        }

        compteurs[type]++;

        Sauvegarder();

        return compteurs[type];
    }

    // Obtenir le dernier numéro attribué
    public int ObtenirDernierNumero(char type)
    {
        return compteurs[type];
    }

    // Sauvegarder les trois compteurs
    private void Sauvegarder()
    {
        string[] lignes =
        {
            $"V={compteurs['V']}",
            $"R={compteurs['R']}",
            $"I={compteurs['I']}"
        };

        File.WriteAllLines(cheminFichier, lignes);
    }
}
