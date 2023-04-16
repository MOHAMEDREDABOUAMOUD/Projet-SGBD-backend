using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Projet_SGBD_backend.services.interfaces;

namespace Projet_SGBD_backend.services
{
    public class Database : IDatabase
    {
        string name;
        List<Table> tables;
        public Database(string name)
        {
            this.name = name;
            tables = new List<Table>();
        }

        public string Name { get => name; set => name = value; }
        public List<Table> Tables { get => tables; set => tables = value; }

        public bool add(Table table)
        {
            tables.Add(table);
            return true;
        }

        public bool executeQuery(string query)
        {
            query = query.ToLower();
            string[] elems = query.Split(' ');
            if (elems[0] == "select")
            {
                string[] columns = elems[1].Split(',');//test if it returns one if there is juste one
                if (columns.Length == 1)
                {
                    if (columns[0] == "*")
                    {
                        if (elems[2] == "from")
                        {
                            string table = elems[3];
                            if (elems.Length>4)
                            {
                                if (elems[4] == "where")
                                {

                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                rechercher(table).print("*");
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (elems[2] == "from")
                        {
                            string table = elems[3];
                            rechercher(table).print(columns[0]);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if(columns.Length>1)
                {
                    if (elems[2] == "from")
                    {
                        string table = elems[3];
                        rechercher(table).print(columns);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        public bool executeUpdate(string query)
        {
            query = query.ToLower();
            string[] elems = query.Split(' ');
            if (elems[0] == "insert")
            {
                //
            }
            else if (elems[0] == "update")
            {
                //
            }
            else if (elems[0] == "delete")
            {
                //
            }
            else
            {
                return false;
            }
            return false;
        }

        public void load()
        {
            FileStream fs2 = new FileStream("./" + Name + ".json", FileMode.Open);
            Database backup = JsonSerializer.Deserialize(fs2, typeof(Database)) as Database;
            fs2.Close();
            Name = backup.Name;
            tables = backup.Tables;
        }

        public bool modify(string tableName, string NewTableName)
        {
            Table t = rechercher(tableName);
            t.StructTable.Name = NewTableName;
            return true;
        }

        public Table rechercher(string name)
        {
            foreach (Table table in Tables)
            {
                if (table.StructTable.Name == name) return table;
            }
            return null;
        }

        public bool remove(string tabmeName)
        {
            Table t = rechercher(tabmeName);
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
            foreach (Table table in tables)
            {
                table.StructTable.Describe();
            }
        }
    }
}
