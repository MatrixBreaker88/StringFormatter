using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

/* Notes: Jacor T. Finlayson 09/03/23
    -All the models created are within this one file for easy reasibility 
    -The output doesn't contain property names across the board so I won't include serialization*/

namespace RedRover
{
    // enum for different outputs
    enum RedRoverFormat
    {
        Scarlet = 1,
        Burgundy = 2
    }

    // class for types property
    class RoverType
    {
        public string Id;
        public string Name;
        public string TypeName;
        public string CustomFieldName;
        public List<string> CustomFields;

        public RoverType(string[] roverType, ref int startIndex)
        {
            //parse the type object
            if(roverType != null && roverType.Length > 0)
            {
                TypeName = roverType[startIndex].Split('(')[0];
                Id = roverType[startIndex++].Split('(')[1];
                Name = roverType[startIndex++];
                CustomFieldName = roverType[startIndex].Split('(')[0];

                CustomFields = new List<string>();
                CustomFields.Add(roverType[startIndex++].Split('(')[1]);
                CustomFields.Add(roverType[startIndex++]);
                CustomFields.Add(roverType[startIndex++].Replace(")",""));
            }
        }
    }

    // class for all properties 
    class RoverModel
    {
        public string Id;
        public string ExternalId;
        public string Name;
        public string Email;
        public RoverType Type;

        public RoverModel(string roverModelText)
        {
            //remove the parenthesis
            var input = roverModelText.Substring(1, roverModelText.Length - 2).Split(", ");
            var inputIndex = 0;

            //assign properties
            Id = input[inputIndex++];
            Name = input[inputIndex++];
            Email = input[inputIndex++];
            Type = new RoverType(input, ref inputIndex);
            ExternalId = input[inputIndex];
        }

        public void DisplayModel(RedRoverFormat redFormat = RedRoverFormat.Scarlet)
        {
            StringBuilder sb = new StringBuilder();
            var prefix = "- ";

            // display the format based on the display type
            switch(redFormat)
            {
                case RedRoverFormat.Burgundy:
                    sb.AppendLine($"{prefix}{Email}");
                    sb.AppendLine($"{prefix}{ExternalId}");
                    sb.AppendLine($"{prefix}{Id}");
                    sb.AppendLine($"{prefix}{Name}");
                    sb.AppendLine($"{prefix}{Type.TypeName}");

                    sb.AppendLine($"\t{prefix}{Type.CustomFieldName}");
                    foreach(var field in Type.CustomFields){
                        sb.AppendLine($"\t\t{prefix}{field}");
                    }
                    sb.AppendLine($"\t{prefix}{Type.Id}");
                    sb.AppendLine($"\t{prefix}{Type.Name}");
                    break;

                default:
                    sb.AppendLine($"{prefix}{Id}");
                    sb.AppendLine($"{prefix}{Name}");
                    sb.AppendLine($"{prefix}{Email}");

                    sb.AppendLine($"{prefix}{Type.TypeName}");
                    sb.AppendLine($"\t{prefix}{Type.Id}");
                    sb.AppendLine($"\t{prefix}{Type.Name}");

                    sb.AppendLine($"\t{prefix}{Type.CustomFieldName}");
                    foreach(var field in Type.CustomFields){
                        sb.AppendLine($"\t\t{prefix}{field}");
                    }
                    sb.AppendLine($"{prefix}{ExternalId}");
                    break;
            }

            Console.WriteLine(sb.ToString());
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            //convert the input text (not using args for simplicity)
            RoverModel rover9000 = new RoverModel("(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)"); 

            //display the output
            rover9000.DisplayModel(RedRoverFormat.Scarlet);
            Console.WriteLine("\nAnd also to this output:\n");
            rover9000.DisplayModel(RedRoverFormat.Burgundy);
        }
    }
}
