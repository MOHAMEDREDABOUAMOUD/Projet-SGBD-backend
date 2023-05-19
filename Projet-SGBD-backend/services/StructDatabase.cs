using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Projet_SGBD_backend.services.interfaces;

namespace Projet_SGBD_backend.services
{
    [Serializable]
    public class StructDatabase : IStructDatabase
    {
        string name;
        List<StructTable> tables;

        public StructDatabase(string name)
        {
            this.name = name;
            tables = new List<StructTable>();
        }

        public List<StructTable> Tables { get => tables; set => tables = value; }
        public string Name { get => name; set => name = value; }

        public bool add(StructTable table)
        {
            Tables.Add(table);
            return true;
        }

        public void load()
        {
            FileStream fs2 = new FileStream("./" + Name + ".json", FileMode.Open);
            StructDatabase backup = JsonSerializer.Deserialize(fs2, typeof(StructDatabase)) as StructDatabase;
            fs2.Close();
            Name = backup.Name;
            tables = backup.Tables;
        }

        public bool modify(string tableName, string NewTableName)
        {
            StructTable t = rechercher(tableName);
            t.Name = NewTableName;
            return true;
        }

        public StructTable rechercher(string name)
        {
            foreach (StructTable table in Tables)
            {
                if (table.Name == name) return table;
            }
            return null;
        }

        public bool remove(string tabmeName)
        {
            StructTable t = rechercher(tabmeName);
            Tables.Remove(t);
            return true;
        }

        public void save()
        {
            FileStream fs = new FileStream("./" + Name + ".json", FileMode.Create);
            JsonSerializer.Serialize(fs, this);//une methode static du class jsonSerializer
            fs.Close();
        }

        public void ShowTables()
        {
            Console.Write("name : " + Name + "\n tables : |");
            foreach (StructTable table in Tables)
            {
                table.Describe();
            }
        }
    }
}
