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

    private static void RecuperarUsuari()
    {
        throw new NotImplementedException();
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

        Console.Write("Entra una data de naixament: ");
        DateTime dataNaix = Convert.ToDateTime(Console.ReadLine());

        Console.Write("Correu electrònic: ");
        nouUsuari.CorreuElectronic = ValidarCorreu(Console.ReadLine());

        agenda.Add(nouUsuari);
        Console.WriteLine("\nUsuari afegit amb èxit:");
        MostrarDadesUsuari(nouUsuari);
    }

    private static void MostrarDadesUsuari(Usuario nouUsuari)
    {
        throw new NotImplementedException();
    }

    private static object ValidarDNI(string? v)
    {
        throw new NotImplementedException();
    }

    static string ValidarNom(string nom)
    {
        nom = Regex.Replace(nom, @"[^a-zA-Z]", "");
        nom = char.ToUpper(nom[0]) + nom.Substring(1).ToLower();
        return nom;
    }
    static string ValidarCognom(string cognom)
    {
        cognom = Regex.Replace(cognom, @"[^a-zA-Z]", "");
        cognom = char.ToUpper(cognom[0]) + cognom.Substring(1).ToLower();
        return cognom;
    }
    static string ValidarDni(string dni)
    {
        Console.Clear();
        bool dniValid = false;
        while (!dniValid)
        {
            var dniRegex = new Regex(@"^[1-9]{8}[A-Z]{1}$");
            if (!dniRegex.IsMatch(dni))
            {
                Console.Write("Error. Entra un altre DNI: ");
                dni = Console.ReadLine();
            }
            else
            {
                dniValid = true;
            }
        }
        return dni;
    }
    static string ValidarTelefon(string telefon)
    {
        Console.Clear();
        bool telefonValid = false;
        while (!telefonValid)
        {
            var telefonRegex = new Regex(@"^\d{9}$");
            if (!telefonRegex.IsMatch(telefon))
            {
                Console.Write("Error. Entra un altre telefon: ");
                telefon = Console.ReadLine();
            }
            else
                telefonValid = true;
        }
        return telefon;
    }
    static DateTime ValidarDataNeixament(DateTime dataNaix)
    {
        Console.Clear();
        bool dataValida = false;
        while (!dataValida)
        {
            if (dataNaix > DateTime.Now)
            {
                Console.Write("Error. Entra un altre data de naixament: ");
                dataNaix = Convert.ToDateTime(Console.ReadLine());
            }
            else
            {
                dataValida = true;
            }
        }
        return dataNaix;
    }
    static string ValidarCorreu(string correu)
    {
        Console.Clear();
        bool correuValid = false;
        while (!correuValid)
        {
            var correuRegex = new Regex(@"^[a-zA-Z0-9]+@[a-zA-Z]{3,}\.(com|es)$");
            if (!correuRegex.IsMatch(correu))
            {
                Console.Write("Error. Entra un altre correu: ");
                correu = Console.ReadLine();
            }
            else
                correuValid = true;
        }
        return correu;
    }
    static void ObrirFitxer(string nom, string cognom, string dni, string telefon, DateTime dataNaix, string correu)
    {
        StreamWriter sW = new StreamWriter("agenda.txt", true);
        sW.WriteLine($"{nom};{cognom};{dni};{telefon};{dataNaix.ToString("d")};{correu}\r");
        sW.Close();

    }
    static void RecuperarUsuari(string? nomUsuari)
    {
        char trobarUsuari = 'S';
        while (trobarUsuari != 'N' && trobarUsuari != 'n')
        {
            Console.Clear();
            Console.Write("Quin usuari vols trobar? ");
            var linea = File.ReadLines("agenda.txt")
                .Select(linea => linea.Split(';')[0]).ToList();

            bool trobat = linea.Contains(nomUsuari);

            if (trobat)
            {
                Console.WriteLine($"Usuari: {nomUsuari} trobat.");
                trobarUsuari = 'N';
            }
            else
            {
                Console.Write("Error. Vols buscar un altre usuari? (S/N)");
                trobarUsuari = Convert.ToChar(Console.ReadLine());
            }
        }
        
    }
    static void EliminarUsuari()
    {
        char EliminarUsuari = 'S';
        string Nom, usuari;
        while (EliminarUsuari != 'n' && EliminarUsuari != 'N')
        {
            Console.Write("Quin usuari vols eliminar? ");
            Nom = Console.ReadLine();
            var lineas = File.ReadAllLines("agenda.txt").ToList();
            lineas.RemoveAll(linea => linea.Split(';')[0].Equals(Nom));
            File.WriteAllLines("agenda.txt", lineas.Where(linea => !string.IsNullOrWhiteSpace(linea)));

            Console.WriteLine($"Usuari {Nom} eliminat");
            Console.Write("Vols eliminar un altre usuari? (S/N)");
            EliminarUsuari = Convert.ToChar(Console.ReadLine());
        }
    }
    static void MostrarAgenda()
    {
        var lineas = File.ReadLines("agenda.txt")
            .Select(linea => linea.Split(';'))
            .Where(dades => dades.Length >= 4)
            .Select(dades => new
            {
                Nom = dades[0],
                Telefon = dades[3]
            })
            .OrderBy(usuari => usuari.Nom)
            .ToList();

        for (int i = 0; i < lineas.Count; i++)
        {
            Console.WriteLine($"Nom: {lineas[i].Nom}, Telefon: {lineas[i].Telefon}");
        }
    }
    static void OrdenarAgenda()
    {
        var lineas = File.ReadLines("agenda.txt")
            .Select(linea => new
            {
                Datos = linea.Split(';'),
                Nombre = linea.Split(';')[0]
            })
            .OrderBy(usuari => usuari.Nombre)
            .Select(usuari => string.Join(";", usuari.Datos))
            .ToList();

        File.WriteAllLines("agenda.txt", lineas);
        Console.WriteLine("La agenda esta ordenada");
    }
}

