using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ADO
{
    public class DisconnectedMode
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security = true; Initial Catalog = CinemaDb; Server = WINAP6A66HJRUSC\SQLEXPRESS";

        public static void Disconnected()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Costruzione adapter
                SqlDataAdapter adapter = new SqlDataAdapter();

                // Creazione dei comandi da associare all'adapter
                SqlCommand selectCommand = new SqlCommand();
                selectCommand.Connection = connection;
                selectCommand.CommandType = System.Data.CommandType.Text;
                selectCommand.CommandText = "SELECT * FROM Movies";

                SqlCommand insertCommand = new SqlCommand();
                insertCommand.Connection = connection;
                insertCommand.CommandType = System.Data.CommandType.Text;
                insertCommand.CommandText = "INSERT INTO Movies VALUES(@Titolo, @Genere, @Durata)";

                insertCommand.Parameters.Add("@Titolo", System.Data.SqlDbType.NVarChar, 255, "Titolo");
                insertCommand.Parameters.Add("@Genere", System.Data.SqlDbType.NVarChar, 255, "Genere");
                insertCommand.Parameters.Add("@Durata", System.Data.SqlDbType.Int, 500, "Durata");

                //...

                // Associare i comandi all'adapter

                adapter.SelectCommand = selectCommand;
                adapter.InsertCommand = insertCommand;

                DataSet dataset = new DataSet();

                try
                {
                    connection.Open();
                    adapter.Fill(dataset, "Movies"); //prende la tabella Movies e mette i dati dentro il dataset

                    //foreach (DataRow row in dataset.Tables["Movies"].Rows)
                    //{
                    //    Console.WriteLine("Row: {0}", row["Titolo"]);
                    //}

                    // Creazione del record
                    DataRow movie = dataset.Tables["Movies"].NewRow();
                    movie["Titolo"] = "V per vendetta";
                    movie["Genere"] = "Azione";
                    movie["Durata"] = 125;

                    dataset.Tables["Movies"].Rows.Add(movie);

                    // Update del db
                    adapter.Update(dataset, "Movies"); // non è update ovvero il comando della query ma è un comando di C# che dice di aggiornare il db con le modifiche che abbiamo fatto

                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
