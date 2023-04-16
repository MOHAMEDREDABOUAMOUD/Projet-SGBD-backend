using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_SGBD_backend.models
{
    public class Row
    {
        List<string> elems;

        public List<string> Elems { get => elems; set => elems = value; }

        public Row()
        {
            elems = new List<string>();
        }

        public string get(int index)
        {
            return elems[index];
        }

        public void add(string elem)
        {
            elems.Add(elem);
        }
        public void delete(string elem)
        {
            elems.Remove(elem);
        }
        public void modify(int col, string newValue)
        {
            elems[col]=newValue;
        }

       // public List<string> Elems { get => elems; set => elems = value; }
    }
}
