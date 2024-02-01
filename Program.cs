using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

class Programa
{
    static string rutaFitxerAgenda = "agenda.txt";
    static List<Usuari> agenda = new List<Usuari>();

    static void Main()
    {
        Console.Title = "Gestió de l'Agenda";
        CarregarAgendaDesDeFitxer();

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
                    DesarAgendaEnFitxer();
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
        Usuari nouUsuari = new Usuari();

        Console.Write("Nom: ");
        nouUsuari.Nom = Nom(Console.ReadLine());

        Console.Write("Cognom: ");
        nouUsuari.Cognom = Cognom(Console.ReadLine());

        Console.Write("DNI: ");
        nouUsuari.DNI = DNI(Console.ReadLine());

        Console.Write("Telèfon: ");
        nouUsuari.Telefon = Telef(Console.ReadLine());

        Console.Write("Data de naixement (dd/MM/yyyy): ");
        nouUsuari.DataNaixement = DataNaix(Convert.ToDateTime(Console.ReadLine()));

        Console.Write("Correu electrònic: ");
        nouUsuari.CorreuElectronic = Correu(Console.ReadLine());

        agenda.Add(nouUsuari);
        MostrarDadesUsuari(nouUsuari);
        Fitxer(nouUsuari.Nom, nouUsuari.Cognom, nouUsuari.DNI, nouUsuari.Telefon, nouUsuari.DataNaixement, nouUsuari.CorreuElectronic);
        DonarAltaSegons();
    }

    static string Nom(string nom)
    {
        nom = Regex.Replace(nom, @"[^a-zA-Z]", "");
        nom = char.ToUpper(nom[0]) + nom.Substring(1).ToLower();
        return nom;
    }
    static string Cognom(string cognom)
    {
        cognom = Regex.Replace(cognom, @"[^a-zA-Z]", "");
        cognom = char.ToUpper(cognom[0]) + cognom.Substring(1).ToLower();
        return cognom;
    }
    static string DNI(string dni)
    {
        Console.Clear();
        bool dniValid = false;
        while (!dniValid)
        {
            var dniRegex = new Regex(@"^\d{8}[A-Z]$");
            if (!dniRegex.IsMatch(dni))
            {
                Console.Write("DNI no valid, entra un altre: ");
                dni = Console.ReadLine();
            }
            else
            {
                dniValid = true;
            }
        }
        return dni;
    }
    static string Telef(string telefon)
    {
        Console.Clear();
        bool telefonValid = false;
        while (!telefonValid)
        {
            var telefonRegex = new Regex(@"^\d{9}$");
            if (!telefonRegex.IsMatch(telefon))
            {
                Console.Write("Telefon no valid, entra una altre: ");
                telefon = Console.ReadLine();
            }
            else
                telefonValid = true;
        }
        return telefon;
    }
    static DateTime DataNaix(DateTime dataNaix)
    {
        Console.Clear();
        bool dataValida = false;
        while (!dataValida)
        {
            if (dataNaix > DateTime.Now)
            {
                Console.Write("Data de naixament no valida, entra un altre: ");
                dataNaix = Convert.ToDateTime(Console.ReadLine());
            }
            else
            {
                dataValida = true;
            }
        }
        return dataNaix;
    }
    static string Correu(string correu)
    {
        Console.Clear();
        bool correuValid = false;
        while (!correuValid)
        {
            var correuRegex = new Regex(@"^[a-zA-Z0-9]+@[a-zA-Z]{3,}\.(com|es)$");
            if (!correuRegex.IsMatch(correu))
            {
                Console.Write("Correu no valid, entra un altre: ");
                correu = Console.ReadLine();
            }
            else
                correuValid = true;
        }
        return correu;
    }
    static void Fitxer(string nom, string cognom, string dni, string telefon, DateTime dataNaix, string correu)
    {
        StreamWriter sW = new StreamWriter("agenda.txt", true);
        sW.WriteLine($"{nom};{cognom};{dni};{telefon};{dataNaix.ToString("d")};{correu}\r");
        sW.Close();

    }

    static void Recuperar()
    {
        char trobarUsuari = 'S';
        while (trobarUsuari != 'N' && trobarUsuari != 'n')
        {
            Console.Clear();
            Console.Write("Quin usuari vols? ");
            string nomUsuari = Console.ReadLine();

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
                Console.Write("Usuari no trobat. Vols trobar un altre usuari? (S/N)");
                trobarUsuari = Convert.ToChar(Console.ReadLine());
            }
        }
        RecuperarSegons();
    }
    static string RecuperarUsuari(string nomUsuari)
    {
        char trobarUsuari = 'S';
        bool trobat;
        while (trobarUsuari != 'N' && trobarUsuari != 'n')
        {
            var linea = File.ReadLines("agenda.txt")
                .Select(linea => linea.Split(';')[0]).ToList();

            trobat = linea.Contains(nomUsuari);

            if (trobat)
            {
                trobarUsuari = 'N';
            }
            else
            {
                Console.Write("Usuari no trobat. Vols trobar un altre usuari? (S/N)");
                trobarUsuari = Convert.ToChar(Console.ReadLine());
            }
        }
        return nomUsuari;
    }
    static void Modificar()
    {
        char Finalitzar = 'S';
        Console.Write("Quin usuari vols trobar? ");
        string nomUsuari = Console.ReadLine();
        while (Finalitzar != 'N' && Finalitzar != 'n')
        {
            string usuari = RecuperarUsuari(nomUsuari);
            Console.Write("Quina dada vols modificar? ");
            string dada = Console.ReadLine();

            Console.Write("Introdueix el nou valor: ");
            string nouValor = Console.ReadLine();

            var dadesUsuari = usuari.Split(';');

            switch (dada.ToLower())
            {
                case "nom":
                    dadesUsuari[0] = nouValor;
                    break;
                case "cognom":
                    dadesUsuari[1] = nouValor;
                    break;
                case "dni":
                    dadesUsuari[2] = nouValor;
                    break;
                case "telefon":
                    dadesUsuari[3] = nouValor;
                    break;
                case "datanaixament":
                    dadesUsuari[4] = nouValor;
                    break;
                case "correu":
                    dadesUsuari[5] = nouValor;
                    break;
                default:
                    Console.WriteLine("Dada no existent.");
                    return;
            }
            usuari = string.Join(";", dadesUsuari);
            var lineas = File.ReadAllLines("agenda.txt").ToList();
            lineas[lineas.IndexOf(usuari)] = usuari;
            File.WriteAllLines("agenda.txt", lineas);
            Console.WriteLine($"Vols modifcar alguna dada de {nomUsuari}? (S/N)");
            Finalitzar = Convert.ToChar(Console.ReadLine());
        }
    }

    static void Eliminar()
    {
        char tornarEliminarUsuari = 'S';
        string nomUsuari, usuario;
        while (tornarEliminarUsuari != 'n' && tornarEliminarUsuari != 'N')
        {
            Console.Write("Quin usuari vols eliminar? ");
            nomUsuari = Console.ReadLine();

            usuario = RecuperarUsuari(nomUsuari);

            var lineas = File.ReadAllLines("agenda.txt").ToList();
            lineas.RemoveAll(linea => linea.Split(';')[0].Equals(nomUsuari));
            File.WriteAllLines("agenda.txt", lineas.Where(linea => !string.IsNullOrWhiteSpace(linea)));

            Console.WriteLine($"Usuari {nomUsuari} eliminat");
            Console.Write("Vols eliminar algun altre usuari? (S/N)");
            tornarEliminarUsuari = Convert.ToChar(Console.ReadLine());
        }

    }

    static void Mostrar()
    {
        var lineas = File.ReadLines("agenda.txt")
            .Select(linea => linea.Split(';'))
            .Where(datos => datos.Length >= 4)
            .Select(datos => new
            {
                Nombre = datos[0],
                Telefono = datos[3]
            })
            .OrderBy(usuario => usuario.Nombre)
            .ToList();

        for (int i = 0; i < lineas.Count; i++)
        {
            Console.WriteLine($"Nombre: {lineas[i].Nombre}, Teléfono: {lineas[i].Telefono}");
        }
        RecuperarSegons();
    }
    static void Ordenar()
    {
        var lineas = File.ReadLines("agenda.txt")
            .Select(linea => new
            {
                Datos = linea.Split(';'),
                Nombre = linea.Split(';')[0]
            })
            .OrderBy(usuario => usuario.Nombre)
            .Select(usuario => string.Join(";", usuario.Datos))
            .ToList();

        File.WriteAllLines("agenda.txt", lineas);
        Console.WriteLine("La agenda a estat ordenada correctament.");
        RecuperarSegons();
    }
    static void RecuperarSegons()
    {
        int i = 5;
        while (i != 0)
        {
            Console.Write("\r");
            Console.Write($"Tornant al menu: {i}'s");
            Thread.Sleep(1000);
            i--;
        }
    }
    static void DonarAltaSegons()
    {
        int i = 3;
        while (i != 0)
        {
            Console.Write("\r");
            Console.Write($"Tornant al menu: {i}'s");
            Thread.Sleep(1000);
            i--;
        }
    }

}


using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                      Gestió Agenda                      ║");
            Console.WriteLine("║=========================================================║");
            Console.WriteLine("║                    1) Donar d'Alta                      ║");
            Console.WriteLine("║                    2) Recuperació d'un Usuari           ║");
            Console.WriteLine("║                    3) Modificació d'un Usuari           ║");
            Console.WriteLine("║                    4) Esborrar un Usuari                ║");
            Console.WriteLine("║                    5) Mostrar l'Agenda                  ║");
            Console.WriteLine("║                    6) Ordenar                           ║");
            Console.WriteLine("║                    7) Sortir                            ║");
            Console.WriteLine("║=========================================================║");
            Console.WriteLine("║   - Escull una opció -                                  ║");
            Console.WriteLine("║=========================================================║");
            Console.WriteLine("║                                             Nil i Alex  ║");
            Console.WriteLine("║_________________________________________________________║");

            char option = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (char.ToUpper(option))
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
                case 'Q':
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opció no vàlida. Torna a intentar.");
                    break;
            }

            Console.WriteLine("Prem enter per continuar...");
            Console.ReadLine();
        }
    }

    static void DonarAltaUsuari()
    {
        Console.Write("Entra un nom: ");
        string nom = Console.ReadLine();
        Console.Write("Entra un cognom: ");
        string cognom = Console.ReadLine();
        Console.Write("Entra un DNI: ");
        string dni = Console.ReadLine();
        Console.Write("Entra un telf: ");
        string telefon = Console.ReadLine();
        Console.Write("Entra una data de naixament: ");
        DateTime dataNaix = Convert.ToDateTime(Console.ReadLine());
        Console.Write("Entra un email:");
        string correuElectronic = Console.ReadLine();
        Fitxer(Nom(nom), Cognom(cognom), DNI(dni), Telef(telefon), DataNaix(dataNaix), Correu(correuElectronic));
        DonarAltaSegons();

    }
}
static string Nom(string nom)
{
    nom = Regex.Replace(nom, @"[^a-zA-Z]", "");
    nom = char.ToUpper(nom[0]) + nom.Substring(1).ToLower();
    return nom;
}
static string Cognom(string cognom)
{
    cognom = Regex.Replace(cognom, @"[^a-zA-Z]", "");
    cognom = char.ToUpper(cognom[0]) + cognom.Substring(1).ToLower();
    return cognom;
}
static string DNI(string dni)
{
    Console.Clear();
    bool dniValid = false;
    while (!dniValid)
    {
        var dniRegex = new Regex(@"^\d{8}[A-Z]$");
        if (!dniRegex.IsMatch(dni))
        {
            Console.Write("DNI no valid, entra un altre: ");
            dni = Console.ReadLine();
        }
        else
        {
            dniValid = true;
        }
    }
    return dni;
}
static string Telef(string telefon)
{
    Console.Clear();
    bool telefonValid = false;
    while (!telefonValid)
    {
        var telefonRegex = new Regex(@"^\d{9}$");
        if (!telefonRegex.IsMatch(telefon))
        {
            Console.Write("Telefon no valid, entra una altre: ");
            telefon = Console.ReadLine();
        }
        else
            telefonValid = true;
    }
    return telefon;
}
static DateTime DataNaix(DateTime dataNaix)
{
    Console.Clear();
    bool dataValida = false;
    while (!dataValida)
    {
        if (dataNaix > DateTime.Now)
        {
            Console.Write("Data de naixament no valida, entra un altre: ");
            dataNaix = Convert.ToDateTime(Console.ReadLine());
        }
        else
        {
            dataValida = true;
        }
    }
    return dataNaix;
}
static string Correu(string correu)
{
    Console.Clear();
    bool correuValid = false;
    while (!correuValid)
    {
        var correuRegex = new Regex(@"^[a-zA-Z0-9]+@[a-zA-Z]{3,}\.(com|es)$");
        if (!correuRegex.IsMatch(correu))
        {
            Console.Write("Correu no valid, entra un altre: ");
            correu = Console.ReadLine();
        }
        else
            correuValid = true;
    }
    return correu;
}
static void Fitxer(string nom, string cognom, string dni, string telefon, DateTime dataNaix, string correu)
{
    StreamWriter sW = new StreamWriter("agenda.txt", true);
    sW.WriteLine($"{nom};{cognom};{dni};{telefon};{dataNaix.ToString("d")};{correu}\r");
    sW.Close();

}

static void Recuperar()
{
    char trobarUsuari = 'S';
    while (trobarUsuari != 'N' && trobarUsuari != 'n')
    {
        Console.Clear();
        Console.Write("Quin usuari vols? ");
        string nomUsuari = Console.ReadLine();

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
            Console.Write("Usuari no trobat. Vols trobar un altre usuari? (S/N)");
            trobarUsuari = Convert.ToChar(Console.ReadLine());
        }
    }
    RecuperarSegons();
}
static void RecuperarUsuari()
    {
    {
        char trobarUsuari = 'S';
        bool trobat;
        while (trobarUsuari != 'N' && trobarUsuari != 'n')
        {
            var linea = File.ReadLines("agenda.txt")
                .Select(linea => linea.Split(';')[0]).ToList();

            trobat = linea.Contains(nomUsuari);

            if (trobat)
            {
                trobarUsuari = 'N';
            }
            else
            {
                Console.Write("Usuari no trobat. Vols trobar un altre usuari? (S/N)");
                trobarUsuari = Convert.ToChar(Console.ReadLine());
            }
        }
        return nomUsuari;
    }
}
    

    static void ModificarUsuari()
    {
    {
        char Finalitzar = 'S';
        Console.Write("Quin usuari vols trobar? ");
        string nomUsuari = Console.ReadLine();
        while (Finalitzar != 'N' && Finalitzar != 'n')
        {
            string usuari = RecuperarUsuari(nomUsuari);
            Console.Write("Quina dada vols modificar? ");
            string dada = Console.ReadLine();

            Console.Write("Introdueix el nou valor: ");
            string nouValor = Console.ReadLine();

            var dadesUsuari = usuari.Split(';');

            switch (dada.ToLower())
            {
                case "nom":
                    dadesUsuari[0] = nouValor;
                    break;
                case "cognom":
                    dadesUsuari[1] = nouValor;
                    break;
                case "dni":
                    dadesUsuari[2] = nouValor;
                    break;
                case "telefon":
                    dadesUsuari[3] = nouValor;
                    break;
                case "datanaixament":
                    dadesUsuari[4] = nouValor;
                    break;
                case "correu":
                    dadesUsuari[5] = nouValor;
                    break;
                default:
                    Console.WriteLine("Dada no existent.");
                    return;
            }
            usuari = string.Join(";", dadesUsuari);
            var lineas = File.ReadAllLines("agenda.txt").ToList();
            lineas[lineas.IndexOf(usuari)] = usuari;
            File.WriteAllLines("agenda.txt", lineas);
            Console.WriteLine($"Vols modifcar alguna dada de {nomUsuari}? (S/N)");
            Finalitzar = Convert.ToChar(Console.ReadLine());
        }
    }
}

    static void EliminarUsuari()
    {
    {
        char tornarEliminarUsuari = 'S';
        string nomUsuari, usuario;
        while (tornarEliminarUsuari != 'n' && tornarEliminarUsuari != 'N')
        {
            Console.Write("Quin usuari vols eliminar? ");
            nomUsuari = Console.ReadLine();

            usuario = RecuperarUsuari(nomUsuari);

            var lineas = File.ReadAllLines("agenda.txt").ToList();
            lineas.RemoveAll(linea => linea.Split(';')[0].Equals(nomUsuari));
            File.WriteAllLines("agenda.txt", lineas.Where(linea => !string.IsNullOrWhiteSpace(linea)));

            Console.WriteLine($"Usuari {nomUsuari} eliminat");
            Console.Write("Vols eliminar algun altre usuari? (S/N)");
            tornarEliminarUsuari = Convert.ToChar(Console.ReadLine());
        }

    }
}

    static void MostrarAgenda()
    {
    var lineas = File.ReadLines("agenda.txt")
              .Select(linea => linea.Split(';'))
              .Where(datos => datos.Length >= 4)
              .Select(datos => new
              {
                  Nombre = datos[0],
                  Telefono = datos[3]
              })
              .OrderBy(usuario => usuario.Nombre)
              .ToList();

    for (int i = 0; i < lineas.Count; i++)
    {
        Console.WriteLine($"Nombre: {lineas[i].Nombre}, Teléfono: {lineas[i].Telefono}");
    }
    RecuperarSegons();
}
    

    static void OrdenarAgenda()
    {
    {
        var lineas = File.ReadLines("agenda.txt")
            .Select(linea => new
            {
                Datos = linea.Split(';'),
                Nombre = linea.Split(';')[0]
            })
            .OrderBy(usuario => usuario.Nombre)
            .Select(usuario => string.Join(";", usuario.Datos))
            .ToList();

        File.WriteAllLines("agenda.txt", lineas);
        Console.WriteLine("La agenda a estat ordenada correctament.");
        RecuperarSegons();
    }

    static void RecuperarSegons()
    {
        int i = 5;
        while (i != 0)
        {
            Console.Write("\r");
            Console.Write($"Tornant al menu: {i}'s");
            Thread.Sleep(1000);
            i--;
        }
    }
    static void DonarAltaSegons()
    {
        int i = 3;
        while (i != 0)
        {
            Console.Write("\r");
            Console.Write($"Tornant al menu: {i}'s");
            Thread.Sleep(1000);
            i--;
        }
    }
}

