using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;

namespace OOP_ExtraTaskShared
{
    [Serializable]
    public enum ServerProtocol
    {
        svc_noop,
        svc_login, // [int LoginErrors][int sessionId]
        svc_register, // [int status][string id]
        svc_accounthistory, // [data changes]
        svc_accountvalue, // [long value]
        svc_sendresponse, // [int error]
    }

    [Serializable]
    public enum ClientProtocol
    {
        clc_login, // [string login][string password]
        clc_register, // [string password]
        clc_getaccounthistory, // [int sessionId]
        clc_getaccountvalue, // [int sessionId]
        clc_send, // [int sessionId][string id][ulong sum]
        clc_disconnect
    }

    [Serializable]
    public enum LoginError
    {
        LOGIN_SUCCESS = 0,
        LOGIN_INVALIDPASSWD,
        LOGIN_ALREADYLOGIN
    }

    [Serializable]
    public enum SendResponseError
    {
        SEND_INVAL_ID = 0,
        SEND_NOTENOUGH,
        SEND_SUCCESS,
    }

    [Serializable]
    public enum RegisterError
    {
        REGISTER_SUCCESS = 0,
        REGISTER_UNKNOWN
    }

    [Serializable]
    public struct LoginResponse
    {
        public LoginError error;
        public int sessionId;
    }

    [Serializable]
    public struct RegisterResponse
    {
        public RegisterError error;
        public string id;
    }

    [Serializable]
    public struct SendMoney
    {
        public int sessionId;
        public string to; // empty if withdraw
        public long value;
    }

    [Serializable]
    public struct AccountChange
    {
        public bool isSent; // if true, money was sent. If false, money was got
        public string from;
        public string loginId; // money sent from or sent to. Empty if withdraw.
        public long account;
    }

    [Serializable]
    public struct LoginCredentials
    {
        public string login;
        public string passwd;
    }

    public static class Serializer
    {
        public static byte[] SerializeToByteArray( this object obj )
        {
            var ms = new MemoryStream();
            var serial = new BinaryFormatter();

            serial.Serialize(ms, obj);

            return ms.ToArray();
        }

        public static T DeserializeObject<T>(this byte[] src, out long size)
        {
            var ms = new MemoryStream(src);
            var serial = new BinaryFormatter();

            T t = (T)serial.Deserialize(ms);

           
            size = ms.Position;

            return t;
        }

        public static string SerializeToString(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static T DeserializeObject<T>(this string src)
        {
            return JsonConvert.DeserializeObject<T>(src);


        }
    }
}
