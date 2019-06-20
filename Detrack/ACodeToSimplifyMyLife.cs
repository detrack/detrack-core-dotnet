using System;
using Detrack.DetrackCore;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Detrack
{
    public class ACodeToSimplifyMyLife
    {
        public static void Mnotain()
        {
            Type type = typeof(Item);
            PropertyInfo[] properties = type.GetProperties();

            string pattern = $"({{ get {{ return \\w+; }}) (set {{ \\w+ = \\w+; }} }})";
            string input = "{ get { return _job_sequence; } set { _job_sequence = value; } }\n";


            foreach (PropertyInfo property in properties)
            {
                string propertys = property.ToString();
                propertys = propertys.Split(' ')[1];
                string snakeproperty = Regex.Replace(propertys, "([a-z])([A-Z])", "$1_$2").ToLower();
                Item myjob = new Item();
                string types = property.PropertyType.ToString();

                string result = Regex.Replace(input, pattern, $"    public {types} {propertys}\n    {{\n            get\n            {{\n                return _{snakeproperty};\n            }}\n            set\n            {{\n                if (_{snakeproperty} != value)\n                {{\n                    OnPropertyChanged();\n                }}\n                _{snakeproperty} = value;\n            }}\n    }}");
                Console.WriteLine(result);
            }
        }
    }
}
