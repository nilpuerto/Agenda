using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

class Program
{
    static string agendaFilePath = "agenda.txt";
    static List<Usuario> agenda = new List<Usuario>();

    static void Main()
    {
        Console.Title = "Gestió de l'Agenda";
        CargarAgendaDesdeArchivo();

        while (true)
        {
            MostrarMenu();
            char opcio = Console.ReadKey().KeyChar;
            Console.Clear();

            switch (opcio)
            {
                case '1':
                    DonarAltaUsuari();
                    break;
                case '2':
                    RecuperarUsuari();
                    break;
                case '3':
                    ModificarUsuari();
                    break;
                case '4':
                    EliminarUsuari();
                    break;
                case '5':
                    MostrarAgenda();
                    break;
                case '6':
                    OrdenarAgenda();
                    break;
                case '7':
                case 'S':
                case 's':
                    GuardarAgendaEnArxiu();
                    return;
                default:
                    Console.WriteLine("Opció no vàlida. Si us plau, trieu una opció del menú.");
                    break;
            }

            Thread.Sleep(3000);
            Console.Clear();
        }
    }

    static void MostrarMenu()
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;

        string[] menuInterfaz = {
            " ╔═════════════════════════════════════════════════════════╗ ",
            " ║                      Gestió Agenda                      ║ ",
            " ║=========================================================║ ",
            " ║                    1) Donar d'Alta                      ║ ",
            " ║                    2) Recuperació d'un Usuari           ║ ",
            " ║                    3) Modificació d'un Usuari           ║ ",
            " ║                    4) Esborrar un Usuari                ║ ",
            " ║                    5) Mostrar l'Agenda                  ║ ",
            " ║                    6) Ordenar                           ║ ",
            " ║                    7) Sortir                            ║ ",
            " ║=========================================================║ ",
            " ║   - Escull una opció -                                  ║ ",
            " ║=========================================================║ ",
            " ║                                             Nil i Alex  ║ ",
            " ║_________________________________________________________║ "
        };

        int centreY = (Console.WindowHeight - menuInterfaz.Length) / 2;

        foreach (string linea in menuInterfaz)
        {
            int centreX = (Console.WindowWidth - linea.Length) / 2;
            Console.SetCursorPosition(centreX, centreY);
            Console.WriteLine(linea);
            centreY++;
        }

        Console.Write("Escull una opció: ");
    }

    static void DonarAltaUsuari()
    {
        Console.WriteLine("==== Alta d'Usuari ====");
        Usuario nouUsuari = new Usuario();

        Console.Write("Nom: ");
        nouUsuari.Nom = Console.ReadLine();

        Console.Write("Cognom: ");
        nouUsuari.Cognom = Console.ReadLine();

        Console.Write("DNI: ");
        nouUsuari.DNI = ValidarDNI(Console.ReadLine());

        Console.Write("Telèfon: ");
        nouUsuari.Telefon = ValidarTelefon(Console.ReadLine());

        Console.Write("Data de naixement (dd/MM/yyyy): ");
        nouUsuari.DataNaixement = ValidarDataNaixement(Console.ReadLine());

        Console.Write("Correu electrònic: ");
        nouUsuari.CorreuElectronic = ValidarCorreuElectronic(Console.ReadLine());

        agenda.Add(nouUsuari);
        Console.WriteLine("\nUsuari afegit amb èxit:");
        MostrarDadesUsuari(nouUsuari);
    }


}
