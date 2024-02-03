<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

class Programa
{
    static string rutaAgenda = "agenda.txt";
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
                    GuardarAgendaEnFitxer();
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

    static void CarregarAgendaDesDeFitxer()
    {
        if (File.Exists(rutaAgenda))
        {
            try
            {
                var lineas = File.ReadLines(rutaAgenda)
                    .Select(linea => linea.Split(';'))
                    .Select(datos => new Usuari
                    {
                        Nom = datos[0],
                        Cognom = datos[1],
                        DNI = datos[2],
                        Telefon = datos[3],
                        DataNaixament = DateTime.ParseExact(datos[4], "d", CultureInfo.InvariantCulture),
                        CorreuElectronic = datos[5]
                    })
                    .ToList();

                agenda.AddRange(lineas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la lectura del fitxer: {ex.Message}");
            }
        }
        else
        {
            File.Create(rutaAgenda).Close();
        }
    }


    static void DonarAltaUsuari()
    {
        Console.WriteLine("==== Alta d'Usuari ====");
        Usuari nouUsuari = new Usuari();

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
        nouUsuari.DataNaixament = ValidarDataNeixament(dataNaix);

        Console.Write("Correu electrònic: ");
        nouUsuari.CorreuElectronic = ValidarCorreu(Console.ReadLine());

        agenda.Add(nouUsuari);
        Console.WriteLine("\nUsuari afegit amb èxit:");
        MostrarDadesUsuari(nouUsuari);
    }

    static string ValidarDNI(string dni)
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

    static void MostrarDadesUsuari(Usuari nouUsuari)
    {
        Console.WriteLine($"Nom: {nouUsuari.Nom}");
        Console.WriteLine($"Cognom: {nouUsuari.Cognom}");
        Console.WriteLine($"DNI: {nouUsuari.DNI}");
        Console.WriteLine($"Telèfon: {nouUsuari.Telefon}");
        Console.WriteLine($"Data de naixament: {nouUsuari.DataNaixament.ToShortDateString()}");
        Console.WriteLine($"Correu electrònic: {nouUsuari.CorreuElectronic}");
    }

    static void RecuperarUsuari()
    {
        Console.WriteLine("==== Recuperació d'un Usuari ====");
        Console.Write("Entra el DNI de l'usuari a recuperar: ");
        string dniRecuperar = Console.ReadLine();

        Usuari usuariRecuperat = agenda.FirstOrDefault(usuari => usuari.DNI == dniRecuperar);

        if (usuariRecuperat != null)
        {
            Console.WriteLine("\nUsuari Trobat:");
            MostrarDadesUsuari(usuariRecuperat);
        }
        else
        {
            Console.WriteLine("\nUsuari no trobat.");
        }
    }

    static void ModificarUsuari()
    {
        Console.WriteLine("==== Modificació d'un Usuari ====");
        Console.Write("Entra el DNI de l'usuari a modificar: ");
        string dniModificar = Console.ReadLine();

        Usuari usuariModificar = agenda.FirstOrDefault(usuari => usuari.DNI == dniModificar);

        if (usuariModificar != null)
        {
            Console.WriteLine("\nDades actuals de l'usuari:");
            MostrarDadesUsuari(usuariModificar);

            Console.WriteLine("\nIntrodueix les noves dades:");
            Console.Write("Nom (Enter per mantenir actual): ");
            string nouNom = Console.ReadLine();
            usuariModificar.Nom = (nouNom != "") ? nouNom : usuariModificar.Nom;

            Console.Write("Cognom (Enter per mantenir actual): ");
            string nouCognom = Console.ReadLine();
            usuariModificar.Cognom = (nouCognom != "") ? nouCognom : usuariModificar.Cognom;

            Console.Write("Telèfon (Enter per mantenir actual): ");
            string nouTelefon = Console.ReadLine();
            usuariModificar.Telefon = (nouTelefon != "") ? nouTelefon : usuariModificar.Telefon;

            Console.Write("Data de naixament (Enter per mantenir actual): ");
            DateTime novaDataNaixament;
            if (DateTime.TryParse(Console.ReadLine(), out novaDataNaixament))
            {
                usuariModificar.DataNaixament = (novaDataNaixament > DateTime.Now) ? usuariModificar.DataNaixament : novaDataNaixament;
            }

            Console.Write("Correu electrònic (Enter per mantenir actual): ");
            string nouCorreu = Console.ReadLine();
            usuariModificar.CorreuElectronic = (nouCorreu != "") ? nouCorreu : usuariModificar.CorreuElectronic;

            Console.WriteLine("\nUsuari modificat amb èxit:");
            MostrarDadesUsuari(usuariModificar);
        }
        else
        {
            Console.WriteLine("\nUsuari no trobat.");
        }
    }

    static void EliminarUsuari()
    {
        Console.WriteLine("==== Esborrar un Usuari ====");
        Console.Write("Entra el DNI de l'usuari a esborrar: ");
        string dniEliminar = Console.ReadLine();

        Usuari usuariEliminar = agenda.FirstOrDefault(usuari => usuari.DNI == dniEliminar);

        if (usuariEliminar != null)
        {
            agenda.Remove(usuariEliminar);
            Console.WriteLine("\nUsuari eliminat amb èxit.");
        }
        else
        {
            Console.WriteLine("\nUsuari no trobat.");
        }
    }

    static void MostrarAgenda()
    {
        Console.WriteLine("==== Llista d'Usuaris ====");
        if (agenda.Any())
        {
            foreach (Usuari usuari in agenda)
            {
                MostrarDadesUsuari(usuari);
                Console.WriteLine("----------------------------");
            }
        }
        else
        {
            Console.WriteLine("L'agenda està buida.");
        }
    }

    static void OrdenarAgenda()
    {
        Console.WriteLine("==== Ordenar l'Agenda ====");
        if (agenda.Any())
        {
            agenda = agenda.OrderBy(usuari => usuari.Cognom).ToList();
            Console.WriteLine("Agenda ordenada per cognoms.");
        }
        else
        {
            Console.WriteLine("L'agenda està buida.");
        }
    }

    static void GuardarAgendaEnFitxer()
    {
        using (StreamWriter writer = new StreamWriter(rutaAgenda, false))
        {
            foreach (Usuari usuari in agenda)
            {
                string linia = $"{usuari.Nom};{usuari.Cognom};{usuari.DNI};{usuari.Telefon};{usuari.DataNaixament.ToShortDateString()};{usuari.CorreuElectronic}";
                writer.WriteLine(linia);
            }
        }
        Console.WriteLine("Agenda guardada amb èxit.");
    }
}


public class Usuari
{
    public string Nom { get; set; }
    public string Cognom { get; set; }
    public string DNI { get; set; }
    public string Telefon { get; set; }
    public DateTime DataNaixament { get; set; }
    public string CorreuElectronic { get; set; }
}
=======
﻿using System;
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

>>>>>>> c9378241da3551cc38960d366311bacc7337da12
