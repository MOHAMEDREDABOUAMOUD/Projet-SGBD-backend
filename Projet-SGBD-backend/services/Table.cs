using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        public bool add(Row row)
        {
            rows.Add(row);
            return true;
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
        public void print(params string[] values)
        {
            List<string> fields=new List<string>();
            List<int> cols=new List<int>();
            int i = 0;
            foreach (Field field in structTable.Fields)
            {
                if (values.Contains(field.Name) || values[0]=="*")
                {
                    fields.Add(field.Name);
                    cols.Add(i);
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
                foreach(int col in cols)
                {
                    Console.Write(row.get(col)+" | ");
                }
                Console.WriteLine();
            }
        }
    }
}
