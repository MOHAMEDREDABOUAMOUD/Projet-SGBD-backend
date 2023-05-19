using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projet_SGBD_backend.enums;

namespace Projet_SGBD_backend.models
{
    [Serializable]
    public class Field
    {
        string name;
        TypeField type;
        Constraint constr;

        public Field()
        {
            name = "????";
            type = new TypeField();
            constr = new Constraint();
        }

        public Field(string name = "default", TypeField type = TypeField.Text, Constraint contraint = Constraint.NotNull)
        {
            this.name = name;
            this.type = type;
            constr = contraint;
        }

        public string Name { get => name; set => name = value; }
        public TypeField Type { get => type; set => type = value; }
        public Constraint Constr { get => constr; set => constr = value; }
        public override string ToString()
        {
            return "name : " + name + "\t type : " + type + "\t constraint : " + constr;
        }
    }
}
