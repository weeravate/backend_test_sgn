using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Formats.Asn1;
using System.Text.Json;
using test_sgn.Models;

namespace test_sgn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopulateController : ControllerBase
    {
        [HttpGet("getchart")]
        public List<Population> Get()
        {
            string fileName = "population-and-demography.csv";
            string path = Path.Combine(Environment.CurrentDirectory, @"Assets\", fileName);
            List<Population> populations = new List<Population>();
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                bool firstLine = true;
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(new string[] { "," });

                while (!parser.EndOfData)
                {
                    string[] row = parser.ReadFields()!;
                    string[] data = row;
                    if (firstLine)
                    {
                        firstLine = false;
                        continue;
                    }
                    int rowIndex = populations.FindIndex(o => o.Year == Convert.ToInt32(data[1]));
                    if (rowIndex > -1)
                    {
                        populations[rowIndex].CountryList.Add(new Country()
                        {
                            Name = data[0],
                            Populate = data[2],
                            Continent = ""
                        });
                    }
                    else
                    {
                        populations.Add(new Population()
                        {
                            Year = Convert.ToInt32(data[1]),
                            CountryList = new List<Country>()
                            {
                                new Country()
                                {
                                    Name = data[0],
                                    Populate = data[2],
                                    Continent = ""
                                }
                            }
                        });
                    }
                }
            }
            return populations;
        }
    }
}
