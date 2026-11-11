
using System;
using System.Collections.Generic;

class Program
{
    // Modèle unique du message d'attente
    private const string ModeleMessage =
        "Votre numéro est {0}, et il y a {1} personnes qui attendent avant vous.";

    static void Main()
    {
        GestionSaisies saisies = new GestionSaisies();
        GestionCompteurs gestionCompteurs = new GestionCompteurs();

        // Liste des clients de cette exécution
        List<Client> clients = new List<Client>();

        Console.WriteLine("==================================");
        Console.WriteLine(" DISTRIBUTEUR AUTOMATIQUE - BANQUE");
        Console.WriteLine("==================================");
        Console.WriteLine($"Fichier des compteurs : {gestionCompteurs.CheminFichier}");

        bool finProgramme = false;

        // Boucle principale : arrivée de nouveaux clients
        while (!finProgramme)
        {
            Console.WriteLine("\n========== NOUVEAU CLIENT ==========");

            // Choix de la première opération
            EtatSaisie etat = saisies.LireTypeOperation(out char typeOperation);

            if (etat == EtatSaisie.Fin)
            {
                finProgramme = true;
            }
            else if (etat == EtatSaisie.Stop)
            {
                Console.WriteLine("Parcours du client annulé. Aucun ticket attribué.");
            }
            else
            {
                // Récupération des informations personnelles
                etat = LireInformationsClient(
                    saisies,
                    out string numeroCompte,
                    out string nom,
                    out string prenom
                );

                if (etat == EtatSaisie.Fin)
                {
                    finProgramme = true;
                }
                else if (etat == EtatSaisie.Stop)
                {
                    Console.WriteLine("Saisie du client annulée. Aucun ticket attribué.");
                }
                else
                {
                    // Création du client
                    Client client = new Client(numeroCompte, nom, prenom);

                    clients.Add(client);

                
                    while (true)
                    {
                        AttribuerOuReutiliserTicket(
                            client,
                            typeOperation,
                            gestionCompteurs
                        );

                      
                        etat = saisies.LireOuiNon(
                            "\nSouhaitez-vous effectuer une autre opération ? (O/N) : ",
                            out bool autreOperation
                        );

                        if (etat == EtatSaisie.Fin)
                        {
                            finProgramme = true;
                            break;
                        }

                        if (etat == EtatSaisie.Stop)
                        {
                            Console.WriteLine(
                                "Parcours du client arrêté. Les tickets déjà attribués sont conservés."
                            );
                            break;
                        }

                        if (!autreOperation)
                        {
                            break;
                        }

                        
                        etat = saisies.LireTypeOperation(out typeOperation);

                        if (etat == EtatSaisie.Fin)
                        {
                            finProgramme = true;
                            break;
                        }

                        if (etat == EtatSaisie.Stop)
                        {
                            Console.WriteLine(
                                "Parcours du client arrêté. Les tickets déjà attribués sont conservés."
                            );
                            break;
                        }
                    }
                }
            }

            if (finProgramme)
            {
                break;
            }

            // Demander si un nouveau client doit être accueilli
            etat = saisies.LireOuiNon(
                "\nVoulez-vous accueillir un autre client ? (O/N) : ",
                out bool autreClient
            );

            if (etat == EtatSaisie.Fin)
            {
                break;
            }

            if (etat == EtatSaisie.Stop)
            {
                Console.WriteLine("Arrêt de l'accueil des nouveaux clients.");
                break;
            }

            if (!autreClient)
            {
                break;
            }
        }

        
        AfficherRecapitulatif(clients);
    }

    
    static EtatSaisie LireInformationsClient(
        GestionSaisies saisies,
        out string numeroCompte,
        out string nom,
        out string prenom)
    {
        EtatSaisie etat;

        etat = saisies.LireTexteObligatoire(
            "Entrez votre numéro de compte : ",
            out numeroCompte
        );

        if (etat != EtatSaisie.Valide)
        {
            nom = "";
            prenom = "";
            return etat;
        }

        etat = saisies.LireTexteObligatoire(
            "Entrez votre nom : ",
            out nom,
            true
        );

        if (etat != EtatSaisie.Valide)
        {
            prenom = "";
            return etat;
        }

        etat = saisies.LireTexteObligatoire(
            "Entrez votre prénom : ",
            out prenom,
            true
        );

        return etat;
    }

    // Attribuer un nouveau ticket ou réutiliser celui du client
    static void AttribuerOuReutiliserTicket(
        Client client,
        char type,
        GestionCompteurs gestionCompteurs)
    {
        string cle = type.ToString();

        string numeroTicket;
        int rang;

        // Vérifier si le client possède déjà un ticket de ce type
        if (client.Tickets.TryGetValue(cle, out string? ticketExistant)&& ticketExistant != null)
        {
            numeroTicket = ticketExistant;

            // Récupérer le rang du ticket existant
            string[] parties = numeroTicket.Split('-');
            rang = int.Parse(parties[1]);

            Console.WriteLine($"\nVous possédez déjà le ticket {numeroTicket}.");
        }
        else
        {
            // Générer un nouveau numéro
            rang = gestionCompteurs.AttribuerNumero(type);

            numeroTicket = $"{type}-{rang}";

            // Mémoriser le ticket pour ce client
            client.Tickets.Add(cle, numeroTicket);
        }

        // Calcul du nombre de personnes devant le client
        int personnesEnAttente = rang - 1;

        // Affichage du message avec le modèle unique
        Console.WriteLine(
            string.Format(
                ModeleMessage,
                numeroTicket,
                personnesEnAttente
            )
        );
    }

    // Afficher tous les clients et leurs tickets
    static void AfficherRecapitulatif(List<Client> clients)
    {
        Console.WriteLine("\n==================================");
        Console.WriteLine("       LISTE FINALE DES CLIENTS");
        Console.WriteLine("==================================");

        if (clients.Count == 0)
        {
            Console.WriteLine("Aucun client n'a été enregistré.");
            return;
        }

        int numeroClient = 1;

        foreach (Client client in clients)
        {
            Console.WriteLine($"\nClient {numeroClient++}");
            Console.WriteLine($"Numéro de compte : {client.NumeroCompte}");
            Console.WriteLine($"Nom              : {client.Nom}");
            Console.WriteLine($"Prénom           : {client.Prenom}");

            Console.WriteLine("Tickets attribués :");

            foreach (KeyValuePair<string, string> ticket in client.Tickets)
            {
                Console.WriteLine($"- {ticket.Value}");
            }
        }

        Console.WriteLine("\n==================================");
        Console.WriteLine("Fin du programme.");
        Console.WriteLine("==================================");
        Console.WriteLine("STOP : annuler le client en cours.");
        Console.WriteLine("FIN  : quitter le programme.");
        Console.WriteLine("Ces commandes sont disponibles à chaque saisie.\n");
    }
}
