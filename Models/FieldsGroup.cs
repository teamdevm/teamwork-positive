using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Documently.Models
{
    public class FieldsGroup
    {
        public List<string> nameField;//имя поля 
        public string nameGroup;//имя группы, к которой относится тег
        
            
        public FieldsGroup()
        {
            nameField = new List<string>();
            nameGroup = string.Empty;

        }
        public FieldsGroup(string name1, string name2)
        {
            nameField.Add(name1);
            nameGroup = name2;
        }
    }
}
