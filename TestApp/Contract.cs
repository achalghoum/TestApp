using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TestApp
{
    public class Contract
    {
        private DateOnly startDate;
        private DateOnly endDate;
        private Decimal price;
        private int priority;

        public DateOnly StartDate { get => startDate; set => startDate = value; }
        public DateOnly EndDate { get => endDate; set => endDate = value; }
        public int Priority { get => priority; set => priority = value; }
        public decimal Price { get => price; set => price = value; }

        public static List<Contract> MergeContracts(List<Contract> listings)
        {
            //Merges and cleans the input list
            listings.Sort((x, y) => x.StartDate.CompareTo(y.StartDate)!= 0 ? x.StartDate.CompareTo(y.StartDate) : x.priority - y.priority);
            for (int i = 0; i < listings.Count(); i++) { 
                for (int j = i + 1; j<listings.Count() && listings[j].startDate.CompareTo(listings[i].endDate) < 0; j++) {
                    // Iterate over all upcoming contracts that intersect with the current one
                    Contract next = listings[j];
                    Contract current = listings[i];
                    if (next.priority < current.priority)
                    {
                        if (next.endDate.CompareTo(current.endDate) < 0 )
                        {
                            //Remove it if it is completely covered
                            listings.Remove(next);
                        }
                        else
                        {
                            //trim it otherwise
                            next.startDate = current.endDate.AddDays(1);
                            listings[j] = next;
                        }
                    }
                    else {
                        if (next.endDate.CompareTo(current.endDate) < 0)
                        {
                            Contract newListing = new Contract();
                            newListing.price = current.price;
                            newListing.priority = current.priority;
                            Contract updated = new Contract();
                            newListing.startDate = next.endDate.AddDays(1);
                            newListing.endDate = current.endDate;
                            listings.Insert(j + 1, newListing);
                            updated.startDate = current.startDate;
                            updated.endDate = next.startDate.AddDays(-1);
                            updated.price = current.price;
                            updated.priority = current.priority;
                            listings[i] = updated;
                        }
                        else {
                            current.endDate = next.startDate.AddDays(-1);
                            listings[i] = current;
                        }
                    }
                    listings.Sort((x, y) => x.StartDate.CompareTo(y.StartDate) != 0 ? x.StartDate.CompareTo(y.StartDate) : x.priority - y.priority);
                }
            }
            return listings;
        }
        public static Contract FromCsv(string csvLine)
        {
            csvLine = csvLine.Replace(",", ".").Replace(";", ",");
            string[] values = csvLine.Split(',');
            Contract listing = new Contract();
            listing.startDate = DateOnly.FromDateTime(Convert.ToDateTime(values[0]));
            listing.endDate = DateOnly.FromDateTime(Convert.ToDateTime(values[1]));
            listing.price = Convert.ToDecimal(values[2].Replace(".",","));
            listing.priority= Convert.ToInt32(values[3]);
            return listing;
        }


    }


}
