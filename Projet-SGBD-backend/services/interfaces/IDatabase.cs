using System.Xml.Linq;
using Projet_SGBD_backend.services;

namespace Projet_SGBD_backend.services.interfaces
{
    public interface IDatabase
    {
        public string Name { get; set; }
        public List<Table> Tables { get; set; }
        public Table rechercher(string name);
        public bool add(Table table);
        public bool remove(string tabmeName);
        public bool modify(string tableName, string NewTableName);

        public void ShowTables();
        public void save();
        public void load();
        public bool executeQuery(string query);

        public bool executeUpdate(string query);
    }
}