using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration_Studio.Classes
{
    public class Connection
    {
        public string name;
        public string host;
        public string username;
        public string port;

        public Connection(string _name,string _host,string _username,string _port)
        {
            name = _name;
            host = _host;
            username = _username;
            port = _port;
        }


        public string Encode()
        { 
            return "{\"name\":\""+name+ "\",\"host\":\"" + host + "\",\"username\":\"" + username + "\",\"port\":\"" + port + "\"}";
        }

        public void Decode(string arg)
        {

        }
    }
}
