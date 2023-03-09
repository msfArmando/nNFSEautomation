using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nNFSEautomation
{
    public class FormatDate
    {
        public string FormatMonth(string month)
        {
            if (month == "jan")
                int.Parse(month = "01");
            else if (month == "fev")
                int.Parse(month = "02");
            else if (month == "mar")
                int.Parse(month = "03");
            else if (month == "abr")
                int.Parse(month = "04");
            else if (month == "mai")
                int.Parse(month = "05");
            else if (month == "jun")
                int.Parse(month = "06");
            else if (month == "jul")
                int.Parse(month = "07");
            else if (month == "ago")
                int.Parse(month = "08");
            else if (month == "set")
                int.Parse(month = "09");
            else if (month == "out")
                int.Parse(month = "10");
            else if (month == "nov")
                int.Parse(month = "11");
            else if (month == "dez")
                int.Parse(month = "12");
            else
                throw new Exception("Mês inválido!");
            return month;
        }

        public string FormatDay(string day)
        {
            if (int.Parse(day) < 10)
                return $"0{day}";
            else if (int.Parse(day) >= 10 && int.Parse(day) <= 31)
                return $"{day}";
            else
                throw new Exception("Dia inválido.");
        }
    }
}
