using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.Text;

namespace CodegenTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fm = new List<FilterModel>()
            {
                new FilterModel("Price", ">", "100"),
            };



            var fs = new FilterService();
            Func<Product, bool> func = fs.Filter<Product>(fm).Result;

            List<Product> Products = new List<Product>()
            {
                new Product("Some product 1", 100),
                new Product("Some product 2" , 200),
                new Product("Some product 3", 50)
            };

            var p = Products.First(func);
        }
    }

    public class FilterModel
    {
        public string PropertyName { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }

        public FilterModel(string propertyName, string @operator, string value)
        {
            PropertyName = propertyName;
            Operator = @operator;
            Value = value;
        }

        public override string ToString()
        {
            string propertyType = typeof(Product).GetProperty(PropertyName)?.PropertyType.Name;

            switch (propertyType)
            {
                case "Decimal":
                    return $"x.{PropertyName} {Operator} System.Convert.To{propertyType}({Value})";
                default:
                    return $"x.{PropertyName} {Operator} \"{Value}\"";
            }
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    public class FilterService
    {
        public async Task<Func<T, bool>> Filter<T>(List<FilterModel> filterModels)
        {
            ScriptOptions options = ScriptOptions.Default.AddReferences(typeof(T).Assembly, Assembly.GetAssembly(typeof(Convert)));
            //options.AddImports("System");

            StringBuilder sb = new StringBuilder("x => ");
            string code = sb.Append(string.Join("&&", filterModels)).ToString();

            return await CSharpScript.EvaluateAsync<Func<T, bool>>(code, options);
        }
    }

}
