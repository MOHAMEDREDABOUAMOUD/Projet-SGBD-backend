using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Projet_SGBD_backend.enums;
using Projet_SGBD_backend.models;
using Projet_SGBD_backend.services.interfaces;

namespace Projet_SGBD_backend.services
{
    [Serializable]
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

        public Table getTable(int index)
        {
            return tables[index];
        }

        public int getSizeTables()
        {
            return tables.Count;
        }

        public bool add(Table table)
        {
            tables.Add(table);
            return true;
        }

        public bool addTable(string name, List<string> elems)
        {
            StructTable t = new StructTable(name);
            foreach (string element in elems)
            {
                List<string> elem = element.Split(' ').ToList();
                if (elem.Count == 3)
                {
                    TypeField type=TypeField.Text;
                    switch (elem[1].ToLower())
                    {
                        case "int":
                            type = TypeField.Integer;
                            break;
                        case "varchar":
                            type = TypeField.Text;
                            break;
                        case "float":
                            type = TypeField.Reel;
                            break;
                        case "date":
                            type= TypeField.Date;
                            break;
                    }
                    enums.Constraint constraint=enums.Constraint.NotNull;
                    switch (elem[2].ToLower())
                    {
                        case "primarykey":
                            constraint= enums.Constraint.PrimaryKey;
                            break;
                        case "null":
                            constraint=enums.Constraint.Null;
                            break;
                        case "notnull":
                            constraint = enums.Constraint.NotNull;
                            break;
                        case "uniq":
                            constraint = enums.Constraint.Unique;
                            break;
                    }
                    t.add(elem[0], type, constraint);
                }
                else if (elem.Count == 2)
                {
                    TypeField type=TypeField.Text;
                    switch (elem[1].ToLower())
                    {
                        case "int":
                            type = TypeField.Integer;
                            break;
                        case "varchar":
                            type = TypeField.Text;
                            break;
                        case "float":
                            type = TypeField.Reel;
                            break;
                        case "date":
                            type = TypeField.Date;
                            break;
                    }
                    t.add(elem[0], type, enums.Constraint.NotNull);
                }
                else return false;
            }
            add(new Table(t));
            return true;
        }

        public List<List<string>> executeQuery(string query)
        {
            string orderby = "";
            string ordered = "";
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
                                if (elems[i].ToLower()=="order" && elems[i+1].ToLower()=="by" && (elems[i+3].ToLower()=="asc" || elems[i+3].ToLower()=="desc"))
                                {
                                    orderby = elems[i + 2];
                                    ordered = elems[i + 3];
                                }
                                else if (elems[i].ToLower() != "or")
                                {
                                    if (elems[i].Contains("<>"))
                                    {
                                        conditions2.Add(elems[i].Split("<>")[0], elems[i].Split("<>")[1]);
                                        conditions3.Add(elems[i].Split("<>")[0], "<>");
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
                                    else if (elems[i].Contains("="))
                                    {
                                        //conditions2.Add(elems[i].Split("=")[0], elems[i].Split("=")[1].Substring(1, elems[i].Split("=")[1].Length - 2));
                                        conditions2.Add(elems[i].Split("=")[0], elems[i].Split("=")[1]);
                                        conditions3.Add(elems[i].Split("=")[0], "=");
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
                        if (orderby == "")
                        {
                            return rechercher(table).select(columns.ToList(), conditions2, conditions3, opp);
                        }
                        else
                        {
                            return order(rechercher(table).select(columns.ToList(), conditions2, conditions3, opp), orderby, ordered);
                        }
                    }
                    else if (elems[4].ToLower() == "order" && elems[5].ToLower() == "by" && (elems[7].ToLower() == "asc" || elems[7].ToLower() == "desc"))
                    {
                        orderby = elems[6];
                        ordered = elems[7];
                        if (orderby != "")
                        {
                            return order(rechercher(table).select(columns.ToList(), new Dictionary<string, string>(), new Dictionary<string, string>()), orderby, ordered);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return rechercher(table).select(columns.ToList(), new Dictionary<string, string>(), new Dictionary<string, string>());
                }
            }
            else
            {
                return null;
            }
            return null;
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
                        return rechercher(table).insert(row);
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
                        if (elems.Length > i)
                        {
                            if (elems[i].ToLower() == "where")
                            {
                                for (int j = i + 1; j < elems.Length; j++)
                                {
                                    if (elems[j].ToLower() != "and")
                                    {
                                        if (elems[j].ToLower() != "or")
                                        {
                                            if (elems[j].Contains("<>"))
                                            {
                                                conditions.Add(elems[j].Split("<>")[0], elems[j].Split("<>")[1]);
                                                conditions3.Add(elems[j].Split("<>")[0], "<>");
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
                                            else if (elems[j].Contains("="))
                                            {
                                                conditions.Add(elems[j].Split("=")[0], elems[j].Split("=")[1]);
                                                conditions3.Add(elems[j].Split("=")[0], "=");
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
                        }
                        return rechercher(table).update(newValues,conditions,conditions3, opp);
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
                                        if (elems[i].Contains("<>"))
                                        {
                                            conditions2.Add(elems[i].Split("<>")[0], elems[i].Split("<>")[1]);
                                            conditions3.Add(elems[i].Split("<>")[0], "<>");
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
                                        else if (elems[i].Contains("="))
                                        {
                                            conditions2.Add(elems[i].Split("=")[0], elems[i].Split("=")[1]);
                                            conditions3.Add(elems[i].Split("=")[0], "=");
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
                            return rechercher(table).delete(conditions2,conditions3, opp);
                        }
                        else return false;
                    }
                    else
                    {
                        return rechercher(table).delete();
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

        public void load(string path= "")
        {
            if (path != "")
            {
                FileStream fs2 = new FileStream(path, FileMode.Open);
                Database backup = JsonSerializer.Deserialize(fs2, typeof(Database)) as Database;
                fs2.Close();
                Name = backup.Name;
                tables = backup.Tables;
            }
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

        public void save(string path = "")
        {
            if (path != "")
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                JsonSerializer.Serialize(fs, this);//une methode static du class jsonSerializer
                fs.Close();
            }
        }

        public void ShowTables()
        {
            Console.Write("name : " + Name + "\n tables : |");
            foreach (Table table in tables)
            {
                table.StructTable.Describe();
            }
        }
        List<List<string>> order(List<List<string>> data, string orderby, string ordered)
        {
            int col = 0;
            for(int i = 0; i < data[0].Count; i++)
            {
                if (data[0][i] == orderby)
                {
                    col = i;
                    break;
                }
            }
            List<string> pourPremutation;
            for(int i = 1; i < data.Count; i++)
            {
                for(int j = i+1; j < data.Count; j++)
                {
                    if (data[i][col].GetType() == typeof(string))
                    {
                        if (string.Compare(data[i][col],data[j][col])>0 && ordered=="asc")
                        {
                            Console.WriteLine("permuter");
                            pourPremutation = data[i];
                            data[i] = data[j];
                            data[j] = pourPremutation;

                        }
                        else if (string.Compare(data[i][col], data[j][col]) < 0 && ordered == "desc")
                        {
                            Console.WriteLine("permuter");
                            pourPremutation = data[i];
                            data[i] = data[j];
                            data[j] = pourPremutation;

                        }
                    }
                    else if (data[i][col].GetType() == typeof(int))
                    {
                        if (int.Parse(data[i][col]) > int.Parse(data[j][col]) && ordered == "asc")
                        {
                            Console.WriteLine("permuter");
                            pourPremutation = data[i];
                            data[i] = data[j];
                            data[j] = pourPremutation;

                        }
                        else if (int.Parse(data[i][col]) < int.Parse(data[j][col]) && ordered == "desc")
                        {
                            Console.WriteLine("permuter");
                            pourPremutation = data[i];
                            data[i] = data[j];
                            data[j] = pourPremutation;

                        }
                    }
                    else if (data[i][col].GetType() == typeof(float))
                    {
                        if (float.Parse(data[i][col]) > float.Parse(data[j][col]) && ordered == "asc")
                        {
                            Console.WriteLine("permuter");
                            pourPremutation = data[i];
                            data[i] = data[j];
                            data[j] = pourPremutation;

                        }
                        else if (float.Parse(data[i][col]) < float.Parse(data[j][col]) && ordered == "desc")
                        {
                            Console.WriteLine("permuter");
                            pourPremutation = data[i];
                            data[i] = data[j];
                            data[j] = pourPremutation;

                        }
                    }
                }
            }
            foreach (var d in data)
            {
                foreach (var d1 in d)
                {
                    Console.Write(d1+" | ");
                }
                Console.WriteLine();
            }
            return data;
        }
    }
}
