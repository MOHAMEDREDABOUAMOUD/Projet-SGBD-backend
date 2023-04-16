using Projet_SGBD_backend.enums;
using Projet_SGBD_backend.models;
using Projet_SGBD_backend.services;
using Projet_SGBD_backend.services.interfaces;

namespace Projet_SGBD_backend
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StructTable table = new StructTable("t1");
            table.add("t1f1", TypeField.Text, Constraint.PrimaryKey);
            table.add("t1f2", TypeField.Integer, Constraint.NotNull);
            StructTable table1 = new StructTable("t2");
            table1.add("t2f3", TypeField.Text, Constraint.PrimaryKey);
            table1.add("t2f4", TypeField.Integer, Constraint.NotNull);
            //IStructDatabase database = new StructDatabase("db1");
            //database.add(table);
            //database.add(table1);
            //database.remove("t1");
            //database.modify("t2", "t1");
            //database.load();
            //database.ShowTables();
            //database.save();
            //Console.WriteLine(table.rechercher("f1"));
            //table.remove("f2");
            //table.modify("f1",TypeField.Text,Constraint.PrimaryKey);
            //table.Describe();

            /*Table tablee = new Table(table);
            Table tablee1 = new Table(table1);
            Row row = new Row();
            row.add("t1f1e1");
            row.add("t1f2e1");
            Row row1 = new Row();
            row1.add("t2f3e2");
            row1.add("t2f4e2");*/

            Database database = new Database("db1");
            /*database.add(tablee);
            database.add(tablee1);
            database.rechercher("t1").add(row);
            database.rechercher("t2").add(row1);*/

            database.load();
            //database.rechercher("t1").print();

            database.executeQuery("select t1f1 from t1");
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
            database.executeQuery("select * from t2");

            //database.save();
        }
    }
}