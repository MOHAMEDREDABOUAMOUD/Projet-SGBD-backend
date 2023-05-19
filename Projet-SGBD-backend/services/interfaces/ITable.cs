using Projet_SGBD_backend.models;

namespace Projet_SGBD_backend.services.interfaces
{
    public interface ITable
    {
        public List<Row> Rows { get; set; }
        public StructTable StructTable { get; set; }
        public bool insert(Row row);
        public bool delete(Dictionary<string, string> args = null, Dictionary<string, string> opp = null, string oppLog = "");
        public bool update(Dictionary<string, string> newValues = null, Dictionary<string, string> conditions = null, Dictionary<string, string> opp = null, string oppLog = "");
        public List<List<string>> select(List<string> values, Dictionary<string, string> conditions, Dictionary<string, string> conditionsOpp, string oppLog = "");
        public Row rechercher(int colonne, string value);
        public bool remove(int colonne, string value);
        public bool modify(int colonne, string value, string newValue);
    }
}