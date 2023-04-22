using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Projet_SGBD_backend.enums;
using Projet_SGBD_backend.models;
using Projet_SGBD_backend.services.interfaces;

namespace Projet_SGBD_backend.services
{
    public class Table : ITable
    {
        List<Row> rows;
        StructTable structTable;

        public Table(StructTable structTable)
        {
            this.structTable = structTable;
            rows = new List<Row>();
        }

        public List<Row> Rows { get => rows; set => rows = value; }
        public StructTable StructTable { get => structTable; set => structTable = value; }

        private bool isRep(int i,string value)
        {
            for (int j = 0; j < rows.Count; j++)
            {
                if (rows[j].get(i) == value) return false;
            }
            return true;
        }

        public bool add(Row row)
        {
            bool canAdd = true;
            for (int i = 0; i < row.getSize(); i++)
            {
                if(structTable.getField(i).Constr==Constraint.PrimaryKey || structTable.getField(i).Constr == Constraint.Unique) {
                    canAdd = isRep(i,row.get(i));
                }
                if (structTable.getField(i).Type == TypeField.Integer)
                {
                    if(!int.TryParse(row.get(i),out int res)) canAdd = false;
                }
                else if(structTable.getField(i).Type == TypeField.Reel)
                {
                    if (!double.TryParse(row.get(i), out double res)) canAdd = false;
                }
                else if(structTable.getField(i).Type == TypeField.Date)
                {
                    try
                    {
                        if(row.get(i).Split('-').Length!=3) canAdd = false;
                        else
                        {
                            foreach (string s in row.get(i).Split('-'))
                            {
                                if (!int.TryParse(s, out int res)) canAdd = false;
                            }
                        }
                    }
                    catch (Exception e){
                        Console.WriteLine(e);
                    }
                }
            }
            if(canAdd)
            {
                rows.Add(row);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool modify(int colonne, string value, string newValue)
        {
            Row row = rechercher(colonne, value);
            row.modify(colonne, newValue);
           //row.Elems[colonne] = newValue;
            return true;
        }

        public Row rechercher(int colonne, string value)
        {
            foreach (Row row in rows)
            {
                if (row.get(colonne) == value) return row;
            }
            return null;
        }

        public bool remove(int colonne, string value)
        {
            Row row = rechercher(colonne, value);
            rows.Remove(row);
            return true;
        }
        public void print(List<string> values,Dictionary<string,string> conditions)
        {
            List<string> fields=new List<string>();
            List<int> cols=new List<int>();
            Dictionary<int,dynamic> colsCondit = new Dictionary<int, dynamic>();
            int i = 0;
            foreach (Field field in structTable.Fields)
            {
                if (values.Contains(field.Name) || values[0]=="*")
                {
                    fields.Add(field.Name);
                    cols.Add(i);
                }
                if (conditions.Keys.Contains(field.Name))
                {
                    if (conditions[field.Name].Contains("'"))
                    {
                        colsCondit.Add(i, conditions[field.Name].Substring(1, conditions[field.Name].Length - 2) );
                    }
                    else
                    {
                        double res=0;
                        bool isdouble = double.TryParse(conditions[field.Name],out res);
                        if (isdouble) colsCondit.Add(i, res);
                        else {
                            int res1 = 0;
                            bool isint=int.TryParse(conditions[field.Name], out res1);
                            if (isint) colsCondit.Add(i, res1);
                            else Console.WriteLine("error");
                        }
                    }
                }
                i++;
            }
            Console.WriteLine();
            foreach (string field in fields)
            {
                Console.Write(field+" | ");
            }
            Console.WriteLine();
            foreach (Row row in rows)
            {
                bool res = true;
                foreach(KeyValuePair<int,dynamic> col in colsCondit)
                {
                    int res1;double res2;
                    if (structTable.getField(col.Key).Type == TypeField.Integer)
                    {
                        int.TryParse(row.get(col.Key), out res1);
                        if (res1 != col.Value) res = false;
                    }
                    else if (structTable.getField(col.Key).Type == TypeField.Reel) 
                    { 
                        double.TryParse(row.get(col.Key), out res2);
                        if (res2 != col.Value) res = false;
                    }

                    
                }
                if (res)
                {
                    foreach (int col in cols)
                    {
                        Console.Write(row.get(col) + " | ");
                    }
                    Console.WriteLine();
                }
            }
        }

        public void clear(Dictionary<string, string> args = null)
        {
            if (args == null)
            {
                rows.Clear();
            }
            else
            {
                Dictionary<int, string> colsCondit = new Dictionary<int, string>();
                int i = 0;
                foreach (Field field in structTable.Fields)
                {
                    if (args.Keys.Contains(field.Name))
                    {
                        colsCondit.Add(i, args[field.Name]);
                    }
                    i++;
                }
                foreach (Row row in rows)
                {
                    foreach(KeyValuePair<int,string> col in colsCondit)
                    {
                        if (row.get(col.Key) == col.Value)
                        {
                            rows.Remove(row);
                            break;
                        }
                    }
                }
            }
        }
        public void update(Dictionary<string, string> newValues = null, Dictionary<string, string> conditions = null)
        {
            if(conditions!=null && newValues != null)
            {
                Dictionary<int, string> colsCondit = new Dictionary<int, string>();
                Dictionary<int, string> colsNew = new Dictionary<int, string>();
                int i = 0;
                foreach (Field field in structTable.Fields)
                {
                    if (newValues.Keys.Contains(field.Name))
                    {
                        colsNew.Add(i, newValues[field.Name]);
                    }
                    if (conditions.Keys.Contains(field.Name))
                    {
                        colsCondit.Add(i, conditions[field.Name]);
                    }
                    i++;
                }
                foreach (Row row in rows)
                {
                    foreach(KeyValuePair<int,string> col in colsCondit)
                    {
                        Console.WriteLine("enter" + col.Key + " " + col.Value);
                        if (row.get(col.Key) == col.Value)
                        {
                            foreach (KeyValuePair<int, string> neww in colsNew)
                            {
                                row.modify(neww.Key, neww.Value);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
