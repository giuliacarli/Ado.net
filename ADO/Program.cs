using System;

namespace ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConnectedMode.ConnectedWithParameter();
            //ConnectedMode.ConnectedStoredProcedure();
            //ConnectedMode.ConnectedScalar();
            DisconnectedMode.Disconnected();
            ConnectedMode.Connected(); 
        }
    }
}
