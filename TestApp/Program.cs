using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using TestApp;

internal class Program
{
    private static void Main(string[] args)
    {
        List<Contract> values = File.ReadAllLines(args[0]).Skip(1)
                                           .Select(v => Contract.FromCsv(v))
                                           .ToList();

        List<Contract> fixedValues = Contract.MergeContracts(values);
        using (var sw = new System.IO.StreamWriter(args[1]))
        {
            sw.WriteLine("Start;Ende;Preis");
            foreach (Contract con in fixedValues)
            {
                sw.WriteLine(con.StartDate + ";" + con.EndDate + ";" + con.Price.ToString().Replace(".", ","));
            }

        }

    }


}