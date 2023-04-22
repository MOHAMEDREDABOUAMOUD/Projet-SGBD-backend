using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Projet_SGBD_backend.models;
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
            string[] elems = query.Split(' ');
            if(elems[0].ToLower() == "select" && elems[2].ToLower() == "from")
            {
                string[] columns = elems[1].Split(',');
                string table = elems[3];
                if (elems.Length > 4)
                {
                    if (elems[4].ToLower() == "where")
                    {
                        Dictionary<string, string> conditions2 = new Dictionary<string, string>();
                        Dictionary<string, string> conditions3 = new Dictionary<string, string>();
                        string opp = "";
                        for (int i = 5; i < elems.Length; i++)
                        {
                            if (elems[i].ToLower() != "and")
                            {
                                if (elems[i].ToLower() != "or")
                                {
                                    if (elems[i].Contains("=="))
                                    {
                                        //conditions2.Add(elems[i].Split("==")[0], elems[i].Split("==")[1].Substring(1, elems[i].Split("==")[1].Length - 2));
                                        conditions2.Add(elems[i].Split("==")[0], elems[i].Split("==")[1]);
                                        conditions3.Add(elems[i].Split("==")[0], "==");
                                    }
                                    else if (elems[i].Contains(">="))
                                    {
                                        conditions2.Add(elems[i].Split(">=")[0], elems[i].Split(">=")[1]);
                                        conditions3.Add(elems[i].Split(">=")[0], ">=");
                                    }
                                    else if (elems[i].Contains("<="))
                                    {
                                        conditions2.Add(elems[i].Split("<=")[0], elems[i].Split("<=")[1]);
                                        conditions3.Add(elems[i].Split("<=")[0], "<=");
                                    }
                                    else if (elems[i].Contains(">"))
                                    {
                                        conditions2.Add(elems[i].Split(">")[0], elems[i].Split(">")[1]);
                                        conditions3.Add(elems[i].Split(">")[0], ">");
                                    }
                                    else if (elems[i].Contains("<"))
                                    {
                                        conditions2.Add(elems[i].Split("<")[0], elems[i].Split("<")[1]);
                                        conditions3.Add(elems[i].Split("<")[0], "<");
                                    }
                                }
                                else
                                {
                                    opp = "or";
                                }
                            }
                            else opp = "and";
                        }
                        /*Console.Write("conditions");
                        foreach (var condition in conditions2)
                        {
                            Console.Write(condition.Key+" "+condition.Value+" | ");
                        }
                        Console.WriteLine();
                        Console.Write("conditions opp");
                        foreach (var condition in conditions3)
                        {
                            Console.Write(condition.Key + " " + condition.Value + " | ");
                        }*/
                        rechercher(table).print(columns.ToList(), conditions2,conditions3, opp);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    rechercher(table).print(columns.ToList(), new Dictionary<string, string>(), new Dictionary<string, string>());
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool executeUpdate(string query)
        {
            //query = query.ToLower();
            string[] elems = query.Split(' ');
            if (elems[0].ToLower() == "insert")
            {
                if (elems[0].ToLower() == "insert" && elems[1].ToLower() == "into")
                {
                    string table=elems[2];
                    if (elems[3].ToLower().Contains("values"))
                    {
                        string values = elems[3].Split('(')[1].Substring(0, elems[3].Split('(')[1].Length-1);
                        Row row = new Row();
                        foreach(string value in values.Split(','))
                        {
                            if(!value.Contains("'")) row.add(value);
                            else
                            {
                                row.add(value.Substring(1,value.Length-2));
                            }
                        }
                        rechercher(table).add(row);
                    }
                }
            }
            else if (elems[0].ToLower() == "update")
            {
                if (elems[0].ToLower() == "update")
                {
                    string table = elems[1];
                    if (elems[2].ToLower() == "set")
                    {
                        int i;
                        string opp = "";
                        Dictionary<string, string> newValues = new Dictionary<string, string>();
                        Dictionary<string, string> conditions = new Dictionary<string, string>();
                        Dictionary<string, string> conditions3 = new Dictionary<string, string>();
                        for (i = 3; i < elems.Length; i++)
                        {
                            if (elems[i].ToLower() == "where") break;
                            else
                            {
                                if (elems[i].Contains("=") && elems[i] != ",")
                                {
                                    newValues.Add(elems[i].Split('=')[0], elems[i].Split('=')[1].Substring(1, elems[i].Split("=")[1].Length - 2));
                                }
                            }
                        }
                        if (elems[i].ToLower() == "where")
                        {
                            for (int j = i+1; j < elems.Length; j++)
                            {
                                if(elems[j].ToLower() != "and")
                                {
                                    if (elems[j].ToLower() != "or")
                                    {
                                        if (elems[j].Contains("=="))
                                        {
                                            conditions.Add(elems[j].Split("==")[0], elems[j].Split("==")[1]);
                                            conditions3.Add(elems[j].Split("==")[0], "==");
                                        }
                                        else if (elems[j].Contains(">="))
                                        {
                                            conditions.Add(elems[j].Split(">=")[0], elems[j].Split(">=")[1]);
                                            conditions3.Add(elems[j].Split(">=")[0], ">=");
                                        }
                                        else if (elems[j].Contains("<="))
                                        {
                                            conditions.Add(elems[j].Split("<=")[0], elems[j].Split("<=")[1]);
                                            conditions3.Add(elems[j].Split("<=")[0], "<=");
                                        }
                                        else if (elems[j].Contains(">"))
                                        {
                                            conditions.Add(elems[j].Split(">")[0], elems[j].Split(">")[1]);
                                            conditions3.Add(elems[j].Split(">")[0], ">");
                                        }
                                        else if (elems[j].Contains("<"))
                                        {
                                            conditions.Add(elems[j].Split("<")[0], elems[j].Split("<")[1]);
                                            conditions3.Add(elems[j].Split("<")[0], "<");
                                        }
                                    }
                                    else
                                    {
                                        opp = "or";
                                    }
                                }
                                else
                                {
                                    opp = "and";
                                }
                            }
                        }
                        rechercher(table).update(newValues,conditions,conditions3, opp);
                    }
                }
            }
            else if (elems[0].ToLower() == "delete")
            {
                if (elems[0].ToLower()=="delete" && elems[1].ToLower() == "from")
                {
                    string table=elems[2];
                    if (elems.Length > 3)
                    {
                        if (elems[3].ToLower() == "where")
                        {
                            Dictionary<string, string> conditions2 = new Dictionary<string, string>();
                            Dictionary<string, string> conditions3 = new Dictionary<string, string>();
                            string opp = "";
                            for (int i = 4; i < elems.Length; i++)
                            {
                                if (elems[i].ToLower() != "and")
                                {
                                    if (elems[i].ToLower() != "or")
                                    {
                                        if (elems[i].Contains("=="))
                                        {
                                            conditions2.Add(elems[i].Split("==")[0], elems[i].Split("==")[1]);
                                            conditions3.Add(elems[i].Split("==")[0], "==");
                                        }
                                        else if (elems[i].Contains(">="))
                                        {
                                            conditions2.Add(elems[i].Split(">=")[0], elems[i].Split(">=")[1]);
                                            conditions3.Add(elems[i].Split(">=")[0], ">=");
                                        }
                                        else if (elems[i].Contains("<="))
                                        {
                                            conditions2.Add(elems[i].Split("<=")[0], elems[i].Split("<=")[1]);
                                            conditions3.Add(elems[i].Split("<=")[0], "<=");
                                        }
                                        else if (elems[i].Contains(">"))
                                        {
                                            conditions2.Add(elems[i].Split(">")[0], elems[i].Split(">")[1]);
                                            conditions3.Add(elems[i].Split(">")[0], ">");
                                        }
                                        else if (elems[i].Contains("<"))
                                        {
                                            conditions2.Add(elems[i].Split("<")[0], elems[i].Split("<")[1]);
                                            conditions3.Add(elems[i].Split("<")[0], "<");
                                        }
                                    }
                                    else
                                    {
                                        opp = "or";
                                    }
                                }
                                else
                                {
                                    opp = "and";
                                }
                            }
                            /*Console.Write("conditions : ");
                            foreach (var condition in conditions2)
                            {
                                Console.Write(condition.Key + " " + condition.Value + " | ");
                            }
                            Console.WriteLine();
                            Console.Write("conditions opp : ");
                            foreach (var condition in conditions3)
                            {
                                Console.Write(condition.Key + " " + condition.Value + " | ");
                            }*/
                            rechercher(table).clear(conditions2,conditions3, opp);
                        }
                        else return false;
                    }
                    else
                    {
                        rechercher(table).clear();
                    }
                }
                else
                {
                    return false;
                }
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
