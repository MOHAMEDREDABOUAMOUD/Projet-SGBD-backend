using System.Xml.Linq;
using Projet_SGBD_backend.enums;
using Projet_SGBD_backend.models;

namespace Projet_SGBD_backend.services.interfaces
{
    public interface IStructTable
    {
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
        public Field rechercher(string name);
        public bool add(string name, TypeField type, Constraint constr);
        public bool remove(string name);
        public bool modify(string name, TypeField NewType, Constraint NewConstr, string NewName = "");
        public void Describe();
    }
}