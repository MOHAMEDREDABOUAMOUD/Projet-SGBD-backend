using Projet_SGBD_backend.models;

namespace Projet_SGBD_backend.services.interfaces
{
    public interface ITable
    {
        public List<Row> Rows { get; set; }
        public StructTable StructTable { get; set; }
        public Row rechercher(int colonne, string value);
        public bool add(Row row);
        public bool remove(int colonne, string value);
        public bool modify(int colonne, string value, string newValue);
    }
}