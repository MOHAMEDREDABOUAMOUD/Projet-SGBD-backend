﻿using Projet_SGBD_backend.enums;
using Projet_SGBD_backend.models;
using Projet_SGBD_backend.services;
using Projet_SGBD_backend.services.interfaces;

namespace Projet_SGBD_backend
{
    internal class Program
    {
        static List<Database> databases = new List<Database>();
        static void Main(string[] args)
        {
            /*StructTable table = new StructTable("t1");
            table.add("t1f1", TypeField.Text, Constraint.PrimaryKey);
            table.add("t1f2", TypeField.Integer, Constraint.NotNull);
            StructTable table1 = new StructTable("t2");
            table1.add("t2f3", TypeField.Text, Constraint.PrimaryKey);
            table1.add("t2f4", TypeField.Integer, Constraint.NotNull);*/


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
            row.add("Reda");
            row.add("10");
            Row row1 = new Row();
            row1.add("lastK");
            row1.add("1");*/

            //Database database = new Database("db1");
            /*database.add(tablee);
            database.add(tablee1);
            database.rechercher("t1").add(row);
            database.rechercher("t2").add(row1);*/

            //database.load();

            //database.rechercher("t1").print();

            /*database.executeQuery("select t1f1,t1f2 from t1 where t1f1=='Reda' or t1f2==1");
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
            database.executeQuery("select * from t1");*/

            /*database.executeQuery("select * from t1");
            database.executeUpdate("delete from t1 where t1f1=='Reda' or t1f2==1");
            database.executeQuery("select * from t1");*/

            /*database.executeQuery("select * from t1");
            database.executeUpdate("update t1 set t1f1='redaNew' where t1f1=='Reda' or t1f2==1");
            database.executeQuery("select * from t1");*/

            /*database.executeQuery("select * from t1");
            database.executeUpdate("insert into t1 values('reda1',1)");
            database.executeQuery("select * from t1");*/

            //database.save();

            //load();
            //databases[1].executeQuery("select * from ordinateur where id<>1");
        }
        static void load()
        {
            int index = 0;
            foreach (string file in Directory.GetFiles("databases"))
            {
                databases.Add(new Database(Path.GetFileName(file)));
                databases[index].load(file);
                index++;
            }
        }
    }
}