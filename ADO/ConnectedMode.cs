using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO
{
    public class ConnectedMode
    {                                   //non salva la password          //usa le credenziali del nostro account
        const string connectionString = @"Persist Security Info = False; Integrated Security = true; Initial Catalog = CinemaDb; Server = WINAP6A66HJRUSC\SQLEXPRESS";

        public static void Connected()
        {
            // creare una connessione

            // Metodo 1:
            // SqlConnection connection = new SqlConnection();
            // connection.ConnectionString = connectionString;

            // Metodo 2
            // SqlConnection connection = new SqlConnection(connectionString);

            // Metodo 3
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // aprire la connessione

                connection.Open();

                // creare un command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM Movies";


                // creare parametri
                // eseguire command -> DataReader
                SqlDataReader reader = command.ExecuteReader();

                // leggere i dati --- li legge riga per riga

                while (reader.Read())
                {
                    Console.WriteLine(" {0} - {1} {2} {3}",
                        reader["ID"],
                        reader["Titolo"],
                        reader["Genere"],
                        reader["Durata"]);
                }
                // chiudere connessione e reader
                reader.Close();
                connection.Close();
            }



        }

        public static void ConnectedWithParameter()
        {
            // Creare connessione
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Inserimento parametro da riga di comando

                Console.WriteLine("Genere del Film: ");
                string Genere;
                Genere = Console.ReadLine();

                //Aprire la connessione 
                connection.Open();

                // Creare il command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM Movies WHERE Genere = @Genere";

                // Creare Parametro
                SqlParameter genereParam = new SqlParameter();
                genereParam.ParameterName = "@Genere";
                genereParam.Value = Genere;
                command.Parameters.Add(genereParam);

                //secondo modo per creare il parametro
                //command.Parameters.AddWithValue("@Genere", Genere);

                // Eseguire il command
                SqlDataReader reader = command.ExecuteReader();

                // Lettura dei dati
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} {2}",
                        reader["ID"],
                        reader["Titolo"],
                        reader["Genere"]);
                }

                // Chiudere connessione e reader
                reader.Close();
                connection.Close();

            }
        }

        public static void ConnectedStoredProcedure()
        {
            // Creare connessione
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Aprire una connessione
                connection.Open();

                // Creare Command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "stpGetActorsByCachetRange";

                // Creare Parametri
                command.Parameters.AddWithValue("@min_cachet", 5000);
                command.Parameters.AddWithValue("@max_cachet", 9000);

                // Creare valore di ritorno
                SqlParameter returnValue = new SqlParameter();
                returnValue.ParameterName = "@returnedCount";
                returnValue.SqlDbType = System.Data.SqlDbType.Int;

                returnValue.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValue);

                // Eseguire il command
                //SqlDataReader reader = command.ExecuteReader();

                //// Visualizzazione dati
                //while (reader.Read())
                //{
                //    Console.WriteLine("{0} - {1} {2} {3}",
                //        reader["ID"],
                //        reader["FirstName"],
                //        reader["LastName"],
                //        reader["Cachet"]);
                //}

                //reader.Close();

                command.ExecuteNonQuery(); // faccio apparire nella console solo il parametro di ritorno, senza la tabella. se voglio la tabella devo mettere ExecuteReader() come sopra

                Console.WriteLine("#Actors: {0}", command.Parameters["@returnedCount"].Value);

                connection.Close();
            }
        }

        public static void ConnectedScalar()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand scalarCommand = new SqlCommand();
                scalarCommand.Connection = connection;
                scalarCommand.CommandType = System.Data.CommandType.Text;
                scalarCommand.CommandText = "SELECT COUNT(*) FROM Movies";

                int count = (int)scalarCommand.ExecuteScalar(); // scalarCommand.ExecuteScalar() restituisce un oggetto, facendo (int) lo andiamo a convertire

                Console.WriteLine("Conteggio dei film: {0}", count);

                connection.Close();
            }
        }
    }
}
