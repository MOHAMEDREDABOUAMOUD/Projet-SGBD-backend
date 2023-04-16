using System.Xml.Linq;

namespace Projet_SGBD_backend.services.interfaces
{
    public interface IStructDatabase
    {
        public List<StructTable> Tables { get; set; }
        public string Name { get; set; }
        public StructTable rechercher(string name);
        public bool add(StructTable table);
        public bool remove(string tabmeName);
        public bool modify(string tableName, string NewTableName);

        public void ShowTables();
        public void save();
        public void load();
    }
}