using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace AC2TractamentDeFitxers
{
    public class Program
    {
        public static void Main()
        {
            string filePath = @"../../../ConsAigua.csv";
            string xmlFilePath = @"../../../ConsAigua.xml";

            //convertToXML(filePath, xmlFilePath);
            var records = ReadXML(xmlFilePath);
            //ComarquesPoblacio(records);
            CalcularMitjanaConsumDomesticPerComarca(records);
            //ComarquesConsumDomesticMesAlt(records);

        }

        public static void convertToXML(string filePath, string xmlFilePath) // Convertir a XML
        {
            try
            {
                var reader = new StreamReader(filePath);
                var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<Gestor>().ToList();
                var xml = new XmlSerializer(typeof(List<Gestor>));
                var writer = new StreamWriter(xmlFilePath);
                xml.Serialize(writer, records);
                writer.Close();
                Console.WriteLine("Fitxer XML creat correctament");

            }catch (Exception e)
            {
                Console.WriteLine("Error al convertir a XML: " + e.Message);
            }
        }

        public static List<Gestor> ReadXML(string xmlFilePath) //Llegir XML
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(List<Gestor>));
                using (var reader = new StreamReader(xmlFilePath))
                {
                    return (List<Gestor>)xmlSerializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("S'ha produït un error en llegir el fitxer XML: " + ex.Message);
                return new List<Gestor>();
            }
        }

        public static void ComarquesPoblacio(List<Gestor> records) //Comarques amb població superior a 200.000 habitants
        {
            var comarquesSuperior200K = new List<Gestor>();

            foreach (var gestor in records)
            {
                if (gestor.Poblacio > 200000)
                {
                    comarquesSuperior200K.Add(gestor);
                }
            }
            Console.WriteLine("Comarques amb població superior a 200.000 habitants:");
            foreach (var gestor in comarquesSuperior200K)
            {
                Console.WriteLine($"Comarca: {gestor.Comarca}, Població: {gestor.Poblacio}");
            }

        }


        /*public static void CalcularMitjanaConsumDomesticPerComarca(List<Gestor> records)
        {
            var comarques = new List<Gestor>(); //Llista de comarques
            var comarquesMitjana = new List<Gestor>(); //Llista de comarques amb la mitjana de consum domèstic

            foreach (var gestor in records) //Afegir les comarques a la llista
            {
                if (!comarques.Contains(gestor)) //Si la comarca no està a la llista, l'afegim
                {
                    comarques.Add(gestor);
                }
            }

            foreach (var comarca in comarques) //Per cada comarca, calculem la mitjana de consum domèstic
            {
                double consumTotal = 0; //Consum total de la comarca
                var numRegistres = 0; //Número de registres de la comarca
                foreach (var gestor in records) //Per cada registre, sumem el consum domèstic si la comarca és la mateixa
                {
                    if (gestor.Comarca == comarca.Comarca)
                    {
                        consumTotal += gestor.ConsDomCap;
                        numRegistres++;
                    }
                }
                var mitjana = consumTotal / numRegistres; //Calculem la mitjana
                comarquesMitjana.Add(new Gestor(0, 0, comarca.Comarca, 0, 0, 0, 0, mitjana)); //Afegim la comarca amb la mitjana de consum domèstic a la llista
            }

            Console.WriteLine("Mitjana de consum domèstic per comarca:");
            foreach (var gestor in comarquesMitjana)
            {
                Console.WriteLine($"Comarca: {gestor.Comarca}, Mitjana: {gestor.ConsDomCap}");
            }
        }*/
        

        public static void CalcularMitjanaConsumDomesticPerComarca(List<Gestor> records)
        {
            var comarquesMitjana = records
                .GroupBy(r => r.Comarca) // Agrupar registros por comarca
                .Select(group => new Gestor
                {
                    Comarca = group.Key, // La clave del grupo es el nombre de la comarca
                    ConsDomCap = group.Average(r => r.ConsDomCap) // Calcular la media del consumo doméstico para el grupo
                })
                .ToList();

            Console.WriteLine("Mitjana de consum domèstic per comarca:");
            foreach (var gestor in comarquesMitjana)
            {
                Console.WriteLine($"Comarca: {gestor.Comarca}, Mitjana: {gestor.ConsDomCap}");
            }
        }



        public static void ComarquesConsumDomesticMesAlt(List<Gestor> records)
        {
            var comarques = new List<Gestor>(); //Llista de comarques
            var comarquesConsumMesAlt = new List<Gestor>(); //Llista de comarques amb el consum domèstic més alt

            foreach (var gestor in records) //Afegir les comarques a la llista
            {
                if (!comarques.Contains(gestor)) //Si la comarca no està a la llista, l'afegim
                {
                    comarques.Add(gestor);
                }
            }

            foreach (var comarca in comarques) //Per cada comarca, calculem el consum domèstic més alt
            {
                double consumMesAlt = 0; //Consum domèstic més alt de la comarca
                foreach (var gestor in records) //Per cada registre, comprovem si el consum domèstic és més alt
                {
                    if (gestor.Comarca == comarca.Comarca && gestor.ConsDomCap > consumMesAlt)
                    {
                        consumMesAlt = gestor.ConsDomCap;
                    }
                }
                comarquesConsumMesAlt.Add(new Gestor(0, 0, comarca.Comarca, 0, 0, 0, 0, consumMesAlt)); //Afegim la comarca amb el consum domèstic més alt a la llista
            }

            Console.WriteLine("Comarques amb el consum domèstic per càpital més alt:");
            foreach (var gestor in comarquesConsumMesAlt)
            {
                Console.WriteLine($"Comarca: {gestor.Comarca}, Consum domèstic més alt: {gestor.ConsDomCap}");
            }
        }

    }
}