using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AC2TractamentDeFitxers
{
    public class Gestor
    {
        public int Any {  get; set; }
        public int CodiComarca { get; set; }
        public string Comarca { get; set; }
        public int Poblacio { get; set; }
        public  int DomesticXarxa { get; set; }
        public int ActiEconom { get; set; }
        public int Total {  get; set; }
        public double ConsDomCap {  get; set; }

    }

    public static List<Gestor> CargarDatos(string filePath)
    {
        var datos = new List<Gestor>();

        using (var reader = new StreamReader(filePath))
        {
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                datos.Add(new Gestor
                {
                    Any = int.Parse(values[0]),
                    CodiComarca = int.Parse(values[1]),
                    Comarca = values[2]

                });


            }
        }

    }
}
