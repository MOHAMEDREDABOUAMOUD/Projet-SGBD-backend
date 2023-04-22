using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projet_SGBD_backend.enums;
using Projet_SGBD_backend.models;
using Projet_SGBD_backend.services.interfaces;

namespace Projet_SGBD_backend.services
{
    public class StructTable : IStructTable
    {
        string name;
        List<Field> fields;

        public StructTable()
        {
            name = "???";
            fields = new List<Field>();
        }

        public StructTable(string name)
        {
            Name = name;
            fields = new List<Field>();
        }

        public string Name { get => name; set => name = value; }
        public List<Field> Fields { get => fields; set => fields = value; }

        public bool add(string name, TypeField type, Constraint constr)
        {
            fields.Add(new Field(name, type, constr));
            return true;
        }

        public void Describe()
        {
            Console.Write("name : " + Name + "\n" + "fields : |");
            foreach (Field field in fields)
            {
                Console.Write(field + "|");
            }
            Console.WriteLine();
        }

        public bool modify(string name, TypeField NewType, Constraint NewConstr = Constraint.NotNull, string NewName = "")
        {
            Field f = rechercher(name);
            if (NewName != "") f.Name = NewName;
            f.Type = NewType;
            f.Constr = NewConstr;
            return true;
        }

        public Field rechercher(string name)
        {
            foreach (Field field in fields)
            {
                if (field.Name == name)
                {
                    return field;
                }
            }
            return null;
        }

        public bool remove(string name)
        {
            Field f = rechercher(name);
            fields.Remove(f);
            return true;
        }
        public Field getField(int index)
        {
            return fields[index];
        }
    }
}
